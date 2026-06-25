#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("ExternalCourseTraniees", Schema = "ExamTraining")]

    public class ExternalCourseTraniees
    {
        public int Id { get; set; }
        public string Code { get; set; }

        [ForeignKey("TrainingCourseSchedule")]

        public int? TrainingCourseScheduleId { get; set; }

        [ForeignKey("Course")]

        public int? CourseId { get; set; }
        public string FullName { get; set; }
        public string IdCardNumber { get; set; }
        public DateTime? BirthDate { get; set; }

        public int? Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        [ForeignKey("Grade")]

        public int? GradeType { get; set; }
        public int? GradeYear { get; set; }
        public string GradeTitle { get; set; }
        public int Status { get; set; }
        public bool? IsAttendedExam { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsSentCertification { get; set; }
        public string CertificationNumber { get; set; }

        [ForeignKey("CreatedByNavigation")]

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("UpdatedByNavigation")]

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual TrainingCourseSchedule TrainingCourseSchedule { get; set; }
        public virtual TrainingCourse Course { get; set; }
        public virtual MajorLookup Grade { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }


    }
}
