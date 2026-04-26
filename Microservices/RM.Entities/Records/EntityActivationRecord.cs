namespace RM.Entities.Records
{
    public record EntityActivationRecord
    {
        public string ID { get; set; }
        public bool IsActive { get; set; }
    }
}
