using DocumentFormat.OpenXml.Wordprocessing;
using RM.Core.Helpers;

namespace RM.Jobs.Records
{
    public record GetJobAdvertiesmentListRecord
    {
        public string referenceId {  get; set; }
        public int? Type { get; set; }
        public bool? IsContinuing { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public bool? IsActive { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }
    }
}
