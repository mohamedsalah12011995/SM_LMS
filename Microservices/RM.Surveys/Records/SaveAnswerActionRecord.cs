namespace RM.Surveys.Records
{
    public record SaveAnswerActionRecord
    {
        public string SurveyId { get; set; }
        public List<SurveyQuestionAnswerRecord> SurveyQuestionAnswers { get; set; }
    }

    public record SurveyQuestionAnswerRecord
    {
        public string Text { get; set; }
        public int? Value { get; set; }

        public string QuestionType { get; set; }
        public string Notes { get; set; }
        public string DataSourceId { get; set; }
        public string QuestionId { get; set; }
    }
}
