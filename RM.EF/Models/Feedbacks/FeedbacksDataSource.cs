using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FeedbacksDataSource", Schema = "Feedback")]

    public class FeedbacksDataSource
    {
        public FeedbacksDataSource()
        {
            FeedbacksAnswers = new List<FeedbacksAnswer>();
        }
        public int Id { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }

        public bool? HasNote { get; set; }

        public bool? IsHelpful { get; set; }

        [ForeignKey("Feedbacks")]
        public int? FeedbacksId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }


        [ForeignKey("LessAverage")]
        public int? LessAverageId { get; set; }
        public Recommendations LessAverage { get; set; }


        [ForeignKey("Average")]
        public int? AverageId { get; set; }
        public Recommendations Average { get; set; }


        [ForeignKey("AboveAverage")]
        public int? AboveAverageId { get; set; }
        public Recommendations AboveAverage { get; set; }

        public virtual Feedbacks Feedbacks { get; set; }

        public virtual List<FeedbacksAnswer> FeedbacksAnswers { get; set; }

    }
}
