using RM.News.Dtos;

namespace RM.News.Records
{
    public record SaveNewsRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public DateTime? NewsDate { get; set; }
        public string NewsSourceAr { get; set; }
        public string NewsSourceEn { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string NewsContentAr { get; set; }
        public string NewsContentEn { get; set; }
        public bool? ShowInHome { get; set; }
        public string OriginalPicBase64 { get; set; }
        public List<TagsDto> ListOfTags { get; set; }
        public List<ReferenceRecord> PublishEntity { get; set; }


    }

    public record ReferenceRecord
    {
        public string ID { get; set; }

    }
}
