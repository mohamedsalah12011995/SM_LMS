
#nullable disable

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FlowStepper", Schema = "Permits")]
    public class FlowStepper
    {
        public FlowStepper()
        {

            CurrentStepNavigations = new HashSet<PermitsRequest>();
            NextStepNavigations = new HashSet<PermitsRequest>();
            StepNavigations = new HashSet<PermitAction>();
        }
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public virtual ICollection<PermitsRequest> CurrentStepNavigations { get; set; }
        public virtual ICollection<PermitsRequest> NextStepNavigations { get; set; }
        public virtual ICollection<PermitAction> StepNavigations { get; set; }
       

    }
}
