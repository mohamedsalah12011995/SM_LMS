namespace RM.Awards.Records
{
    public record DeleteRecord
    {
        public string ID { get; set; }
        public bool IsDeleted { get; set; }
    }
}
