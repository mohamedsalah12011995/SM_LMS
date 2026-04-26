using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("ExamExternalTranieesQuestionAnswer", Schema = "ExamTraining")]
    public class ExamExternalTranieesQuestionAnswer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? Value { get; set; }
        [ForeignKey("Question")]
        public int? QuestionId { get; set; }
        [ForeignKey("DataSource")]
        public int? DataSourceId { get; set; }
        [ForeignKey("ExamExternalTranieesAnswerAction")]
        public int? ExamExternalTranieesAnswerActionId { get; set; }


        public virtual ExamQuestion Question { get; set; }
        public virtual ExamDataSource DataSource { get; set; }
        public virtual ExamExternalTranieesAnswerAction ExamExternalTranieesAnswerAction { get; set; }
    }
}
