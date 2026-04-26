using RM.Advertisements.Dtos;

namespace RM.Advertisements.Records
{
    public record SaveAdvertismentRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsPopup { get; set; }
        public bool? IsHomeSliderAd { get; set; }
        public string ImageUrlBase64 { get; set; }
        public string Url { get; set; }
        public List<ReferenceRecord> PublishEntity { get; set; } = new List<ReferenceRecord>();

    }

    public record ReferenceRecord
    {
        public string ID { get; set; }

    }
}
