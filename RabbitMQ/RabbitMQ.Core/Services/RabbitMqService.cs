using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Core.Dtos;
using System.Text;

namespace RabbitMQ.Core.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly RabbitMQConfiguration _configuration;
        private readonly ILogger<RabbitMqService> logger;

        private IModel _channel = default!;
        private IConnection _connection = default!;
        public RabbitMqService(IOptions<RabbitMQConfiguration> options, ILogger<RabbitMqService> logger)
        {
            _configuration = options.Value;
            this.logger = logger;
        }
        public (IConnection channel, IModel model) CreateConnection()
        {
            var factory = new ConnectionFactory
            {
                UserName = _configuration.User,
                Password = _configuration.Password,
                HostName = _configuration.Host,
                VirtualHost = _configuration.VirtualHost,
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            return (_connection, _channel);
        }

        /// <summary>
        /// Durability: The queue and exchange are declared as non-durable (i.e., durable: false), meaning they will not survive a broker restart.
        /// Queue Arguments: No additional arguments are provided for the queue declaration.
        /// Usage: Suitable for use cases where you don't need message durability and persistence across RabbitMQ restarts.
        /// </summary>
        public ResultDto DeclareQueue_Classic(IModel model, string queue, string exchange, string routingKey, string exchangeType = ExchangeType.Direct)
        {
            var result = new ResultDto();
            try
            {
                model.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
                model.ExchangeDeclare(exchange, type: exchangeType, durable: false, autoDelete: false);
                model.QueueBind(queue, exchange, routingKey: routingKey);

                result.IsValid = true;
                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorException = ex; return result;
            }
        }

        /// <summary>
        /// Durability: The queue and exchange are declared as durable (i.e., durable: true), meaning they will survive a broker restart.
        /// Queue Arguments: Specifies the queue type as quorum by including the argument {"x-queue-type": "quorum"}.
        ///Usage: Suitable for use cases requiring higher availability and message durability, as quorum queues provide stronger guarantees for replicated messaging.
        /// </summary>

        public ResultDto DeclareQueue_Quorum(IModel model, string queue, string exchange, string routingKey, string exchangeType = ExchangeType.Direct)
        {
            var result = new ResultDto();
            try
            {
                // Declare the dead-letter exchange and queue
                model.ExchangeDeclare("dead_letter_exchange", ExchangeType.Direct, durable: true, autoDelete: false);
                model.QueueDeclare("dead_letter_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
                model.QueueBind("dead_letter_queue", "dead_letter_exchange", routingKey: "dead_letter_key");



                model.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false,
                    arguments: new Dictionary<string, object>
                {
                    { "x-queue-type", "quorum" },
                    { "x-dead-letter-exchange", "dead_letter_exchange" },
                    { "x-dead-letter-routing-key", "dead_letter_key" },
                  //  { "x-delivery-limit", 5 }
                });
                model.ExchangeDeclare(exchange, type: exchangeType, durable: true, autoDelete: false);
                model.QueueBind(queue, exchange, routingKey: routingKey);

                result.IsValid = true;
                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorException = ex;
                logger.LogError("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", ex.Message, ex.StackTrace, null);
                return result;
            }
        }

        public ResultDto DeclareQueue_Stream(IModel model, string queue, string exchange, string exchangeType = ExchangeType.Direct)
        {
            var result = new ResultDto(); try
            {
                model.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false,
                    arguments: new Dictionary<string, object> {
                        { "x-queue-type", "stream" }
                    });

                model.ExchangeDeclare(exchange, type: exchangeType, durable: true, autoDelete: false);
                model.QueueBind(queue, exchange, routingKey: "");

                result.IsValid = true;
                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorException = ex;
                logger.LogError("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", ex.Message, ex.StackTrace, null);

                return result;
            }
        }


        public async Task<ResultDto> ConsumeMessageAsync<T>(IModel model, string queue, bool requeueWhenFailure, Func<T, Task<ResultDto>> processingFunc, CancellationToken cancellationToken)
        {
            var result = new ResultDto();
            try
            {

                var consumer = new AsyncEventingBasicConsumer(model);
                consumer.Received += async (ch, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var deserializedMessage = JsonConvert.DeserializeObject<T>(message);
                    var processingResult = await processingFunc(deserializedMessage);

                    if (processingResult.IsValid)
                    {
                        model.BasicAck(ea.DeliveryTag, false);
                        logger.LogInformation("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", $"Message processed and acknowledged: {message}", null, null);

                    }
                    else
                    {
                        var headers = ea.BasicProperties.Headers;
                        if (headers != null && headers.ContainsKey("x-death"))
                        {
                            var deathCount = (headers["x-death"] as IList<object>)?.Count ?? 0;
                            if (deathCount >= 5) // Max requeue attempts
                            {
                                model.BasicReject(ea.DeliveryTag, false); // Move to dead-letter queue
                                logger.LogInformation("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", $"Message moved to dead-letter queue: {message}", null, null);
                            }
                            else
                            {
                                model.BasicNack(ea.DeliveryTag, false, requeueWhenFailure);
                                logger.LogInformation("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", $"Message processing failed and requeued: {message}", null, null);
                            }
                        }
                        else
                        {
                            model.BasicNack(ea.DeliveryTag, false, requeueWhenFailure);
                            logger.LogInformation("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", $"Message processing failed and requeued: {message}", null, null);

                        }
                    }

                    result = processingResult;
                };

                model.BasicConsume(queue, false, consumer);

                var tcs = new TaskCompletionSource<bool>();
                cancellationToken.Register(() => tcs.SetResult(true));
                await tcs.Task;

                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result.ErrorException = ex;
                result.IsValid = false;
                logger.LogError("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", ex.Message, ex.StackTrace, null);
            }

            return result;
        }


        public ResultDto SendMessage<T>(IModel model, string exchange, T data, string routingKey = "", TimeSpan? expirationTime = null, IBasicProperties basicProperties = null)
        {
            var result = new ResultDto();
            try
            {
                var properties = basicProperties ?? model.CreateBasicProperties();
                properties.Persistent = true;

                if (expirationTime.HasValue)
                {
                    properties.Expiration = ((int)expirationTime.Value.TotalMilliseconds).ToString();
                }

                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
                model.BasicPublish(exchange: exchange, routingKey: routingKey, basicProperties: properties, body: body);

                result.IsValid = true;
                logger.LogInformation("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", $"Message sent with expiration: {properties.Expiration}", null, null);

            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.ErrorException = ex;
                logger.LogError("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", ex.Message, ex.StackTrace, null);
            }
            return result;
        }


        public void Dispose()
        {
            if (_channel != null)
            {
                _channel.Close();
                _channel.Dispose();
            }
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }

        }
    }
}
