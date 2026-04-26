namespace RM.Jobs.Records
{
    public record SaveJobAppExamAnswersRecord
    {
        public string appExamId { get; set; }
        public string Note { get; set; }

        public List<ExamQuestionAnswerRecord> ExamQuestionAnswers { get; set; } = new List<ExamQuestionAnswerRecord>();
    }

    public record ExamQuestionAnswerRecord
    {
        public string questionId { get; set; }
        public string dataSourceId { get; set; }
        public string Text { get; set; }
        public int? Value { get; set; }
    }
}
