#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("InternalCourseTrainees", Schema = "ExamTraining")]

    public class InternalCourseTrainees
    {
        public int Id { get; set; }
        public string Code { get; set; }

        [ForeignKey("Course")]

        public int? CourseId { get; set; }

        [ForeignKey("TrainingCourseSchedule")]

        public int? TrainingCourseScheduleId { get; set; }

        [ForeignKey("Trainee")]

        public int? TraineeId { get; set; }

        public string IdCardNumber { get; set; }
        public string TraineeName { get; set; }
        public string EmployeeID { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
        public bool? IsAttendedExam { get; set; }
        public bool? IsSentCertification { get; set; }
        public string CertificationNumber { get; set; }


        [ForeignKey("CreatedByNavigation")]

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("UpdatedByNavigation")]

        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual User Trainee { get; set; }

        public virtual TrainingCourse Course { get; set; }

        public virtual TrainingCourseSchedule TrainingCourseSchedule { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }

    }
}
