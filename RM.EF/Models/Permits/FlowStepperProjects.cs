
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FlowStepperProjects", Schema = "Permits")]
    public class FlowStepperProjects
    {
        public int Id { get; set; }
        public int? OrderStep { get; set; }
        [ForeignKey("FlowStepProject")]
        public int? ProjectId { get; set; }
        [ForeignKey("FlowProjectStep")]
        public int? StepId { get; set; }
        public virtual Project FlowStepProject { get; set; }
        public virtual FlowStepper FlowProjectStep { get; set; }


    }
}
