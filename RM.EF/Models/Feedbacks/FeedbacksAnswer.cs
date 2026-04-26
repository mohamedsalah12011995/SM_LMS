using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FeedbacksAnswers", Schema = "Feedback")]
    public class FeedbacksAnswer
    {

        public int Id { get; set; }
        public string Text { get; set; }

        [ForeignKey("FeedbacksDataSource")]
        public int? FeedbacksDataSourceId { get; set; }
        [ForeignKey("FeedbacksAnswerAction")]
        public int? FeedbacksAnswerActionId { get; set; }

        public virtual FeedbacksDataSource FeedbacksDataSource { get; set; }
        public virtual FeedbacksAnswerAction FeedbacksAnswerAction { get; set; }
    }
}
