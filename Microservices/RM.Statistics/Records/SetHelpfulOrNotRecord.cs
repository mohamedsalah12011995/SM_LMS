namespace RM.Statistics.Records
{
    public record SetHelpfulOrNotRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string ItemUrl { get; set; }
        public string itemId { get; set; }
        public bool IsHelpful { get; set; }
    }
}
