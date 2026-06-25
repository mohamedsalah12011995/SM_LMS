
#nullable disable

using System.ComponentModel.DataAnnotations.Schema;



namespace RM.Models
{
    [Table("JobApplicationExams", Schema = "Job")]

    public partial class JobApplicationExams
    {
        public int Id { get; set; }

        [ForeignKey("JobApplication")]

        public int JobApplicationId { get; set; }

        [ForeignKey("Exam")]

        public int ExamId { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }


        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("DeletedByNavigation")]
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public double Result { get; set; }
        public bool? IsSuccess { get; set; }

        public JobApplication JobApplication { get; set; }
        public Exam Exam { get; set; }
        public User CreatedByNavigation { get; set; }
        public User UpdatedByNavigation { get; set; }
        public User DeletedByNavigation { get; set; }


    }
}
