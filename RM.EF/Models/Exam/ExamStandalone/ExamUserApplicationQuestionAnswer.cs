#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("ExamUserApplicationQuestionAnswer", Schema = "ExamStandalone")]
    public class ExamUserApplicationQuestionAnswer
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? Value { get; set; }
        [ForeignKey("Question")]
        public int? QuestionId { get; set; }
        [ForeignKey("DataSource")]
        public int? DataSourceId { get; set; }
        [ForeignKey("ExamUserApplicationAnswerAction")]
        public int? ExamUserApplicationAnswerActionId { get; set; }


        public virtual ExamQuestion Question { get; set; }
        public virtual ExamDataSource DataSource { get; set; }
        public virtual ExamUserApplicationAnswerAction ExamUserApplicationAnswerAction { get; set; }
    }
}
