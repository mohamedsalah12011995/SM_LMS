#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("Certificates", Schema = "Exams")]
    public class Certificate
    {
        public int Id { get; set; }

        [ForeignKey("Reference")]

        public int? ReferenceId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Signature { get; set; }
        public string LogoUrl { get; set; }
        [ForeignKey("CreatedByNavigation")]

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("UpdatedByNavigation")]

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
    }
}
