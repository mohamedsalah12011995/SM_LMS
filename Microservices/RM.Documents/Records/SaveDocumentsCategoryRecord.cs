namespace RM.Documents.Records
{
    public record SaveDocumentsCategoryRecord
    {
        public string ID { get; set; }
        public string typeId { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
