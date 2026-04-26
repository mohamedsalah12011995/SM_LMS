using DocumentFormat.OpenXml.Wordprocessing;
using LinqKit;
using RM.Core.Helpers;

namespace RM.WorkFlow.Records
{
    public record GetEnginesListRecord
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
