
namespace RM.FileSharing.Records
{
    public record CopyFileOrDirectoryRecord
    {
        public DestinationRecord Destination { get; set; }
        public IList<SourcesRecord> Sources { get; set; }
    }

    public record DestinationRecord
    {
        public string Path { get; set; }
    }

    public record SourcesRecord
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
    }
}
