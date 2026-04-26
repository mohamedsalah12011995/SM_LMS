using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("ExamUserApplicationAnswerAction", Schema = "ExamStandalone")]
    public class ExamUserApplicationAnswerAction
    {
        public ExamUserApplicationAnswerAction()
        {
            ExamQuestionAnswer = new List<ExamUserApplicationQuestionAnswer>();
        }

        public int Id { get; set; }

        [ForeignKey("Exam")]
        public int? ExamId { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [ForeignKey("UserApplicationExam")]

        public int? UserApplicationExamId { get; set; }
        public string Note { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual UserApplicationExam UserApplicationExam { get; set; }
        public virtual List<ExamUserApplicationQuestionAnswer> ExamQuestionAnswer { get; set; }
    }
}
