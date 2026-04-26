namespace RM.Jobs.Records
{
    public record SendJobApplicationNotificationRecord
    {
        public List<JobApplicationNotificationRecord> JobApplicationList { get; set; } = new List<JobApplicationNotificationRecord>();
        public string NotificationMessage { get; set; }
    }
    public record JobApplicationNotificationRecord
    {
        public string ID { get; set; }
    }
}
