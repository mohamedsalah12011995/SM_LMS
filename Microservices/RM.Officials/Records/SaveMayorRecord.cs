namespace RM.Officials.Records
{
    public record SaveMayorRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string OriginalPicBase64 { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public string Email { get; set; }
        public string JobTitleAr { get; set; }
        public string JobTitleEn { get; set; }
        public string CvUrlBase64 { get; set; }
    }
}
