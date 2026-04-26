using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("ExamInternalTranieesAnswerAction", Schema = "ExamTraining")]

    public class ExamInternalTranieesAnswerAction
    {

        public ExamInternalTranieesAnswerAction()
        {
            ExamInternalTranieesQuestionAnswer = new List<ExamInternalTranieesQuestionAnswer>();
        }
        public int Id { get; set; }

        [ForeignKey("Exam")]
        public int? ExamId { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? InternalCourseExamsId { get; set; }
        public string Note { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual List<ExamInternalTranieesQuestionAnswer> ExamInternalTranieesQuestionAnswer { get; set; }

    }


}
