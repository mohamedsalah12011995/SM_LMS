namespace RM.Exams.Records
{
    public record GetQuestionDetailsRecord
    {
        public string ID { get; set; }
        public bool? IsActive { get; set; }

    }
}
