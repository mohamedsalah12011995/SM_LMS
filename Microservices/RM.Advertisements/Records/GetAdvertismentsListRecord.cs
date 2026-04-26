using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using RM.Core.Helpers;

namespace RM.Advertisements.Records
{
    public record GetAdvertismentsListRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsPopup { get; set; } = false;
        public bool? IsHomeSliderAd { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
