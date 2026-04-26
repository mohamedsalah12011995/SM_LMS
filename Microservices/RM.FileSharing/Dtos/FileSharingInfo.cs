using RM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace RM.FileSharing.Dtos
{
    public class FileSharingInfo
    {
        public Guid? Guid { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public string UrlBase64 { get; set; }
        public string Type { get; set; }
        public string Owner { get; set; }
        public string Size { get; set; }
        public long? SizeKb { get; set; }

        public string ProccessId { get; set; }
        public bool StopProccess { get; set; }

        public string ModifiedDate { get; set; }

        public DateTime? FromModifiedDate { get; set; }
        public DateTime? ToModifiedDate { get; set; }
        public bool? ModifiedToday { get; set; }
        public bool? ModifiedWeek { get; set; }
        public bool? ModifiedMonth { get; set; }
        public string SearchWord { get; set; }
        public bool? ParentFolder { get; set; }

        public bool? AddRootPermission { get; set; }
        public DirFilePermission Permission { get; set; }
        public List<DirFileAccessRule> DirFileAccessRules { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

    }

    public class DirFilePermission
    {
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Modify { get; set; }
        public bool ReadAndExecute { get; set; }
        public bool FullControl { get; set; }
        public bool IsInherit { get; set; }
        public bool IsOwner { get; set; }
        public bool IsAdmin { get; set; }

        public int Type { get; set; }

    }

    public class ADUser
    {
        public Guid? Guid { get; set; }
        public string Name { get; set; }
        public string UserPrincipalName { get; set; }
        public string? Sid { get; set; }
        public string SamAccountName { get; set; }
        public bool IsLockout { get; set; }
        public List<ADGroup> Groups { get; set; } = new List<ADGroup>();
    }

    public class ADGroup
    {
        public Guid? Guid { get; set; }
        public string Name { get; set; }
        public string? Sid { get; set; }
        public string SamAccountName { get; set; }
        public List<ADUser> Members { get; set; } = new List<ADUser>();
    }

    public class DirFileAccessRule
    {
        public string Name { get; set; }
        public string UserPrincipalName { get; set; }
        public string Type { get; set; }
        public string? Sid { get; set; }

        public DirFilePermission Permission { get; set; }
        public int? ApplyTo { get; set; }

    }

    public class FilesSharingInfo
    {
        public FileSharingInfo Destination { get; set; }
        public IList<FileSharingInfo> Sources { get; set; }

    }

}
