using RM.Core.Helpers;

namespace RM.Officials.Records
{
    public record GetOfficialsListRecord
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string PersonCreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }
    }
}
