namespace RabbitMQ.Core.Dtos
{
    public class ResultDto
    {
        public bool IsValid { get; set; }
        public Exception? ErrorException { get; set; }
    }
}
