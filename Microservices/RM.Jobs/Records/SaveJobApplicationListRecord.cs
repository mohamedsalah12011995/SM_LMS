namespace RM.Jobs.Records
{
    public record SaveJobApplicationListRecord
    {
        public string ID { get; set; }
        public int? NextState { get; set; }

    }
}
