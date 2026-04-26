namespace RM.Surveys.Records
{
    public record QuestionModelActionsRecord
    {
        public string ID { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
    }
}
