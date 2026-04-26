using RM.Core.Helpers;

namespace RM.Awards.Records
{
    public record GetAwardsListRecord
    {
        public string referenceId {  get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
