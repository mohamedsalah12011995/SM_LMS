using RM.Core.Helpers;

namespace RM.Orders.Records
{
    public record GetOrderListRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string Subject { get; set; }
        public string Details { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string createdBy { get; set; }
        public string Code { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


    }
}
