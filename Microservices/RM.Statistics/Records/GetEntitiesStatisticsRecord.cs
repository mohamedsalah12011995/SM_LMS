namespace RM.Statistics.Records
{
    public record GetEntitiesStatisticsRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string ItemUrl { get; set; }

    }
}
