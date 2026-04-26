namespace RM.ScientificLetters.Records
{
    public record ModelActionsRecord
    {
        public string ID { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
