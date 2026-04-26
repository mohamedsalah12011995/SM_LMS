namespace RM.FileSharing.Dtos
{
    public class Role
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int ApplyTo { get; set; }

    }

    public enum RoleType
    {
        FolderOnly=1,
        FolderAndSubFolder = 2,
        FolderAndSubFolderAndFiles = 3
    }
}
