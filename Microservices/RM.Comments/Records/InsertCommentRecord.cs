namespace RM.Comments.Records
{
    public record InsertCommentRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string itemId { get; set; }
        public string ItemUrl { get; set; }
        public string Text { get; set; }
        public string CommenterName { get; set; }
        public string Email { get; set; }
        public bool? IsAgreeTerms { get; set; }

    }
}
