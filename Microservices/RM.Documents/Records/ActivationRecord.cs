namespace RM.Documents.Records
{
    public record ActivationRecord
    {
        public string ID { get; set; }
        public bool IsActive { get; set; }
    }
}
