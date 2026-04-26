namespace RM.Surveys.Records
{
    public record SendEmailSurveysStatisticsRecord
    {
        public string SurveyId { get; set; }
        public List<string> DropDownIds { get; set; } = new List<string>();
        public List<string> Emails { get; set; } = new List<string>();
        public string Subject { get; set; }
        public string Body { get; set; }
        public string FileName { get; set; }

    }
}
