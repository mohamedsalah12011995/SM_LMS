#nullable disable

using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("Projects", Schema = "Permits")]
    public class Project
    {

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DetailsAr { get; set; }
        public string DetailsEn { get; set; }
        public int? EntityId { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }

        public DateTime? DeletedDate { get; set; }

        [ForeignKey("DeletedByNavigation")]
        public int? DeletedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }

        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }
        public virtual Entity Entity { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }



    }
}
