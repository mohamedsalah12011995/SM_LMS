using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("ExternalCourseExams", Schema = "ExamTraining")]
    public class ExternalCourseExams
    {
        public int Id { get; set; }

        [ForeignKey("TrainingCourseSchedule")]
        public int? TrainingCourseScheduleId { get; set; }

        [ForeignKey("Exam")]
        public int? ExamId { get; set; }

        [ForeignKey("ExternalCourseTraniee")]
        public int? ExternalCourseTranieeId { get; set; }


        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }


        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }



        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public double? AnswerTotalTime { get; set; }
        public double Result { get; set; }
        public bool? IsSuccess { get; set; }
        public string CertificateNumber { get; set; }

        public virtual TrainingCourseSchedule TrainingCourseSchedule { get; set; }
        public virtual ExternalCourseTraniees ExternalCourseTraniee { get; set; }
        public virtual Exam Exam { get; set; }
        public User CreatedByNavigation { get; set; }
        public User UpdatedByNavigation { get; set; }

    }
}
