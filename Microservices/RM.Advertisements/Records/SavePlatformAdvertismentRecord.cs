namespace RM.Advertisements.Records
{
    public record SavePlatformAdvertismentRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string regionId { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string Url { get; set; }
        public string Destination { get; set; }
        public string FileUrlBase64 { get; set; }
    }
}
