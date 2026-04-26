namespace RM.Surveys.Records
{
    public record GetAllQuestionsListRecord
    {
        public string TypeQuestionId { get; set; }
        public string referenceID { get; set; }
    }
}
