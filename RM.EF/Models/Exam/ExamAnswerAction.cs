
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{

    [Table("ExamAnswerAction", Schema = "Exams")]
    public class ExamAnswerAction
    {

        public ExamAnswerAction()
        {
            ExamQuestionAnswers = new List<ExamQuestionAnswer>();
        }

        public int Id { get; set; }

        [ForeignKey("Exam")]
        public int? ExamId { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ItemId { get; set; }
        public string Note { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual List<ExamQuestionAnswer> ExamQuestionAnswers { get; set; }

    }

}
