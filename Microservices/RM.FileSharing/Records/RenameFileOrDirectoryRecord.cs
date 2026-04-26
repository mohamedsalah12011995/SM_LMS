namespace RM.FileSharing.Records
{
    public record RenameFileOrDirectoryRecord
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
    }
}
