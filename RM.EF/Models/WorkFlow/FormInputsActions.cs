using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FormInputsActions", Schema = "WorkFlow")]

    public class FormInputsActions
    {
        public int Id { get; set; }
        public int? FormId { get; set; }
        public int? EngineFormId { get; set; }
        public int? FormInputId { get; set; }
        public int? ActionId { get; set; }

        [ForeignKey("EngineActionJobRoleNavigation")]
        public int? EngineActionJobRoleId { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool? IsActive { get; set; }
        public DateTime? ActivatedDate { get; set; }

        public virtual EngineForms EngineForm { get; set; }
        public virtual FormInput FormInput { get; set; }
        public virtual WorkFlowActions Action { get; set; }
        public virtual EngineActionJobRole EngineActionJobRoleNavigation { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }

    }
}
