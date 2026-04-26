using RM.Core.Helpers;

namespace RM.ContactUs.Records
{
    public record GetContactListForQualityAssuranceRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? FromCreatedDate { get; set; }
        public DateTime? ToCreatedDate { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public int? FilterStatusId { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }
    }
}
