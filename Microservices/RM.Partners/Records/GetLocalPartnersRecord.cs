using RM.Core.Helpers;

namespace RM.Partners.Records
{
    public record GetLocalPartnersRecord
    {
        public string entityId { get; set; }
        public string referenceId { get; set; }
        public DateTime? ContractDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string RmdepartmentNameEn { get; set; }
        public string RmdepartmentNameAr { get; set; }
        public string PartnershipTitleAr { get; set; }
        public string PartnershipTitleEn { get; set; }
        public bool? ContractActive { get; set; }
        public bool? IsActive { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }
    }
}
