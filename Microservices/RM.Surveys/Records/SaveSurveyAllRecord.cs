using RM.Core.Helpers;

namespace RM.Surveys.Records
{
    public record SaveSurveyAllRecord
    {
        public string ID { get; set; }
        public string referenceID { get; set; }
        public string entityID { get; set; }
        public string themeId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool? ShowInHomePage { get; set; }
        public bool? UseCapcha { get; set; }
        public bool? InnerOnly { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ImageBase64 { get; set; }
        public List<GroupQuestionRecord> GroupQuestion { get; set; }
        public List<Dtos.Reference> PublishEntity { get; set; }
        public List<SurveySettingsRecord> CronSettings { get; set; } = new List<SurveySettingsRecord>();

    }

    public record GroupQuestionRecord
    {
        public string GroupId { get; set; }
        public string TypeId { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }

        public List<SurveyQuestionRecord> SurveyQuestion { get; set; }
        public List<SurveyDataSourceRecord> GroupDataSource { get; set; }
    }

    public record SurveyQuestionRecord
    {
        public string ID { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool? Mandatory { get; set; }
        public bool? VerticalAnswersDirection { get; set; }
        public bool? IsFiltration { get; set; }
        public bool? IsGlobal { get; set; }
        public int? SubQuestionOrder { get; set; }
        public int? GroupOrder { get; set; }
        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public bool? IsNoteRequired { get; set; }
        public string ImageBase64 { get; set; }
        public bool? IsActive { get; set; }
        public QuestionsRecommendationsRecord QuestionsRecommendations { get; set; }
    }

    public record SurveyDataSourceRecord
    {
        public string ID { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string ImageBase64 { get; set; }
        public int Rate { get; set; }

    }

    public record QuestionsRecommendationsRecord
    {
        public string ID { get; set; }
        public string lessAverageId { get; set; }
        public string averageId { get; set; }
        public string aboveAverageId { get; set; }
        public string questionId { get; set; }

    }

    public record SurveySettingsRecord
    {
        public string ID { get; set; }
        public int CronTypeId { get; set; }
        public string surveyId { get; set; }

        public List<string> Emails { get; set; }
        public bool? IsActive { get; set; }
    }
}
