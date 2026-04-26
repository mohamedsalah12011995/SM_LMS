namespace RM.Orders.Records
{
    public record AddOrderRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
    }
}
