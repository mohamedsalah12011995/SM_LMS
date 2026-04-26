namespace RM.FAQs.Records
{
    public record DeleteRecord
    {
        public string ID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
