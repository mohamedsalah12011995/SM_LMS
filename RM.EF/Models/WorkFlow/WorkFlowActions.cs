using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("WorkFlowActions", Schema = "WorkFlow")]
    public class WorkFlowActions
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool? IsActive { get; set; }
        public DateTime? ActivatedDate { get; set; }
        [ForeignKey("Reference")]
        public int? ReferenceId { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual Reference Reference { get; set; }

        public virtual ICollection<EngineActionJobRole> NextSteps { get; set; } = new List<EngineActionJobRole>();
        public virtual ICollection<EngineActionJobRole> ReturnSteps { get; set; } = new List<EngineActionJobRole>();
        public virtual ICollection<EngineActionJobRole> RejectSteps { get; set; } = new List<EngineActionJobRole>();
        public virtual ICollection<EngineActionJobRole> CloseSteps { get; set; } = new List<EngineActionJobRole>();
        public virtual ICollection<FormInputsActions> FormInputsActions { get; set; } = new List<FormInputsActions>();
        public virtual ICollection<FormValuesActions> FormValuesActions { get; set; } = new List<FormValuesActions>();
        public virtual ICollection<EngineActionJobRole> Actions { get; set; } = new List<EngineActionJobRole>();

    }
}
