

namespace RM.Exams.Records
{
    public record SaveAnswerActionRecord
    {
        public string examId { get; set; }
        public string itemId { get; set; }

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
