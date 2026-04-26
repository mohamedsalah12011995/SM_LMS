using RM.Core.Helpers;

namespace RM.Comments.Records
{
    public record GetAllCommentsRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string itemId { get; set; }
        public string ItemUrl { get; set; }
        public string CommenterName { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsApproved { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
