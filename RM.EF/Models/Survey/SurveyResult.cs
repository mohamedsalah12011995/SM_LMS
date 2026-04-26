
namespace RM.Models
{
    public class SurveyResult
    {
        public int? SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public string SurveyTitleEn { get; set; }
        public int? QuestionId { get; set; }
        public int? QuestionTypeId { get; set; }

        public string Question { get; set; }
        public string QuestionEn { get; set; }
        public int? Count { get; set; }
        public int? Rate { get; set; }
        public int? DataSourceId { get; set; }
        public string Choice { get; set; }
        public string ChoiceEn { get; set; }

        public string AnswerText { get; set; }
        public int? AnswerValue { get; set; }

        public string GroupId { get; set; }
    }
}
