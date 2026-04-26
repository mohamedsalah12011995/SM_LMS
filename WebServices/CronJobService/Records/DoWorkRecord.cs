namespace CronJobService.Records
{
    public record DoWorkRecord
    {
        public int CronTypeId { get; set; }
    }
}
