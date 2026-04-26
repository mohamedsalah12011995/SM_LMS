using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;
using System.Linq;

namespace RM.FileSharing.Dtos
{
    public class SharedFoldersPaths
    {
        public string SuperAdminsUsers { get; set; }   
        public List<string> SuperAdminsUsersList { get { return !string.IsNullOrEmpty(SuperAdminsUsers) ? SuperAdminsUsers.Split(',').ToList():new List<string>(); }}

        public string SuperAdminsGroups { get; set; }
        public List<string> SuperAdminsGroupsList { get { return !string.IsNullOrEmpty(SuperAdminsGroups) ? SuperAdminsGroups.Split(',').ToList():new List<string>(); } }

        public List<FoldersList> FoldersList { get; set; }
    }

    public class FoldersList
    {
        public string FolderPath { get; set; }
        public string AdminsUsers { get; set; }
        public List<string> AdminsUsersList { get { return  !string.IsNullOrEmpty(AdminsUsers) ? AdminsUsers.Split(',').ToList() : new List<string>() ; }}

        public string AdminsGroups { get; set; }
        public List<string> AdminsGroupsList { get { return  !string.IsNullOrEmpty(AdminsGroups) ? AdminsGroups.Split(',').ToList() : new List<string>() ; } }
    }
}
