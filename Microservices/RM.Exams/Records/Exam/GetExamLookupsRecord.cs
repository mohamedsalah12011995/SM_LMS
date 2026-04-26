namespace RM.Exams.Records
{
    public record GetExamLookupsRecord
    {
        public string referenceId { get; set; }
        public string entityId { get; set; }
    }
}
