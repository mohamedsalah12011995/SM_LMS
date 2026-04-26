using RM.Core.Helpers;

namespace RM.OpenData.Records
{
    public record GetOpenDataRequestListRecord
    {
        public string referenceId { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Details { get; set; }
        public string Address { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
