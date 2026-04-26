using RM.Core.Helpers;

namespace RM.WorkFlow.Records
{
    public record GetFormDataListRecord
    {
        public string EntityId { get; set; }
        public string ReferenceId { get; set; }
        public string EntityUrl { get; set; }
        public string FormId { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


    }
}
