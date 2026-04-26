namespace RM.Exams.Records
{
    public record QuestionModelActionsRecord
    {
        public string ID { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
