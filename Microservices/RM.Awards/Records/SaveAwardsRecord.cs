namespace RM.Awards.Records
{
    public record SaveAwardsRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string IconUrlBase64 { get; set; }

    }
}
