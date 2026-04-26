using System.ComponentModel.DataAnnotations.Schema;



namespace RM.Models
{
    [Table("EngineActionJobRole", Schema = "WorkFlow")]
    public class EngineActionJobRole
    {
        public int Id { get; set; }
        public int? EngineId { get; set; }
        public int? ActionId { get; set; }
        public int? NextStep { get; set; }
        public int? ReturnStep { get; set; }
        public int? RejectStep { get; set; }
        public int? CloseStep { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? JobRoleId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? StepNo { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? HasNote { get; set; }
        public bool? NoteIsRequired { get; set; }
        public bool? IsTransferToReference { get; set; }
        public bool? IsSendEmail { get; set; }
        public string EmailBody { get; set; }

        public virtual Engine EngineNavigation { get; set; }
        public virtual WorkFlowActions ActionNavigation { get; set; }
        public virtual WorkFlowActions NextStepNavigation { get; set; }
        public virtual WorkFlowActions ReturnStepNavigation { get; set; }
        public virtual WorkFlowActions RejectStepNavigation { get; set; }
        public virtual WorkFlowActions CloseStepNavigation { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }

        public virtual JobRole JobRole { get; set; }
        public virtual ICollection<FormValuesActions> FormValuesActions { get; set; } = new List<FormValuesActions>();
        public virtual ICollection<FormInputsActions> FormInputsEnginActionJobRoles { get; set; } = new List<FormInputsActions>();



    }
}
