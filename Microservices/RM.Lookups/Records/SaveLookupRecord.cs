namespace RM.Lookups.Records
{
    public record SaveLookupRecord
    {
        public string ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool? IsActive { get; set; }
        public List<SaveMajorLookupsRecord> MajorLookups { get; set; }

    }

    public record SaveMajorLookupsRecord
    {
        public string ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string referenceId { get; set; }
        public List<SaveMajorLookupsRecord> SubLookups { get; set; }

    }
}
