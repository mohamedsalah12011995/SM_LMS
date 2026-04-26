using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("ExamInternalTranieesQuestionAnswer", Schema = "ExamTraining")]

    public class ExamInternalTranieesQuestionAnswer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? Value { get; set; }
        [ForeignKey("Question")]
        public int? QuestionId { get; set; }
        [ForeignKey("DataSource")]
        public int? DataSourceId { get; set; }
        [ForeignKey("ExamInternalTranieesAnswerAction")]
        public int? ExamInternalTranieesAnswerActionId { get; set; }


        public virtual ExamQuestion Question { get; set; }
        public virtual ExamDataSource DataSource { get; set; }
        public virtual ExamInternalTranieesAnswerAction ExamInternalTranieesAnswerAction { get; set; }
    }
}
