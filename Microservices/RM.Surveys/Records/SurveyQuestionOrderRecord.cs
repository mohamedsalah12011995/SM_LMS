
namespace RM.Surveys.Records
{
    public record SurveyQuestionOrderRecord
    {
        public string ID { get; set; }
        public List<GroupQuestionOrderRecord> GroupQuestion { get; set; }

    }

    public record GroupQuestionOrderRecord
    {
        public List<SurveyQuestionsOrderRecord> SurveyQuestion { get; set; }
    }

    public record SurveyQuestionsOrderRecord
    {
        public string ID { get; set; }
        public int? SubQuestionOrder { get; set; }
        public int? GroupOrder { get; set; }
    }
}
