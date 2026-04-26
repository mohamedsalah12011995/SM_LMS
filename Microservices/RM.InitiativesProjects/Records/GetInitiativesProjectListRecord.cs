using RM.Core.Helpers;

namespace RM.InitiativesProjects.Records
{
    public record GetInitiativesProjectListRecord
    {
        public string referenceId {  get; set; }
        public DateTime? InitiativeDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string typeId { get; set; }
        public string beneficiaryId { get; set; }
        public bool? IsActive { get; set; }
        public List<BeneficiariesRecord> ListOfBeneficiaries { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }

    }

    public record BeneficiariesRecord
    {
        public string ID { get; set; }
    }
}
