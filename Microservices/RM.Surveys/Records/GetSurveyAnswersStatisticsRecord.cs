namespace RM.Surveys.Records
{
    public record GetSurveyAnswersStatisticsRecord
    {
        public string SurveyId { get; set; }
        public List<string> DropDownIds { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
