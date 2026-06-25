#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("FormValuesActions", Schema = "WorkFlow")]

    public class FormValuesActions
    {
        public int Id { get; set; }
        public int? FormValueId { get; set; }
        public int? EngineActionJobRoleId { get; set; }
        public int? ActionId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? FromUserId { get; set; }
        public int? ToReferenceId { get; set; }
        public string Notes { get; set; }
        public bool? IsReturned { get; set; }
        public bool? IsRejected { get; set; }
        public int? TransferToReferenceId { get; set; }
        public virtual FormValue FormValue { get; set; }
        public virtual EngineActionJobRole EngineActionJobRole { get; set; }
        public virtual WorkFlowActions Action { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User FromUser { get; set; }
        public virtual Reference ToReference { get; set; }
    }
}
