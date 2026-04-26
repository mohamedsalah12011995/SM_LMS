namespace RM.Innovations.Records
{
    public record TransferSuggestionToInnovationRecord
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public bool? IdeaExist { get; set; }
        public string Country { get; set; }
        public string City { get; set; }

        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string referenceId { get; set; }
    }
}
