namespace RM.FileSharing.Records
{
    public record DeleteFileOrDirectoryRecord
    {
        public string Path { get; set; }
        public string Type { get; set; }
    }
}
