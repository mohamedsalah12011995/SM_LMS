namespace RM.Exams.Records
{
    public record GetExamDetailsRecord
    {
        public string ID { get; set; }
        public bool? IsActive { get; set; }
    }
}
