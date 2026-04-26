using RM.Core.Helpers;

namespace RM.Surveys.Records
{
    public record GetSurveysListRecord
    {

        public string referenceID { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? ShowInHomePage { get; set; }
        public bool? UseCapcha { get; set; }
        public bool? InnerOnly { get; set; }
        public bool? IsActive { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }
    }
}
