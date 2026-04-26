using DocumentFormat.OpenXml.Wordprocessing;
using RM.Core.Helpers;

namespace RM.Permits.Records
{
    public record GetPermitRequestsListRecord
    {
        public bool? IsExpiredFilter { get; set; }
        public string CarLitters { get; set; }
        public int? PermitState { get; set; }

        public int? PermitType { get; set; }
        public DateTime? PermitStartDate { get; set; }
        public DateTime? PermitEndDate { get; set; }
        public string IdentityNumber { get; set; }
        public string Code { get; set; }
        public string Nationality { get; set; }
        public string JobName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
