#nullable disable

using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("QuestionAnswers", Schema = "Survey")]
    public partial class SurveyQuestionAnswer
    {

        public int Id { get; set; }
        public string Text { get; set; }
        public int? Value { get; set; }
        [ForeignKey("Question")]
        public int? QuestionId { get; set; }
        [ForeignKey("DataSource")]
        public int? DataSourceId { get; set; }
        [ForeignKey("SurveyAnswerAction")]
        public int? SurveyAnswerActionId { get; set; }
        public string Notes { get; set; }

        public virtual SurveyQuestion Question { get; set; }
        public virtual SurveyDataSource DataSource { get; set; }
        public virtual SurveyAnswerAction SurveyAnswerAction { get; set; }
    }
}
