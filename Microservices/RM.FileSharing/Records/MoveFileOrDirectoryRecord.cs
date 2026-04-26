namespace RM.FileSharing.Records
{
    public record MoveFileOrDirectoryRecord
    {
        public MoveDestinationRecord Destination { get; set; }
        public IList<MoveSourcesRecord> Sources { get; set; }
    }

    public record MoveDestinationRecord
    {
        public string Path { get; set; }
    }

    public record MoveSourcesRecord
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
    }

}
