namespace RabbitMQ.Core.Dtos
{
    public class RabbitMQConfiguration
    {
        public string Host { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string VirtualHost { get; set; } = string.Empty;
        public string Queue { get; set; } = string.Empty;
        public string Exchange { get; set; } = string.Empty;
    }

}
