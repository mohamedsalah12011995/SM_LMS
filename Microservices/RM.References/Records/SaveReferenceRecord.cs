namespace RM.References.Records
{
    public record SaveReferenceRecord
    {
        public string ID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string parentId { get; set; }
        public string referencesMajorId { get; set; }
        public bool? HasContent { get; set; }
        public bool? IsPortal { get; set; }
        public string Url { get; set; }
    }
}
