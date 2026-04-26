using RabbitMQ.Client;
using RabbitMQ.Core.Dtos;

namespace RabbitMQ.Core.Services
{
    public interface IRabbitMqService
    {
        (IConnection channel, IModel model) CreateConnection();

        ResultDto DeclareQueue_Classic(IModel model, string queue, string exchange, string routingKey = "", string exchangeType = ExchangeType.Fanout);
        ResultDto DeclareQueue_Quorum(IModel model, string queue, string exchange, string routingKey = "", string exchangeType = ExchangeType.Fanout);
        ResultDto DeclareQueue_Stream(IModel model, string queue, string exchange, string exchangeType = ExchangeType.Fanout);
        ResultDto SendMessage<T>(IModel model, string exchange, T data, string routingKey = "", TimeSpan? expirationTime = null, IBasicProperties basicProperties = null);
        Task<ResultDto> ConsumeMessageAsync<T>(IModel model, string queue, bool requeueWhenFailure, Func<T, Task<ResultDto>> processingFunc, CancellationToken cancellationToken);
        void Dispose();
    }
}
