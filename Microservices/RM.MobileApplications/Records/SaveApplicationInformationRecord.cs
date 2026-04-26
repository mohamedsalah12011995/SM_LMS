namespace RM.MobileApplications.Records
{
    public record SaveApplicationInformationRecord
    {
        public string ID { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string ApplicationSize { get; set; }
        public string AndroidUrl { get; set; }
        public string IosUrl { get; set; }
        public string ImageUrlBase64 { get; set; }
        public string UserManualUrlArBase64 { get; set; }
        public string UserManualUrlEnBase64 { get; set; }
        public List<QuestionsAnswersRecord> ListOfQuestions { get; set; }
        public List<AttachmentsRecord> ListOfImage { get; set; }

    }

    public record QuestionsAnswersRecord
    {
        public string ID { get; set; }
        public string QuestionAr { get; set; }
        public string QuestionEn { get; set; }
        public string AnswerAr { get; set; }
        public string AsnwerEn { get; set; }
    }

    public record AttachmentsRecord
    {
        public string ID { get; set; }
        public string NameAr { get; set; }
        public string Url { get; set; }
    }
}
