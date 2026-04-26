namespace RM.Orders.Records
{
    public record AddOrderActionRecord
    {
        public string orderId {  get; set; }
        public string typeId { get; set; }
        public string Note { get; set; }

    }
}
