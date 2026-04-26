
namespace RM.WorkFlow.Records
{
    public record SaveEngineRecord
    {
        public string ID {  get; set; }
        public string referenceId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public List<EnginesActionsJobRoleRecord> EnginesActionsJobRoles { get; set; } = new List<EnginesActionsJobRoleRecord>();

    }

    public record EnginesActionsJobRoleRecord
    {
        public string ID { get; set; }
        public int? StepNo { get; set; }
        public string nextStep { get; set; }
        public string closeStep { get; set; }
        public string returnStep { get; set; }
        public string rejectStep { get; set; }
        public bool? HasNote { get; set; }
        public bool? NoteIsRequired { get; set; }
        public bool? IsTransferToReference { get; set; }
        public string jobRoleId { get; set; }
        public string actionId { get; set; }

    }
}
