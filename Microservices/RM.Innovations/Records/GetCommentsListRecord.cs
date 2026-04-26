using RM.Core.Helpers;

namespace RM.Innovations.Records
{
    public record GetCommentsListRecord
    {
        public DateTime? CreatedDate { get; set; }
        public string CommenterName { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public bool? IsApproved { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }
}
