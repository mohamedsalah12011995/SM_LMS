namespace RM.FileSharing.Records
{
    public record GetGroupMembersRecord
    {
        public Guid? Guid { get; set; }
        public string Path { get; set; }
    }
}
