namespace RM.Statistics.Records
{
    public record GetPortalLatestUpdateRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public int? StatisticsType { get; set; }


    }
}
