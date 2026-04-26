using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("ExamExternalTranieesAnswerAction", Schema = "ExamTraining")]

    public class ExamExternalTranieesAnswerAction
    {
        public ExamExternalTranieesAnswerAction()
        {
            ExamExternalTranieesQuestionAnswer = new List<ExamExternalTranieesQuestionAnswer>();
        }
        public int Id { get; set; }

        [ForeignKey("Exam")]
        public int? ExamId { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ExternalCourseExamsId { get; set; }
        public string Note { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual List<ExamExternalTranieesQuestionAnswer> ExamExternalTranieesQuestionAnswer { get; set; }

    }


}
