namespace RM.Orders.Records
{
    public record GetOrderRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string Code { get; set; }
    }
}
