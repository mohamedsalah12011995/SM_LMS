namespace RM.Surveys.Records
{
    public record ModelActionsRecord
    {
        public string ID { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
    }
}
