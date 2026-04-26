namespace RM.Surveys.Records
{
    public record QuestionsDeleteListRecord
    {
        public string GroupId { get; set; }
        public bool? IsDeleted { get; set; }

    }
}
