namespace RM.Surveys.Records
{
    public record SaveSurveyRecord
    {
        public string ID { get; set; }
        public string referenceID { get; set; }
        public string entityID { get; set; }
        public string themeId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool? ShowInHomePage { get; set; }
        public bool? UseCapcha { get; set; }
        public bool? InnerOnly { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ImageBase64 { get; set; }
    }
}
