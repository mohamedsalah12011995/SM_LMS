namespace RM.Comments.Records
{
    public record ApproveRejectCommentRecord
    {
        public string ID { get; set; }
        public bool? IsApproved { get; set; }

    }
}
