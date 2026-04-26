namespace RM.FAQs.Records
{
    public record SaveFAQRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string QuestionAr { get; set; }
        public string QuestionEn { get; set; }
        public string AnswerAr { get; set; }
        public string AnswerEn { get; set; }
    }
}
