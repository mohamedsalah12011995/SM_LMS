namespace RM.Surveys.Records
{
    public record QuestionsActivationListRecord
    {
        public string GroupId { get; set; }
        public bool? IsActive { get; set; }

    }
}
