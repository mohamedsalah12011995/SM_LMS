#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("TrainingCourseSchedule", Schema = "ExamTraining")]
    public class TrainingCourseSchedule
    {
        public int Id { get; set; }
        [ForeignKey("Reference")]
        public int? ReferenceId { get; set; }
        [ForeignKey("DepartmentReference")]
        public int? DepartmentReferenceId { get; set; }
        [ForeignKey("Course")]
        public int? CourseId { get; set; }

        [ForeignKey("Exam")]
        public int? ExamId { get; set; }

        [ForeignKey("Certificate")]
        public int? CertificateId { get; set; }
        [ForeignKey("CertificateTheme")]
        public int? CertificateThemeId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsClosed { get; set; }

        [ForeignKey("CreatedByNavigation")]

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("UpdatedByNavigation")]

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }


        [ForeignKey("DeletedByNavigation")]

        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual Reference DepartmentReference { get; set; }
        public virtual TrainingCourse Course { get; set; }
        public virtual Exam Exam { get; set; }
        public virtual Certificate Certificate { get; set; }
        public virtual CertificateThemes CertificateTheme { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
    }
}
