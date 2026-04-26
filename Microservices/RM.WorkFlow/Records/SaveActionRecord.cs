namespace RM.WorkFlow.Records
{
    public record SaveActionRecord
    {
        public string ActionId { get; set; }
        public string FormValueId { get; set; }
        public string EngineActionJobRoleId { get; set; }
        public string FromUserId { get; set; }
        public bool? IsReturned { get; set; }
        public bool? IsRejected { get; set; }
        public string Notes { get; set; }
        public string TransferToReferenceId { get; set; }
    }
}
