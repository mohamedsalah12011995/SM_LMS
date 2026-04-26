using RM.Core.Helpers;

namespace RM.Innovations.Records
{
    public record GetPublicIdeasListRecord
    {
        public string referenceId {  get; set; }
        public string actionId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsShow { get; set; }
        public string priority { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string category { get; set; }
        public bool? Capability { get; set; }
        public bool? NeedsBudget { get; set; }
        public bool? Feasibility { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }
    }
}
