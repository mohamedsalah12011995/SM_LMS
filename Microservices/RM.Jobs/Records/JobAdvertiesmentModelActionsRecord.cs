namespace RM.Jobs.Records
{
    public record JobAdvertiesmentModelActionsRecord
    {
        public string ID { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
