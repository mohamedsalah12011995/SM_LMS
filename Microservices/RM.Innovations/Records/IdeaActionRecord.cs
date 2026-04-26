namespace RM.Innovations.Records
{
    public record IdeaActionRecord
    {
        public string ID { get; set; }
        public string actionId { get; set; }
        public string category { get; set; }
        public string toReference { get; set; }
        public string ActionNote { get; set; }

    }
}
