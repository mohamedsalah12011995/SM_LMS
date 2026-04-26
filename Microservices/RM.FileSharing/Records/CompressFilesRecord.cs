namespace RM.FileSharing.Records
{
    public record CompressFilesRecord
    {
        public CompressDestinationRecord Destination { get; set; }
        public IList<CompressSourcesRecord> Sources { get; set; }
    }

    public record CompressDestinationRecord
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public record CompressSourcesRecord
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
    }
}
