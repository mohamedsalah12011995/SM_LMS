namespace RM.Articles.Records
{
    public record SaveArticleRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string MenuCode { get; set; }
        public bool? ShowBySearch { get; set; }
    }
}
