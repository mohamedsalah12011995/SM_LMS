
namespace RM.Menu.Records
{
    public record SaveMenuRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string referenceMajorId { get; set; }
        public string typeId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Url { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsHidden { get; set; }
        public List<SaveSubMenusRecord> SubMenus { get; set; }

    }
    public record SaveSubMenusRecord
    {
        public string ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? MenuOrder { get; set; }
        public string Url { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsHidden { get; set; }
        public string entityId { get; set; }
        public string articleId { get; set; }
        public bool? OpenBlankPage { get; set; }
        public string FontIcon { get; set; }
        public string ImageIcon { get; set; }
        public string SvgIcon { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public List<SaveSubMenusRecord> SubMenus { get; set; }

    }

}
