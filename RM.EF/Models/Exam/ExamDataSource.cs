
#nullable disable

using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{

    [Table("DataSource", Schema = "Exams")]

    public class ExamDataSource
    {
        public ExamDataSource()
        {
            ExamQuestionAnswers = new List<ExamQuestionAnswer>();
        }
        public int Id { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }


        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }


        [ForeignKey("DeletedByNavigation")]
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        [ForeignKey("SurveyQuestion")]
        public int? QuestionId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsCorrect { get; set; }

        public virtual ExamQuestion ExamQuestion { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }

        public virtual List<ExamQuestionAnswer> ExamQuestionAnswers { get; set; }

    }

}
