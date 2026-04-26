
using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{

    [Table("Questions", Schema = "Exams")]
    public class ExamQuestion
    {
        public ExamQuestion()
        {

            ExamQuestionAnswers = new List<ExamQuestionAnswer>();
            ExamDataSources = new List<ExamDataSource>();
        }

        public int Id { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        [ForeignKey("QuestionType")]
        public int? TypeId { get; set; }

        [ForeignKey("Exam")]
        public int? ExamId { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        [ForeignKey("DeletedByNavigation")]
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public bool? Mandatory { get; set; }
        public bool? VerticalAnswersDirection { get; set; }
        public double? Mark { get; set; }
        public int? Order { get; set; }

        public virtual Exam Exam { get; set; }
        public virtual ExamQuestionType QuestionType { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }

        public virtual User DeletedByNavigation { get; set; }
        public virtual List<ExamQuestionAnswer> ExamQuestionAnswers { get; set; }
        public virtual List<ExamDataSource> ExamDataSources { get; set; }

    }

}
