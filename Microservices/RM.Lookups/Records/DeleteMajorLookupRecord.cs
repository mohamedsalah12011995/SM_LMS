namespace RM.Lookups.Records
{
    public record DeleteMajorLookupRecord
    {
        public string ID { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
