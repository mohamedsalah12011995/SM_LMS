using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("Surveys", Schema = "Survey")]
    public partial class Survey
    {

        public int Id { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Code { get; set; }


        [ForeignKey("CreatedByNavigation")]
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }


        [ForeignKey("UpdatedByNavigation")]
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }


        [ForeignKey("DeletedByNavigation")]
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        [ForeignKey("Reference")]
        public int? ReferenceId { get; set; }
        [ForeignKey("Entity")]
        public int? EntityId { get; set; }
        public int? ThemeId { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? ShowInHomePage { get; set; }
        public bool? UseCapcha { get; set; }
        public bool? InnerOnly { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Image { get; set; }

        public virtual Entity Entity { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User UpdatedByNavigation { get; set; }
        public virtual SurveyTheme Theme { get; set; }

        public virtual List<SurveyQuestion> SurveyQuestions { get; set; } = new List<SurveyQuestion>();
        public virtual List<SurveyAnswerAction> SurveyAnswerActions { get; set; } = new List<SurveyAnswerAction>();
        public virtual List<CronSettings> CronSettings { get; set; } = new List<CronSettings>();

    }
}


