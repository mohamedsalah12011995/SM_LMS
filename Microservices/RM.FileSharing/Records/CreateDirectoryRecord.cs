namespace RM.FileSharing.Records
{
    public record CreateDirectoryRecord
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
