namespace RM.FileSharing.Records
{
    public record UserOrGroupSearchRecord
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
