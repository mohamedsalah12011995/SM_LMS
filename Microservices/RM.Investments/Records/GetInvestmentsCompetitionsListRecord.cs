using RM.Core.Helpers;

namespace RM.Investments.Records
{
    public record GetInvestmentsCompetitionsListRecord
    {
        public string entityId { get; set; }
        public string referenceId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? OpportunityDate { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Price { get; set; }
        public string opportunityType { get; set; }
        public string OpportunityReference { get; set; }
        public string OpportunityNo { get; set; }
        public bool? IsActive { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }
    }
}
