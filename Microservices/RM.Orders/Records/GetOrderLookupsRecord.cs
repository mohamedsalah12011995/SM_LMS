namespace RM.Orders.Records
{
    public record GetOrderLookupsRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
    }
}
