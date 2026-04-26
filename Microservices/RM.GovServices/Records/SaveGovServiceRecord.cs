namespace RM.GovServices.Records
{
    public record SaveGovServiceRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string parentId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string SiteNameAr { get; set; }
        public string SiteNameEn { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }

        public string AudienceEn { get; set; }
        public string AudienceAr { get; set; }
        public string ServiceChannelEn { get; set; }
        public string ServiceChannelAr { get; set; }
        public string RequirementsEn { get; set; }
        public string RequirementsAr { get; set; }
        public string StepsEn { get; set; }
        public string StepsAr { get; set; }
        public string ServiceUrl { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
