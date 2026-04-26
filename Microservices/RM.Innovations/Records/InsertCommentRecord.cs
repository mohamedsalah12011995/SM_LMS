namespace RM.Innovations.Records
{
    public record InsertCommentRecord
    {
        public string ideaId { get; set; }
        public string Capcha { get; set; }
        public string CommenterName { get; set; }
        public string Text { get; set; }
        public string Email { get; set; }
        public bool? IsAgreeTerms { get; set; }

    }
}
