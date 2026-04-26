namespace RM.ExternalSites.Records
{
    public record SaveExternalSiteRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string parentId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Url { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string ImageBase64 { get; set; }

    }
}
