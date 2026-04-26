
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("Status", Schema = "Contact")]
    public class Status
    {
        public Status()
        {
            ActionsStatus = new HashSet<Actions>();
        }
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        [ForeignKey("MajorStatus")]
        public int? MajorStatusId { get; set; }
        public MajorStatus MajorStatus { get; set; }

        public virtual ICollection<Actions> ActionsStatus { get; set; }
    }
}
