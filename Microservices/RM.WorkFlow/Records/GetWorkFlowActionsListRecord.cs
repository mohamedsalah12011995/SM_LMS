using RM.Core.Helpers;

namespace RM.WorkFlow.Records
{
    public record GetWorkFlowActionsListRecord
    {
        public string referenceId { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
        public bool? IsActive { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }
    }
}
