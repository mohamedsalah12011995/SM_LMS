using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FeedbacksAnswerAction", Schema = "Feedback")]
    public class FeedbacksAnswerAction
    {

        public FeedbacksAnswerAction()
        {
            FeedbacksAnswers = new List<FeedbacksAnswer>();
        }

        public int Id { get; set; }

        [ForeignKey("Feedbacks")]
        public int? FeedbacksId { get; set; }
        [ForeignKey("Entity")]
        public int? EntityId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? ItemId { get; set; }
        public bool? IsHelpful { get; set; }
        public string ItemUrl { get; set; }
        public virtual Entity Entity { get; set; }

        public virtual Feedbacks Feedbacks { get; set; }
        public virtual List<FeedbacksAnswer> FeedbacksAnswers { get; set; }

    }
}
