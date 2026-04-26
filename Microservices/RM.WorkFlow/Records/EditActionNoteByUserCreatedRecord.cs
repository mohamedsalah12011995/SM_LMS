namespace RM.WorkFlow.Records
{
    public record EditActionNoteByUserCreatedRecord
    {
        public string FormValueId { get; set; }
        public string EngineActionJobRoleId { get; set; }
        public string Notes { get; set; }
    }
}
