using DocumentFormat.OpenXml.InkML;
using RM.FileSharing.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace RM.FileSharing
{
    public class Helper
    {
        public static readonly Dictionary<string, (int,int)> PermissionThreads = new Dictionary<string, (int, int)>();
        public static readonly Dictionary<string, ADUser> ADUsers = new Dictionary<string, ADUser>();
        public static readonly Dictionary<string, string> NTAccounts = new Dictionary<string, string>();
        public static SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

        public static readonly Dictionary<string,(bool, List<FileSharingInfo>)> SearchResult = new Dictionary<string, (bool, List<FileSharingInfo>)>();

        Helper()
        {

        }

        public static void Start(string proccessId)
        {
            int c = 0; int f = 0;
            (c, f) = PermissionThreads[proccessId];
            c++;
            PermissionThreads[proccessId] = (c, f);
        }

        public static void Finish(string proccessId)
        {
            lock (PermissionThreads)
            {
                int c = 0; int f = 0;
                (c, f) = PermissionThreads[proccessId];
                f++;
                PermissionThreads[proccessId] = (c, f);
            }
        }

        public static List<FileSharingInfo> GetSearchResult(string proccessId,bool stop)
        {
            List<FileSharingInfo> current = SearchResult[proccessId].Item2;
            lock (SearchResult)
            {
                SearchResult[proccessId] = (SearchResult[proccessId].Item1==true ? true: stop, new List<FileSharingInfo>());
            }
            return current;
        }

        public static Dictionary<string, string> knownSIDs { get; set; } = new Dictionary<string, string>
        {
            ["Dialup"] = "S-1-5-1",
            ["Local account"] = "S-1-5-113",
            ["Administrators group"] = "S-1-5-114",
            ["Network"] = "S-1-5-2",
            ["Batch"] = "S-1-5-3",
            ["Interactive"] = "S-1-5-4",
            ["Service"] = "S-1-5-6",
            ["Anonymous Logon"] = "S-1-5-7",
            ["Proxy"] = "S-1-5-8",
            ["Enterprise Domain"] = "S-1-5-9",
            ["Self"] = "S-1-5-10",
            ["Authenticated Users"] = "S-1-5-11",
            ["Restricted Code"] = "S-1-5-12",
            ["Terminal Server User"] = "S-1-5-13",
            ["Remote Interactive"] = "S-1-5-14",
            ["This Organization"] = "S-1-5-15",
            ["IUSR"] = "S-1-5-17",
            ["System"] = "S-1-5-18",
            ["NT Authority"] = "S-1-5-19",
            ["Network Service"] = "S-1-5-20",
            ["Administrator"] = "S-1-5-domain-500",
            ["Guest"] = "S-1-5-domain-501",
            ["KRBTGT"] = "S-1-5-domain-502",
            ["Domain Admins"] = "S-1-5-domain-512",
            ["Domain Users"] = "S-1-5-domain-513",
            ["Domain Guests"] = "S-1-5-domain-514",
            ["Domain Computers"] = "S-1-5-domain-515",
            ["Domain Controllers"] = "S-1-5-domain-516",
            ["Cert Publishers"] = "S-1-5-domain-517",
            ["Schema Admins"] = "S-1-5-root domain-518",
            ["Enterprise Admins"] = "S-1-5-root domain-519",
            ["Group Policy Creator"] = "S-1-5-domain-520",
            ["Read-only Domain"] = "S-1-5-domain-521",
            ["Clonable Controllers"] = "S-1-5-domain-522",
            ["Protected Users"] = "S-1-5-domain-525",
            ["Key Admins"] = "S-1-5-root domain-526",
            ["Enterprise Key Admins"] = "S-1-5-domain-527",
            ["Administrators"] = "S-1-5-32-544",
            ["Users"] = "S-1-5-32-545",
            ["Guests"] = "S-1-5-32-546",
            ["Power Users"] = "S-1-5-32-547",
            ["Account Operators"] = "S-1-5-32-548",
            ["Server Operators"] = "S-1-5-32-549",
            ["Print Operators"] = "S-1-5-32-550",
            ["Backup Operators"] = "S-1-5-32-551",
            ["Replicators"] = "S-1-5-32-552",
            ["RAS and IAS Servers"] = "S-1-5-domain-553",
            ["Builtin Pre-Windows"] = "S-1-5-32-554",
            ["Builtin Remote Desktop"] = "S-1-5-32-555",
            ["Network Configuration"] = "S-1-5-32-556",
            ["Incoming Forest"] = "S-1-5-32-557",
            ["Performance Monitor Users"] = "S-1-5-32-558",
            ["Performance Log Users"] = "S-1-5-32-559",
            ["Windows Authorization Access"] = "S-1-5-32-560",
            ["Terminal Server License"] = "S-1-5-32-561",
            ["Distributed COM"] = "S-1-5-32-562",
            ["IIS_IUSRS"] = "S-1-5-32-568",
            ["Cryptographic Operators"] = "S-1-5-32-569",
            ["Allowed RODC"] = "S-1-5-domain-571",
            ["Denied RODC"] = "S-1-5-domain-572",
            ["Event Log Readers"] = "S-1-5-32-573",
            ["Certificate Service"] = "S-1-5-32-574",
            ["RDS Remote"] = "S-1-5-32-575",
            ["RDS Endpoint"] = "S-1-5-32-576",
            ["RDS Management"] = "S-1-5-32-577",
            ["Hyper-V Administrators"] = "S-1-5-32-578",
            ["Access Control Assistance"] = "S-1-5-32-579",
            ["Remote Management Users"] = "S-1-5-32-580",
            ["NTLM Authentication"] = "S-1-5-64-10",
            ["SChannel Authentication"] = "S-1-5-64-14",
            ["Digest Authentication"] = "S-1-5-64-21",
            ["NT Service"] = "S-1-5-80",
            ["All Services"] = "S-1-5-80-0",
            ["NT VIRTUAL MACHINE"] = "S-1-5-83-0",
            ["Storage Replica Administrators"] = "S-1-5-32-582",
        };

        public static DirFilePermission GetDirectoryOrFileUserPermission(ADUser aDUser, DirectorySecurity aclD = null, FileSecurity aclF = null, string ownerSid = null)
        {

            DirFilePermission dirFilePermission = new DirFilePermission();
            AuthorizationRuleCollection rules;

            List<string> ugSids = new List<string> { aDUser.Sid };

            if (aclD != null)
            {
                if (ownerSid == null)
                    ownerSid = aclD.GetOwner(typeof(SecurityIdentifier)).Value;
                if (aDUser.Sid == ownerSid)
                {
                    dirFilePermission.Read = true;
                    dirFilePermission.Write = true;
                    dirFilePermission.ReadAndExecute = true;
                    dirFilePermission.Modify = true;
                    dirFilePermission.FullControl = true;
                    dirFilePermission.Type = (int)AccessControlType.Allow;
                    dirFilePermission.IsOwner = true;

                    return dirFilePermission;

                }
                rules = aclD.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
            }
            else
            {
                if (ownerSid == null)
                    ownerSid = aclF.GetOwner(typeof(SecurityIdentifier)).Value;

                if (aDUser.Sid == ownerSid)
                {
                    dirFilePermission.Read = true;
                    dirFilePermission.Write = true;
                    dirFilePermission.ReadAndExecute = true;
                    dirFilePermission.Modify = true;
                    dirFilePermission.FullControl = true;
                    dirFilePermission.Type = (int)AccessControlType.Allow;
                    dirFilePermission.IsOwner = true;

                    return dirFilePermission;

                }
                rules = aclF.GetAccessRules(true, true, typeof(System.Security.Principal.SecurityIdentifier));
            }


            if (rules == null) return dirFilePermission;

            AuthorizationRuleCollection userRole = new AuthorizationRuleCollection();

            foreach (AuthorizationRule rule in rules)
            {
                if (ugSids.Contains(rule.IdentityReference.Value))
                    userRole.AddRule(rule);
            }

            if (userRole.Count == 0)
            {
                foreach (var g in aDUser.Groups)
                    ugSids.Add(g.Sid);

                foreach (AuthorizationRule rule in rules)
                {
                    if (ugSids.Contains(rule.IdentityReference.Value))
                        userRole.AddRule(rule);
                }
            }

            // SecurityIdentifier everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

            foreach (FileSystemAccessRule rule in rules)
            {
                if (rule.IdentityReference == Helper.everyone)
                {

                    if ((FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read)
                        dirFilePermission.Read = true;
                    if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write)
                        dirFilePermission.Write = true;
                    if ((FileSystemRights.ReadAndExecute & rule.FileSystemRights) == FileSystemRights.ReadAndExecute)
                        dirFilePermission.ReadAndExecute = true;
                    if ((FileSystemRights.FullControl & rule.FileSystemRights) == FileSystemRights.FullControl)
                        dirFilePermission.FullControl = true;
                    if ((FileSystemRights.Modify & rule.FileSystemRights) == FileSystemRights.Modify)
                        dirFilePermission.Modify = true;

                    dirFilePermission.Type = (int)rule.AccessControlType;

                    dirFilePermission.IsInherit = rule.IsInherited;

                }
            }

            foreach (FileSystemAccessRule rule in userRole)
            {

                if (rule.AccessControlType == AccessControlType.Allow)
                {
                    if ((FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read)
                        dirFilePermission.Read = true;
                    if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write)
                        dirFilePermission.Write = true;
                    if ((FileSystemRights.ReadAndExecute & rule.FileSystemRights) == FileSystemRights.ReadAndExecute)
                        dirFilePermission.ReadAndExecute = true;
                    if ((FileSystemRights.FullControl & rule.FileSystemRights) == FileSystemRights.FullControl)
                        dirFilePermission.FullControl = true;
                    if ((FileSystemRights.Modify & rule.FileSystemRights) == FileSystemRights.Modify)
                        dirFilePermission.Modify = true;
                    dirFilePermission.Type = (int)AccessControlType.Allow;
                    dirFilePermission.IsInherit = rule.IsInherited;


                }
            }

            foreach (FileSystemAccessRule rule in userRole)
            {

                if (rule.AccessControlType == AccessControlType.Deny)
                {
                    if ((FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read)
                        dirFilePermission.Read = false;
                    if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write)
                        dirFilePermission.Write = false;
                    if ((FileSystemRights.ReadAndExecute & rule.FileSystemRights) == FileSystemRights.ReadAndExecute)
                        dirFilePermission.ReadAndExecute = false;
                    if ((FileSystemRights.FullControl & rule.FileSystemRights) == FileSystemRights.FullControl)
                        dirFilePermission.FullControl = false;
                    if ((FileSystemRights.Modify & rule.FileSystemRights) == FileSystemRights.Modify)
                        dirFilePermission.Modify = false;
                    dirFilePermission.Type = (int)AccessControlType.Deny;
                    dirFilePermission.IsInherit = rule.IsInherited;


                }
            }

            return dirFilePermission;
        }

        public static (long? start, long? end) ParseRange(string range, long fileLength)
        {
            if (string.IsNullOrEmpty(range))
                return (null, null);

            var bytes = range.Replace("bytes=", "").Split('-');
            long? start = bytes[0] != "" ? Convert.ToInt64(bytes[0]) : (long?)null;
            long? end = bytes[1] != "" ? Convert.ToInt64(bytes[1]) : (long?)null;

            if (!start.HasValue && end.HasValue)
            {
                start = fileLength - end;
                end = fileLength - 1;
            }
            else if (start.HasValue && !end.HasValue)
            {
                end = fileLength - 1;
            }

            return (start, end);
        }
    }

    public static class DirectoryInfoExtensions
    {
        public static void DeepCopy(this DirectoryInfo directory, string destinationDir,ADUser aDUser)
        {

            foreach (string dir in Directory.GetDirectories(directory.FullName, "*", SearchOption.AllDirectories))
            {
                try
                {
                    DirectoryInfo source = new DirectoryInfo(@dir);
                    DirectorySecurity dSecurity = source.GetAccessControl(AccessControlSections.All);
                    var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, null);

                    if (per.Read)
                    {

                        string dirToCreate = dir.Replace(directory.FullName, destinationDir);
                        if (Directory.Exists(dirToCreate))
                        {
                            var count = directory.Parent.GetDirectories().Where(x => x.Name.StartsWith(dirToCreate)).Count() + 1;
                            var nD = dirToCreate + " - Copy(" + count.ToString() + ")";

                            if (Directory.Exists(@nD))
                            {
                                dirToCreate += " - Copy(" + DateTime.Now.Millisecond.ToString() + ")";
                            }
                            else
                                dirToCreate += " - Copy(" + count.ToString() + ")";


                        }

                        Directory.CreateDirectory(dirToCreate);
                    }
                }
                catch { }
            }

            foreach (string newPath in Directory.GetFiles(directory.FullName, "*.*", SearchOption.AllDirectories))
            {
                try 
                {
                    FileInfo file = new FileInfo(@newPath);
                    FileSecurity fileSecurity = file.GetAccessControl(AccessControlSections.All);
                    var perF = Helper.GetDirectoryOrFileUserPermission(aDUser, null, fileSecurity, null);
                    if (perF.Read)
                    {
                        File.Copy(newPath, newPath.Replace(directory.FullName, destinationDir), true);
                    }
                }
                catch { }
            }
        }

        public static void DeepMove(this DirectoryInfo directory, string destinationDir)
        {
            bool canDelete = true;
            foreach (string dir in Directory.GetDirectories(directory.FullName, "*", SearchOption.AllDirectories))
            {
                try
                { 
                    string dirToCreate = dir.Replace(directory.FullName, destinationDir);
                    //if (File.Exists(dirToCreate))
                    //    dirToCreate += "_Copy" + DateTime.Now.Millisecond.ToString();
                    DirectoryInfo source = new DirectoryInfo(@dir);

                    DirectorySecurity ndSecurity = source.GetAccessControl(AccessControlSections.All);

                    if (!File.Exists(dirToCreate))
                    Directory.CreateDirectory(dirToCreate);

                    DirectoryInfo dirToCreateInfo = new DirectoryInfo(@dirToCreate);
                    dirToCreateInfo.SetAccessControl(ndSecurity);
                }
                catch { canDelete = false; }
            }

            foreach (string newPath in Directory.GetFiles(directory.FullName, "*.*", SearchOption.AllDirectories))
            {
               try{ File.Move(newPath, newPath.Replace(directory.FullName, destinationDir), true); }
               catch { canDelete = false; }
            }

            try
            {
                if (canDelete)
                    directory.Delete(true);
            }
            catch { }
        }

    }
}
