namespace RM.FileSharing.Records
{
    public record GetDirFileAccessRulesRecord
    {
        public string Path { get; set; }
        public string Type { get; set; }
    }
}
