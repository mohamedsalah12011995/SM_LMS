#nullable disable

using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("PermitActions", Schema = "Permits")]
    public class PermitAction
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }
        public bool? IsPrinted { get; set; }

        [ForeignKey("PermitsRequest")]
        public int? PermitRequestId { get; set; }
        [ForeignKey("StepNavigation")]
        public int? StepId { get; set; }
        public int? Status { get; set; }
        public string Notes { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual PermitsRequest PermitsRequest { get; set; }
        public virtual FlowStepper StepNavigation { get; set; }
    }
}
