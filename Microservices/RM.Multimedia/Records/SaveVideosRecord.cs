namespace RM.Multimedia.Records
{
    public record SaveVideosRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Url { get; set; }
    }
}
