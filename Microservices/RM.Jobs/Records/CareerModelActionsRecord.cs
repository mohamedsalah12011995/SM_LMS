namespace RM.Jobs.Records
{
    public record CareerModelActionsRecord
    {
        public string ID { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
