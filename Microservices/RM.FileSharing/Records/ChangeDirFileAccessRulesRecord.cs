using RM.FileSharing.Dtos;

namespace RM.FileSharing.Records
{
    public record ChangeDirFileAccessRulesRecord
    {
        public string Path { get; set; }
        public string Type { get; set; }
        public bool? AddRootPermission { get; set; }

        public List<DirFileAccessRule> DirFileAccessRules { get; set; }

    }
}
