namespace RM.Menu.Records
{
    public record SaveMenuTreeRecord
    {
        public string ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public int? MenuOrder { get; set; }
        public string Url { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsHidden { get; set; }
        public string entityId { get; set; }
        public string articleId { get; set; }
        public string parentId { get; set; }
        public string referenceId { get; set; }
        public string referenceMajorId { get; set; }
        public bool? OpenBlankPage { get; set; }
        public string FontIcon { get; set; }
        public string ImageIcon { get; set; }
        public string SvgIcon { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
    }
}
