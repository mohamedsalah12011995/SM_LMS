namespace RM.Documents.Records
{
    public record SaveEditorDocsRecord
    {
        public string FileName { get; set; }
        public string UrlBase64 { get; set; }

    }
}
