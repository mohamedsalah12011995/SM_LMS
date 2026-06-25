#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("Questions", Schema = "Survey")]
    public partial class SurveyQuestion
    {
        public SurveyQuestion()
        {

            SurveyQuestionAnswers = new List<SurveyQuestionAnswer>();
            SurveyDataSources = new List<SurveyDataSource>();
        }

        public int Id { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        [ForeignKey("QuestionType")]
        public int? TypeId { get; set; }

        [ForeignKey("Survey")]
        public int? SurveyId { get; set; }

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
        public bool? IsGlobal { get; set; }

        public bool? Mandatory { get; set; }
        public bool? VerticalAnswersDirection { get; set; }
        public string GroupId { get; set; }
        public int? SubQuestionOrder { get; set; }
        public int? GroupOrder { get; set; }

        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public bool? IsFiltration { get; set; }
        public string? Image { get; set; }
        public bool? IsNoteRequired { get; set; }

        public virtual Survey Survey { get; set; }
        public virtual SurveyQuestionType QuestionType { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }

        public virtual User DeletedByNavigation { get; set; }
        public virtual List<SurveyQuestionAnswer> SurveyQuestionAnswers { get; set; } = new List<SurveyQuestionAnswer>();
        public virtual List<SurveyDataSource> SurveyDataSources { get; set; } = new List<SurveyDataSource>();
        public virtual QuestionsRecommendations QuestionsRecommendations { get; set; } 
    }
}
