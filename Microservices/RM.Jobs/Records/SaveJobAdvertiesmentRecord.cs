namespace RM.Jobs.Records
{
    public record SaveJobAdvertiesmentRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }

        public int? Type { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<JobCareerRecord> jobCareers { get; set; } = new List<JobCareerRecord>();

    }

    public record JobCareerRecord
    {
        public string ID { get; set; }
        public string jobAdvertisementId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string JobLocationAr { get; set; }
        public string JobLocationEn { get; set; }
        public string JobCondationsAr { get; set; }
        public string JobCondationsEn { get; set; }
        public int? MaxLimit { get; set; }
        public string qualificationId { get; set; }
        public int? Type { get; set; }

        public bool? IsNoticeBeneficiaries { get; set; }
        public List<jobLookUpRecord> ListOfTags { get; set; } = new List<jobLookUpRecord>();

        public List<jobLookUpRecord> ListOfSkills { get; set; } = new List<jobLookUpRecord>();
        public List<jobLookUpRecord> ListOfSpecifications { get; set; } = new List<jobLookUpRecord>();

    }

    public record jobLookUpRecord
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
    }
}
