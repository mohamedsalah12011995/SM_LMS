#nullable disable

using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("SurveyAnswerAction", Schema = "Survey")]
    public partial class SurveyAnswerAction
    {

        public SurveyAnswerAction()
        {
            SurveyQuestionAnswers = new List<SurveyQuestionAnswer>();
        }

        public int Id { get; set; }

        [ForeignKey("Survey")]
        public int? SurveyId { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual List<SurveyQuestionAnswer> SurveyQuestionAnswers { get; set; }

    }
}
