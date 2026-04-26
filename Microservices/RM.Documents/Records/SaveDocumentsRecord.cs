namespace RM.Documents.Records
{
    public record SaveDocumentsRecord
    {
        public string ID { get; set; }
        public string typeId { get; set; }
        public string referenceId { get; set; }
        public string entityId { get; set; }
        public string parentId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string UrlBase64 { get; set; }

    }
}
