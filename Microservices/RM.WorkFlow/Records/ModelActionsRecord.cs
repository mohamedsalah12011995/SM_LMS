namespace RM.WorkFlow.Records
{
    public record ModelActionsRecord
    {
        public string Id { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
