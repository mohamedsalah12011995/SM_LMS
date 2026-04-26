
using RM.Core.Helpers;

namespace RM.ScientificLetters.Records
{
    public record GetScientificLettersListRecord
    {
        public string referenceId {  get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public string ResearcherNameAr { get; set; }
        public string ResearcherNameEn { get; set; }
        public string ResearchPlaceAr { get; set; }
        public string ResearchPlaceEn { get; set; }

        public bool? IsActive { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }
    }
}
