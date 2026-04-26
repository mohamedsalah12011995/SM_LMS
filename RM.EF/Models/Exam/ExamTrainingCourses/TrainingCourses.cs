using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("TrainingCourses", Schema = "ExamTraining")]
    public class TrainingCourse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public int Type { get; set; }
        public bool LoginRequired { get; set; }
        public bool? HasCertificate { get; set; }

        [ForeignKey("CreatedByNavigation")]

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("UpdatedByNavigation")]

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }


        [ForeignKey("DeletedByNavigation")]

        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        [ForeignKey("Reference")]
        public int? ReferenceId { get; set; }

        [ForeignKey("Entity")]
        public int? EntityId { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }

    }
}
