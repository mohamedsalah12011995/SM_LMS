
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{

    [Table("QuestionAnswers", Schema = "Exams")]
    public class ExamQuestionAnswer
    {

        public int Id { get; set; }
        public string Text { get; set; }
        public int? Value { get; set; }
        [ForeignKey("Question")]
        public int? QuestionId { get; set; }
        [ForeignKey("DataSource")]
        public int? DataSourceId { get; set; }
        [ForeignKey("SurveyAnswerAction")]
        public int? ExamAnswerActionId { get; set; }


        public virtual ExamQuestion Question { get; set; }
        public virtual ExamDataSource DataSource { get; set; }
        public virtual ExamAnswerAction ExamAnswerAction { get; set; }
    }

}
