

namespace RM.InitiativesProjects.Records
{
    public record SaveInitiativesProjectRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string GoalsAr { get; set; }
        public string GoalsEn { get; set; }
        public string LocationAr { get; set; }
        public string LocationEn { get; set; }
        public string typeId { get; set; }
        public DateTime? InitiativeDate { get; set; }


        public List<SaveBeneficiariesRecord> ListOfBeneficiaries { get; set; } = new List<SaveBeneficiariesRecord>();

    }

    public record SaveBeneficiariesRecord
    {
        public string ID { get;set; }
    }
}
