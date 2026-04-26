
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("Engine", Schema = "WorkFlow")]
    public class Engine
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("Reference")]
        public int? ReferenceId { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool? IsActive { get; set; }
        public DateTime? ActivatedDate { get; set; }

        public virtual Reference Reference { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }

        public virtual ICollection<EngineActionJobRole> EnginesActionsJobRoles { get; set; } = new List<EngineActionJobRole>();


    }
}
