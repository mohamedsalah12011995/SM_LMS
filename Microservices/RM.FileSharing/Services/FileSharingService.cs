

using Dasync.Collections;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using OfficeOpenXml;
using RM.Core.Services;
using RM.FileSharing.Dtos;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.IO.Compression;
using System.Security.AccessControl;
using System.Security.Principal;
using Xceed.Words.NET;
using RM.FileSharing.UnitOfWorks;
using RM.Core.Helpers;
using static RM.FileSharing.Dtos.OperationOutput;
using RM.FileSharing.Const;


namespace RM.FileSharing.Services
{
    public class FileSharingService:BaseService,IFileSharingService
    {  
        public string SharedFolderIP;
        public SharedFoldersPaths SharedFoldersPaths;
        public string DomainName;
        public string FileDomainName;
        public string PhysicalPath;
        SecurityIdentifier everyone;
        Dictionary<string, string> knownSIDs;
        public List<string> notAllowedExtensions;
        public IUnitOfWork _unitOfWork;

        public FileSharingService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IOptions<SharedFoldersPaths> sharedFoldersPaths)
        :base(httpContextAccessor,unitOfWork.Configuration)
        {
            _unitOfWork=unitOfWork;
            DomainName = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("DomainName").Value;
            FileDomainName = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("FileDomainName").Value;
            PhysicalPath = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("PhysicalPath").Value;
            SharedFoldersPaths = sharedFoldersPaths.Value;
            notAllowedExtensions = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("NotAllowedUploadExtensions").Value.Split(',').ToList();
            everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
            knownSIDs = Helper.knownSIDs;
        }

        public OperationOutput GetGroups(Dtos.FileSharingInfo RequestedData)
        {

            if (RequestOwner.Id == null || RequestedData.Path == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            PrincipalContext context = new PrincipalContext(ContextType.Domain, DomainName);
            GroupPrincipal groupPrincipal = new GroupPrincipal(context);

            PrincipalSearcher gSearcher = new PrincipalSearcher(groupPrincipal);


            var groups = gSearcher.FindAll().AsQueryable().Cast<GroupPrincipal>().OrderBy(x => x.Name).Select(x => new ADGroup { Name = x.Name, Sid = x.Sid.Value, Guid = x.Guid.Value, SamAccountName = x.SamAccountName }).ToList();

            WindowsIdentity identity = WindowsIdentity.GetCurrent();

            var filterdGroups = groups.Where(x => !knownSIDs.Values.Contains(x.Sid) && !identity.Groups.Select(x => x.Value).ToList().Contains(x.Sid) && !IsAdminsGroup(x.SamAccountName, RequestedData.Path)).ToList();

            if (IsSuperAdmin(RequestOwner.UserName))
                filterdGroups.Add(new ADGroup { Name = "Everyone", Sid = everyone.Value, Guid = Guid.Empty, SamAccountName = "Everyone" });

            List<Role> roleTypes = new List<Role> {
            new Role{NameAr="المجلد فقط",NameEn="Folder Only",ApplyTo=(int)RoleType.FolderOnly},
            new Role{NameAr="المجلد ,المجلدات الفرعية",NameEn="Folder,SubFolder",ApplyTo=(int)RoleType.FolderAndSubFolder},
            new Role{NameAr="المجلد ,(المجلدات / الملفات) الفرعية",NameEn="Folder,SubFolder,Files",ApplyTo=(int)RoleType.FolderAndSubFolderAndFiles},

            };

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.RoleTypes, roleTypes),
                   new OutputDictionary(OperationOutputKey.ADGroups, filterdGroups));

        }
        public OperationOutput GetGroupMembers(Dtos.FileSharingInfo RequestedData)
        {

            if (RequestOwner.Id == null || RequestedData.Path == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            PrincipalContext context = new PrincipalContext(ContextType.Domain, DomainName);
            GroupPrincipal groupPrincipal = new GroupPrincipal(context);

            PrincipalSearcher gSearcher = new PrincipalSearcher(groupPrincipal);


            var group = gSearcher.FindAll().AsQueryable().Cast<GroupPrincipal>().OrderBy(x => x.Name).Where(x => x.Guid.Value == RequestedData.Guid.Value).Select(x => new { x.Name, Sid = x.Sid.Value, Guid = x.Guid.Value, x.SamAccountName, Members = x.GetMembers().ToList() }).FirstOrDefault();

            if (!IsAdmin(RequestOwner.UserName, RequestedData.Path))
            {
                foreach (var usr in group.Members)
                {
                    if (IsAdmin(usr.SamAccountName, RequestedData.Path))
                        group.Members.Remove(usr);
                }
            }

            if (!IsSuperAdmin(RequestOwner.UserName))
            {
                foreach (var usr in group.Members)
                {
                    if (IsSuperAdmin(usr.SamAccountName))
                        group.Members.Remove(usr);
                }
            }

            var members= group != null ? group.Members.Select(x => new { x.Name, userPrincipalName = x.UserPrincipalName, Sid = x.Sid.Value, Guid = x.Guid.Value, x.SamAccountName, x.DisplayName }).ToList() : null;
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKey.ADUsers, members));

        }
        public OperationOutput UserOrGroupSearch(Dtos.FileSharingInfo RequestedData)
        {
            if (RequestOwner.Id == null || RequestedData.Path == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            PrincipalContext context = new PrincipalContext(ContextType.Domain, DomainName);
            GroupPrincipal groupPrincipal = new GroupPrincipal(context);

            PrincipalSearcher gSearcher = new PrincipalSearcher(groupPrincipal);


            var groups = gSearcher.FindAll().AsQueryable().Cast<GroupPrincipal>().OrderBy(x => x.Name).Where(x => x.Name.Contains(RequestedData.Name) && !IsAdminsGroup(x.SamAccountName, RequestedData.Path)).Select(x => new ADGroup { Name = x.Name, Guid = x.Guid.Value, SamAccountName = x.SamAccountName, Sid = x.Sid.Value }).ToList();

            if (IsSuperAdmin(RequestOwner.UserName))
            {
                string ev = "everyone";
                if (ev.Contains(RequestedData.Name.ToLower()))
                    groups.Add(new ADGroup { Name = "Everyone", Sid = everyone.Value, Guid = Guid.Empty, SamAccountName = "Everyone" });
            }

            UserPrincipal principal = new UserPrincipal(context);
            // principal.UserPrincipalName = RequestedData.Name+"*@*";
            principal.DisplayName = RequestedData.Name + "*";

            principal.Enabled = true;
            PrincipalSearcher searcher = new PrincipalSearcher(principal);

            var users = searcher.FindAll().AsQueryable().Cast<UserPrincipal>().OrderBy(x => x.Name).Select(x => new { x.Name, Sid = x.Sid.Value, Guid = x.Guid.Value, x.UserPrincipalName, x.SamAccountName, x.DisplayName }).ToList();

            UserPrincipal principal2 = new UserPrincipal(context);
            principal2.UserPrincipalName = RequestedData.Name + "*@*";

            principal.Enabled = true;
            PrincipalSearcher searcher2 = new PrincipalSearcher(principal2);

            users.AddRange(searcher2.FindAll().AsQueryable().Cast<UserPrincipal>().OrderBy(x => x.Name).Select(x => new { x.Name, Sid = x.Sid.Value, Guid = x.Guid.Value, x.UserPrincipalName, x.SamAccountName, x.DisplayName }).ToList());

            UserPrincipal principal3 = new UserPrincipal(context);
            principal3.Surname = RequestedData.Name + "*";

            principal.Enabled = true;
            PrincipalSearcher searcher3 = new PrincipalSearcher(principal3);

            users.AddRange(searcher3.FindAll().AsQueryable().Cast<UserPrincipal>().OrderBy(x => x.Name).Select(x => new { x.Name, Sid = x.Sid.Value, Guid = x.Guid.Value, x.UserPrincipalName, x.SamAccountName, x.DisplayName }).ToList());

            var dupes = users.GroupBy(s => new { s.Sid }).SelectMany(grp => grp.Skip(1)).ToList();
            foreach (var dup in dupes)
            {
                users.Remove(dup);
            }

            if (!IsAdmin(RequestOwner.UserName, RequestedData.Path))
            {
                var temp = users;
                foreach (var usr in temp)
                {
                    if (IsAdmin(usr.SamAccountName, RequestedData.Path))
                        users.Remove(usr);
                }
            }

            if (!IsSuperAdmin(RequestOwner.UserName))
            {
                var temp = users;
                foreach (var usr in temp)
                {
                    if (IsSuperAdmin(usr.SamAccountName))
                        users.Remove(usr);
                }
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.ADUsers, users),
                   new OutputDictionary(OperationOutputKey.ADGroups, groups.Where(x => !knownSIDs.Values.Contains(x.Sid)).ToList()));

        }
        public async Task<OperationOutput> GetDirectory(Dtos.FileSharingInfo RequestedData)
        {
            List<FileSharingInfo> Dirs = new List<FileSharingInfo>();
            List<FileSharingInfo> Files = new List<FileSharingInfo>();
            List<string> Paths = new List<string>();

            List<DirectoryInfo> dirs = new List<DirectoryInfo>();
            List<FileInfo> files = new List<FileInfo>();

            List<FileSharingInfo> Parents = new List<FileSharingInfo>();

            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            ADUser aDUser = getAdUser();

            if (aDUser == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            if (aDUser.IsLockout)
                return GetOperationOutput(header: Enums.ServiceMessages.UserIsBlocked);


            bool isAdmin = IsAdmin(RequestOwner.UserName, RequestedData.Path);
            string proccessId = TransactionDate.Ticks.ToString();

            if (RequestedData.Path != null)
            {
                string path = RequestedData.Path.Replace("//", "/");

                DirectoryInfo dInfo = new DirectoryInfo(@path);

                if (RequestedData.ParentFolder == true)
                {

                    dirs.AddRange(dInfo.Parent.GetDirectories().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)));
                    files.AddRange(dInfo.Parent.GetFiles().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)));

                    while (dInfo.Parent != null)
                    {
                        dInfo = dInfo.Parent;
                        DirectorySecurity dSecurity = dInfo.GetAccessControl(AccessControlSections.All);
                        var ownerSid = dSecurity.GetOwner(typeof(SecurityIdentifier)).Value;
                        var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, ownerSid);
                        Parents.Add(new FileSharingInfo { Path = dInfo.FullName, Name = dInfo.Name, Permission = per });
                    }
                }
                else if (string.IsNullOrEmpty(RequestedData.Name) && string.IsNullOrEmpty(RequestedData.SearchWord) && !RequestedData.ModifiedToday.HasValue && !RequestedData.ModifiedWeek.HasValue && !RequestedData.ModifiedMonth.HasValue && !RequestedData.FromModifiedDate.HasValue)
                {
                    dirs.AddRange(dInfo.GetDirectories().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)));
                    files.AddRange(dInfo.GetFiles().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden)));
                }
                else
                {
                    Helper.SearchResult.Add(proccessId.ToString(), (false, new List<FileSharingInfo>()));



                    // Files.AddRange(await SearchFiles(dInfo, RequestedData.Name, aDUser, isAdmin, RequestedData.ModifiedToday , RequestedData.ModifiedWeek , RequestedData.ModifiedMonth , RequestedData.FromModifiedDate, RequestedData.ToModifiedDate,RequestedData.SearchWord));

                    if (string.IsNullOrEmpty(RequestedData.SearchWord))
                        SearchDirectories(dInfo, RequestedData.Name, aDUser, isAdmin, RequestedData.ModifiedToday, RequestedData.ModifiedWeek, RequestedData.ModifiedMonth, RequestedData.FromModifiedDate, RequestedData.ToModifiedDate, proccessId);
                    else
                        Dirs = new List<FileSharingInfo>();

                    var dirResult = Helper.GetSearchResult(proccessId, false);

                    Thread th = new Thread(() =>
                    {
                        SearchFiles(dInfo, RequestedData.Name, aDUser, isAdmin, RequestedData.ModifiedToday, RequestedData.ModifiedWeek, RequestedData.ModifiedMonth, RequestedData.FromModifiedDate, RequestedData.ToModifiedDate, RequestedData.SearchWord, proccessId);
                        Helper.SearchResult[proccessId] = (true, Helper.SearchResult[proccessId].Item2);
                    });
                    th.Start();
                    Thread.Sleep(500);

                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                           new OutputDictionary(OperationOutputKey.ProccessId, proccessId),
                           new OutputDictionary(OperationOutputKey.Directories, dirResult),
                           new OutputDictionary(OperationOutputKey.Files, Files));
                }

                foreach (var dir in dirs)
                {
                    try
                    {
                        DirectorySecurity dSecurity = dir.GetAccessControl(AccessControlSections.All);
                        var ownerSid = dSecurity.GetOwner(typeof(SecurityIdentifier)).Value;
                        var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, ownerSid);


                        if (per.Type == (int)AccessControlType.Allow && per.Read || aDUser.Sid == ownerSid)
                        {
                            var owner = "";
                            if (Helper.NTAccounts.ContainsKey(ownerSid))
                            {
                                owner = Helper.NTAccounts[ownerSid];
                            }
                            else
                            {
                                owner = dSecurity.GetOwner(typeof(NTAccount)).Value;
                                Helper.NTAccounts.Add(ownerSid, owner);
                            }

                            var Owner = owner.Split("\\");

                            per.IsAdmin = isAdmin;
                            Dirs.Add(new FileSharingInfo { Path = dir.FullName, Name = dir.Name, Owner = Owner.Length > 0 ? Owner[1] : owner, Size = "", SizeKb = 0, Type = "Folder", Permission = per, ModifiedDate = dir.LastWriteTimeUtc.ToString("yyyy-MM-dd HH:mm:ss") });
                        }
                    }
                    // catch (Exception ex) { _logger.LogError("{ExceptionMessage} | {ExceptionTrace} | {RecordInformation}", ex.Message, ex.StackTrace, RequestedData); }
                    catch { }
                }

                foreach (var file in files)
                {
                    try
                    {
                        FileSecurity fileSecurity = file.GetAccessControl(AccessControlSections.All);
                        var ownerSid = fileSecurity.GetOwner(typeof(SecurityIdentifier)).Value;

                        var per = Helper.GetDirectoryOrFileUserPermission(aDUser, null, fileSecurity, ownerSid);

                        if (per.Type == (int)AccessControlType.Allow && per.Read || aDUser.Sid == ownerSid)
                        {
                            var owner = "";
                            if (Helper.NTAccounts.ContainsKey(ownerSid))
                            {
                                owner = Helper.NTAccounts[ownerSid];
                            }
                            else
                            {
                                owner = fileSecurity.GetOwner(typeof(NTAccount)).Value;
                                Helper.NTAccounts.Add(ownerSid, owner);
                            }

                            per.IsAdmin = isAdmin;
                            var Owner = owner.Split("\\");

                            Files.Add(new FileSharingInfo { Url = GetWebDavUrl(file.Extension, per.Write, file.FullName), Path = file.FullName, Name = file.Name, Type = file.Extension, Owner = Owner.Length > 0 ? Owner[1] : owner, Size = GetSizeWithUnit(file.Length), SizeKb = file.Length, Permission = per, ModifiedDate = file.LastWriteTimeUtc.ToString("yyyy-MM-dd HH:mm:ss") });
                        }
                    }
                    catch { }

                }
            }
            else
            {
                foreach (var Folder in SharedFoldersPaths.FoldersList)
                {
                    try
                    {
                        DirectoryInfo dir = new DirectoryInfo(@Folder.FolderPath);
                        DirectorySecurity dSecurity = dir.GetAccessControl(AccessControlSections.All);

                        var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, null);

                        if (per.Type == (int)AccessControlType.Allow && per.Read)
                        {
                            per.IsAdmin = isAdmin;
                            Dirs.Add(new FileSharingInfo { Path = dir.FullName, Name = dir.Name, Owner = "", Size = "", SizeKb = 0, Type = "Folder", Permission = per, ModifiedDate = dir.LastWriteTimeUtc.ToString("yyyy-MM-dd HH:mm:ss") });
                        }
                    }
                    catch { }

                }

                if (!string.IsNullOrEmpty(RequestedData.Name) || !string.IsNullOrEmpty(RequestedData.SearchWord) || RequestedData.ModifiedToday.HasValue || RequestedData.ModifiedWeek.HasValue || RequestedData.ModifiedMonth.HasValue || RequestedData.FromModifiedDate.HasValue)
                {
                    Helper.SearchResult.Add(proccessId.ToString(), (false, new List<FileSharingInfo>()));

                    if (string.IsNullOrEmpty(RequestedData.SearchWord))
                    {
                        foreach (var dir in Dirs)
                        {
                            DirectoryInfo dInfo = new DirectoryInfo(@dir.Path);

                            SearchDirectories(dInfo, RequestedData.Name, aDUser, isAdmin, RequestedData.ModifiedToday, RequestedData.ModifiedWeek, RequestedData.ModifiedMonth, RequestedData.FromModifiedDate, RequestedData.ToModifiedDate, proccessId);
                        }
                    }

                    var dirResult = Helper.GetSearchResult(proccessId, false);
                    Thread th = new Thread(() =>
                    {
                        foreach (var dir in Dirs)
                        {
                            DirectoryInfo dInfo = new DirectoryInfo(@dir.Path);
                            //  Files.AddRange(await SearchFiles(dInfo, RequestedData.Name, aDUser, isAdmin, RequestedData.ModifiedToday, RequestedData.ModifiedWeek, RequestedData.ModifiedMonth, RequestedData.FromModifiedDate, RequestedData.ToModifiedDate,RequestedData.SearchWord));

                            SearchFiles(dInfo, RequestedData.Name, aDUser, isAdmin, RequestedData.ModifiedToday, RequestedData.ModifiedWeek, RequestedData.ModifiedMonth, RequestedData.FromModifiedDate, RequestedData.ToModifiedDate, RequestedData.SearchWord, proccessId);

                        }
                        Helper.SearchResult[proccessId] = (true, Helper.SearchResult[proccessId].Item2);

                    });
                    th.Start();
                    Thread.Sleep(500);

                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                           new OutputDictionary(OperationOutputKey.ProccessId, proccessId),
                           new OutputDictionary(OperationOutputKey.Directories, dirResult),
                           new OutputDictionary(OperationOutputKey.Files, Files));
                }
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKey.ProccessId, proccessId),
                       new OutputDictionary(OperationOutputKey.Directories, Dirs),
                       new OutputDictionary(OperationOutputKey.Files, Files),
                       new OutputDictionary(OperationOutputKey.Parents, Parents));

        }
        public bool SearchFiles(DirectoryInfo dInfo, string fileName, ADUser aDUser, bool isAdmin, bool? ModifiedToday, bool? ModifiedWeek, bool? ModifiedMonth, DateTime? FromModifiedDate, DateTime? ToModifiedDate, string searchWord, string proccessId)
        {
            //  List<FileSharingInfo> Files = new List<FileSharingInfo>();

            try
            {
                List<FileInfo> searchResult = new List<FileInfo>();
                var dirs = dInfo.GetDirectories().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));
                searchResult.AddRange(dInfo.GetFiles()
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Where(f => !string.IsNullOrEmpty(fileName) ? f.Name.Contains(fileName) : true)
                    .Where(f => ModifiedToday == true ? f.LastWriteTimeUtc.Date == TransactionDate.Date : true)
                    .Where(f => ModifiedWeek == true ? f.LastWriteTimeUtc.Date >= TransactionDate.AddDays(-6).Date : true)
                    .Where(f => ModifiedMonth == true ? f.LastWriteTimeUtc.Date.Month == TransactionDate.Date.Month && f.LastWriteTimeUtc.Date.Year == TransactionDate.Date.Year : true)
                    .Where(f => ModifiedMonth == true ? f.LastWriteTimeUtc.Date.Month == TransactionDate.Date.Month && f.LastWriteTimeUtc.Date.Year == TransactionDate.Date.Year : true)
                    .Where(f => FromModifiedDate.HasValue && !ToModifiedDate.HasValue ? f.LastWriteTimeUtc.Date == FromModifiedDate.Value.Date : true)
                    .Where(f => FromModifiedDate.HasValue && ToModifiedDate.HasValue ? f.LastWriteTimeUtc.Date >= FromModifiedDate.Value.Date && f.LastWriteTimeUtc.Date <= ToModifiedDate.Value.Date : true)
                    .Where(f => string.IsNullOrEmpty(searchWord) || FileContainsText(f.FullName, searchWord)));


                foreach (var file in searchResult)
                {
                    try
                    {
                        if (!Helper.SearchResult.Keys.Contains(proccessId) || Helper.SearchResult[proccessId].Item1)
                            return true;

                        FileSecurity fileSecurity = file.GetAccessControl(AccessControlSections.All);
                        var ownerSid = fileSecurity.GetOwner(typeof(SecurityIdentifier)).Value;

                        var per = Helper.GetDirectoryOrFileUserPermission(aDUser, null, fileSecurity, ownerSid);

                        if (per.Type == (int)AccessControlType.Allow && per.Read || aDUser.Sid == ownerSid)
                        {
                            var owner = "";
                            if (Helper.NTAccounts.ContainsKey(ownerSid))
                            {
                                owner = Helper.NTAccounts[ownerSid];
                            }
                            else
                            {
                                owner = fileSecurity.GetOwner(typeof(NTAccount)).Value;
                                Helper.NTAccounts.Add(ownerSid, owner);
                            }

                            per.IsAdmin = isAdmin;
                            var Owner = owner.Split("\\");

                            //  Files.Add(new FileSharingInfo { Url = GetWebDavUrl(file.Extension, per.Write, file.FullName), Path = file.FullName, Name = file.Name, Type = file.Extension, Owner = Owner.Length > 0 ? Owner[1] : owner, Size = GetSizeWithUnit(file.Length), SizeKb = file.Length, Permission = per, ModifiedDate = file.LastWriteTimeUtc.ToString("yyyy-MM-dd HH:mm:ss") });

                            int retryCount = 0;
                            while (retryCount < 5)
                            {
                                try
                                {
                                    Helper.SearchResult[proccessId].Item2.Add(new FileSharingInfo { Url = GetWebDavUrl(file.Extension, per.Write, file.FullName), Path = file.FullName, Name = file.Name, Type = file.Extension, Owner = Owner.Length > 0 ? Owner[1] : owner, Size = GetSizeWithUnit(file.Length), SizeKb = file.Length, Permission = per, ModifiedDate = file.LastWriteTimeUtc.ToString("yyyy-MM-dd HH:mm:ss") });
                                    break;
                                }
                                catch
                                {
                                    retryCount++;
                                    Thread.Sleep(500);
                                }
                            }

                        }
                    }
                    catch { }
                }

                foreach (var dir in dirs)
                {
                    if (!Helper.SearchResult.Keys.Contains(proccessId) || Helper.SearchResult[proccessId].Item1)
                        return true;

                    //  Files.AddRange( SearchFiles(dir, fileName, aDUser, isAdmin,ModifiedToday,ModifiedWeek,ModifiedMonth,FromModifiedDate,ToModifiedDate, searchWord,proccessId));
                    SearchFiles(dir, fileName, aDUser, isAdmin, ModifiedToday, ModifiedWeek, ModifiedMonth, FromModifiedDate, ToModifiedDate, searchWord, proccessId);
                }
            }
            catch { }
            return true;
        }
        public bool SearchDirectories(DirectoryInfo dInfo, string fileName, ADUser aDUser, bool isAdmin, bool? ModifiedToday, bool? ModifiedWeek, bool? ModifiedMonth, DateTime? FromModifiedDate, DateTime? ToModifiedDate, string proccessId)
        {

            // List<FileSharingInfo> Dirs = new List<FileSharingInfo>();

            try
            {
                List<DirectoryInfo> searchResult = new List<DirectoryInfo>();
                var dirs = dInfo.GetDirectories().Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));
                searchResult.AddRange(dInfo.GetDirectories()
                    .Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden))
                    .Where(f => !string.IsNullOrEmpty(fileName) ? f.Name.Contains(fileName) : true)
                    .Where(f => ModifiedToday == true ? f.LastWriteTimeUtc.Date == TransactionDate.Date : true)
                    .Where(f => ModifiedWeek == true ? f.LastWriteTimeUtc.Date >= TransactionDate.AddDays(-6).Date : true)
                    .Where(f => ModifiedMonth == true ? f.LastWriteTimeUtc.Date.Month == TransactionDate.Date.Month && f.LastWriteTimeUtc.Date.Year == TransactionDate.Date.Year : true)
                    .Where(f => ModifiedMonth == true ? f.LastWriteTimeUtc.Date.Month == TransactionDate.Date.Month && f.LastWriteTimeUtc.Date.Year == TransactionDate.Date.Year : true)
                    .Where(f => FromModifiedDate.HasValue && !ToModifiedDate.HasValue ? f.LastWriteTimeUtc.Date == FromModifiedDate.Value.Date : true)
                    .Where(f => FromModifiedDate.HasValue && ToModifiedDate.HasValue ? f.LastWriteTimeUtc.Date >= FromModifiedDate.Value.Date && f.LastWriteTimeUtc.Date <= ToModifiedDate.Value.Date : true));


                foreach (var dir in searchResult)
                {
                    try
                    {
                        DirectorySecurity dSecurity = dir.GetAccessControl(AccessControlSections.All);
                        var ownerSid = dSecurity.GetOwner(typeof(SecurityIdentifier)).Value;
                        var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, ownerSid);


                        if (per.Type == (int)AccessControlType.Allow && per.Read || aDUser.Sid == ownerSid)
                        {
                            var owner = "";
                            if (Helper.NTAccounts.ContainsKey(ownerSid))
                            {
                                owner = Helper.NTAccounts[ownerSid];
                            }
                            else
                            {
                                owner = dSecurity.GetOwner(typeof(NTAccount)).Value;
                                Helper.NTAccounts.Add(ownerSid, owner);
                            }

                            var Owner = owner.Split("\\");

                            per.IsAdmin = isAdmin;
                            //  Dirs.Add(new FileSharingInfo { Path = dir.FullName, Name = dir.Name, Owner = Owner.Length > 0 ? Owner[1] : owner, Size = "", SizeKb = 0, Type = "Folder", Permission = per, ModifiedDate = dir.LastWriteTimeUtc.ToString("yyyy-MM-dd HH:mm:ss") });
                            int retryCount = 0;
                            while (retryCount < 5)
                            {
                                try
                                {
                                    Helper.SearchResult[proccessId].Item2.Add(new FileSharingInfo { Path = dir.FullName, Name = dir.Name, Owner = Owner.Length > 0 ? Owner[1] : owner, Size = "", SizeKb = 0, Type = "Folder", Permission = per, ModifiedDate = dir.LastWriteTimeUtc.ToString("yyyy-MM-dd HH:mm:ss") });
                                    break;
                                }
                                catch
                                {
                                    retryCount++;
                                    Thread.Sleep(500);
                                }
                            }

                        }
                    }
                    catch { }
                }

                foreach (var dir in dirs)
                {
                    if (!Helper.SearchResult.Keys.Contains(proccessId) || Helper.SearchResult[proccessId].Item1)
                        return true;
                    // Dirs.AddRange(await SearchDirectories(dir, fileName, aDUser, isAdmin, ModifiedToday, ModifiedWeek, ModifiedMonth, FromModifiedDate, ToModifiedDate));
                    SearchDirectories(dir, fileName, aDUser, isAdmin, ModifiedToday, ModifiedWeek, ModifiedMonth, FromModifiedDate, ToModifiedDate, proccessId);
                }
            }
            catch { }
            return true;

        }
        public OperationOutput GetSearchResult(FileSharingInfo RequestedData)
        {
            List<FileSharingInfo> Files = new List<FileSharingInfo>();
            if (Helper.SearchResult.Keys.Contains(RequestedData.ProccessId))
            {

                Files = Helper.GetSearchResult(RequestedData.ProccessId, RequestedData.StopProccess);
                if (Helper.SearchResult[RequestedData.ProccessId].Item1)
                {
                    Helper.SearchResult.Remove(RequestedData.ProccessId);
                }
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.StopProccess, !Helper.SearchResult.Keys.Contains(RequestedData.ProccessId)),
                   new OutputDictionary(OperationOutputKey.Directories, new List<FileSharingInfo>()),
                   new OutputDictionary(OperationOutputKey.Files, Files));
        }
        static bool FileContainsText(string filePath, string searchText)
        {
            try
            {
                var extension = Path.GetExtension(filePath);
                if (extension == ".txt")
                    return File.ReadAllLines(filePath).Any(line => line.Contains(searchText));
                else if (extension == ".doc" || extension == ".docx")
                {
                    using (var doc = DocX.Load(filePath))
                    {
                        foreach (var paragraph in doc.Paragraphs)
                        {
                            if (paragraph.Text.Contains(searchText))
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                }
                else if (extension == ".xls" || extension == ".xlsx")
                {
                    using (var package = new ExcelPackage(new FileInfo(filePath)))
                    {
                        foreach (var worksheet in package.Workbook.Worksheets)
                        {
                            foreach (var cell in worksheet.Cells)
                            {
                                if (cell.Text.Contains(searchText))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public OperationOutput GetDirFileAccessRules(Dtos.FileSharingInfo RequestedData)
        {

            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            ADUser aDUser = getAdUser();

            if (aDUser == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);


            if (aDUser.IsLockout)
                return GetOperationOutput(header: Enums.ServiceMessages.UserIsBlocked);


            List<DirFileAccessRule> dirFileAccessRule = GetDirFileDomainAccessRules(aDUser, RequestedData.Path, RequestedData.Type);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.DirFileAccessRule, dirFileAccessRule));

        }
        public OperationOutput ChangeDirFileAccessRules(FileSharingInfo RequestedData)
        {
            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            ADUser aDUser = getAdUser();

            if (aDUser == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            if (aDUser.IsLockout)
                return GetOperationOutput(header: Enums.ServiceMessages.UserIsBlocked);


            string proccessId = TransactionDate.Ticks.ToString();
            Helper.PermissionThreads.Add(proccessId.ToString(), (0, 0));

            string path = RequestedData.Path.Replace("//", "/");
            //  List<DirFileAccessRule> dirFileAccessRule = GetDirFileDomainAccessRules(RequestedData.Path);
            List<string> NotRemovedRules = new List<string>();
            if (RequestedData.Type == "Folder")
            {

                DirectoryInfo dInfo = new DirectoryInfo(@path);
                DirectorySecurity dSecurity = dInfo.GetAccessControl(AccessControlSections.All);
                var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, null);

                if (!per.FullControl && !IsAdmin(RequestOwner.UserName, RequestedData.Path))
                    return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


                var ownerSid = dSecurity.GetOwner(typeof(SecurityIdentifier)).Value;
                // WindowsIdentity identity = WindowsIdentity.GetCurrent();
                AuthorizationRuleCollection rules = dSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));
                //  var cuserIdR = new NTAccount(DomainName, aDUser.Name);
                foreach (FileSystemAccessRule rule in rules)
                {
                    if (rule.IdentityReference.Value != aDUser.Sid && !knownSIDs.Values.Contains(rule.IdentityReference.Value) && rule.IdentityReference.Value != ownerSid && !rule.IsInherited)
                        dSecurity.RemoveAccessRule(rule);
                    else
                    {
                        // NotRemovedRules.Add(rule.IdentityReference.Value);
                        var r = RequestedData.DirFileAccessRules.Find(x => x.Sid == rule.IdentityReference.Value);
                        if (r != null)
                            RequestedData.DirFileAccessRules.Remove(r);
                    }

                }

                foreach (var acc in RequestedData.DirFileAccessRules)
                {
                    //  if (acc.Sid != aDUser.Sid && acc.Sid != ownerSid && !NotRemovedRules.Contains(acc.Sid))
                    //  {
                    if (acc.ApplyTo == (int)RoleType.FolderOnly)
                    {
                        if (acc.Permission.Read) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Read, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                        if (acc.Permission.Write) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Write, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                        if (acc.Permission.ReadAndExecute) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.ReadAndExecute, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                        if (acc.Permission.FullControl) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.FullControl, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                        if (acc.Permission.Modify) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Modify, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                    }
                    else if (acc.ApplyTo == (int)RoleType.FolderAndSubFolder)
                    {
                        if (acc.Permission.Read) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Read, InheritanceFlags.ContainerInherit, PropagationFlags.None, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                        if (acc.Permission.Write) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Write, InheritanceFlags.ContainerInherit, PropagationFlags.None, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                        if (acc.Permission.ReadAndExecute) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit, PropagationFlags.None, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                        if (acc.Permission.FullControl) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.None, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                        if (acc.Permission.Modify) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Modify, InheritanceFlags.ContainerInherit, PropagationFlags.None, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                    }
                    else
                    {
                        if (acc.Permission.Read) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Read, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                        if (acc.Permission.Write) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Write, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                        if (acc.Permission.ReadAndExecute) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                        if (acc.Permission.FullControl) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                        if (acc.Permission.Modify) dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                    }
                    //  }
                }

                Helper.Start(proccessId);
                Thread th = new Thread(() =>
                {
                    const int maxRetries = 10;
                    int retryCount = 0;

                    while (retryCount < maxRetries)
                    {
                        try
                        {
                            dInfo.SetAccessControl(dSecurity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            retryCount++;
                            Thread.Sleep(1000);
                        }
                    }

                    Helper.Finish(proccessId);

                });
                th.Start();
                Thread.Sleep(500);

                if (RequestedData.AddRootPermission == true)
                    AddRootReadPermission(RequestedData.Path, RequestedData.DirFileAccessRules, proccessId);

            }
            else
            {
                FileInfo fInfo = new FileInfo(@path);
                FileSecurity fSecurity = fInfo.GetAccessControl(AccessControlSections.All);
                var per = Helper.GetDirectoryOrFileUserPermission(aDUser, null, fSecurity, null);
                if (!per.FullControl && !IsAdmin(RequestOwner.UserName, RequestedData.Path))
                    return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


                // WindowsIdentity identity = WindowsIdentity.GetCurrent();
                var ownerSid = fSecurity.GetOwner(typeof(SecurityIdentifier)).Value;

                AuthorizationRuleCollection rules = fSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));
                //  var cuserIdR = new NTAccount(DomainName, aDUser.Name);
                foreach (FileSystemAccessRule rule in rules)
                {
                    if (rule.IdentityReference.Value != aDUser.Sid && !knownSIDs.Values.Contains(rule.IdentityReference.Value) && rule.IdentityReference.Value != ownerSid && !rule.IsInherited)
                        fSecurity.RemoveAccessRule(rule);
                    else
                    {
                        // NotRemovedRules.Add(rule.IdentityReference.Value);
                        var r = RequestedData.DirFileAccessRules.Find(x => x.Sid == rule.IdentityReference.Value);
                        if (r != null)
                            RequestedData.DirFileAccessRules.Remove(r);
                    }
                }

                foreach (var acc in RequestedData.DirFileAccessRules)
                {
                    //  if (acc.Sid != aDUser.Sid && acc.Sid != ownerSid && !NotRemovedRules.Contains(acc.Sid))
                    //  {

                    if (acc.Permission.Read) fSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Read, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                    if (acc.Permission.Write) fSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Write, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                    if (acc.Permission.ReadAndExecute) fSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.ReadAndExecute, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                    if (acc.Permission.FullControl) fSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.FullControl, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                    if (acc.Permission.Modify) fSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Modify, acc.Permission.Type == 0 ? AccessControlType.Allow : AccessControlType.Deny));
                    // }
                    //  }
                }

                Helper.Start(proccessId);
                Thread th = new Thread(() =>
                {
                    const int maxRetries = 10;
                    int retryCount = 0;

                    while (retryCount < maxRetries)
                    {
                        try
                        {
                            fInfo.SetAccessControl(fSecurity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            retryCount++;
                            Thread.Sleep(1000);
                        }
                    }

                    Helper.Finish(proccessId);
                });
                th.Start();
                Thread.Sleep(500);

                if (RequestedData.AddRootPermission == true)
                    AddRootReadPermission(RequestedData.Path, RequestedData.DirFileAccessRules, proccessId);

            }

            List<DirFileAccessRule> dirFileAccessRule = GetDirFileDomainAccessRules(aDUser, RequestedData.Path, RequestedData.Type);

            int c = 0; int f = 0;
            (c, f) = Helper.PermissionThreads[proccessId];

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.DirFileAccessRule, dirFileAccessRule),
                   new OutputDictionary(OperationOutputKey.ProccessId, proccessId),
                   new OutputDictionary(OperationOutputKey.StartCount, c),
                   new OutputDictionary(OperationOutputKey.FinishCount, f));

        }
        public void AddRootReadPermission(string fullPath, List<DirFileAccessRule> accs, string proccessId)
        {
            List<string> roots = fullPath.Replace("\\\\", "").Split('\\').ToList();
            ADUser aDUser = new ADUser();

            string path = roots.First();
            if (fullPath.Contains("\\\\"))
            {
                path = "\\\\" + path;
                roots.RemoveAt(0);
            }

            if (roots.Count() > 1)
            {
                roots.Remove(roots[roots.Count() - 1]);
            }

            foreach (string root in roots)
            {
                //  if (root.Contains(".")) break;

                path = Path.Combine(path, root);
                DirectoryInfo dInfo = new DirectoryInfo(@path);
                DirectorySecurity dSecurity = dInfo.GetAccessControl(AccessControlSections.All);

                foreach (var acc in accs)
                {
                    DirFilePermission per = new DirFilePermission();
                    if (acc.Type == "User")
                    {
                        aDUser = GetADUserByUserPrincipalName(acc.UserPrincipalName);
                        per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, null);
                    }
                    else
                    {
                        per = GetDirectoryOrFileGroupPermission(acc.Name, dSecurity, null);
                    }

                    if (!per.Read)
                    {
                        dSecurity.AddAccessRule(new FileSystemAccessRule(acc.Sid == everyone.Value ? everyone : new SecurityIdentifier(acc.Sid), FileSystemRights.Read, AccessControlType.Allow));
                    }
                }

                Helper.Start(proccessId);
                Thread th = new Thread(() =>
                {
                    const int maxRetries = 10;
                    int retryCount = 0;

                    while (retryCount < maxRetries)
                    {
                        try
                        {
                            dInfo.SetAccessControl(dSecurity);
                            break;
                        }
                        catch (Exception ex)
                        {
                            retryCount++;
                            Thread.Sleep(1000);
                        }
                    }
                    Helper.Finish(proccessId);
                });
                th.Start();
                Thread.Sleep(500);
            }
        }
        public async Task<OperationOutput> UploadFileAsync(Stream fileStream, string contentType)
        {
            string path = "";

            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            ADUser aDUser = getAdUser();

            if (aDUser == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);


            if (aDUser.IsLockout)
                return GetOperationOutput(header: Enums.ServiceMessages.UserIsBlocked);


            var fileCount = 0;
            long totalSizeInBytes = 0;

            var boundary = GetBoundary(MediaTypeHeaderValue.Parse(contentType));
            var multipartReader = new MultipartReader(boundary, fileStream);

            var filePaths = new List<string>();
            var notUploadedFiles = new List<string>();

            var section = await multipartReader.ReadNextSectionAsync();
            //   var path = section.AsFileSection().Name;

            while (section != null)
            {
                var fileSection = section.AsFileSection();
                if (fileSection != null)
                {
                    if (path == "")
                    {
                        path = fileSection.Name;
                        DirectoryInfo dir = new DirectoryInfo(@path);
                        DirectorySecurity dSecurity = dir.GetAccessControl(AccessControlSections.All);

                        var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, null);

                        if (!per.Write)
                            return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

                    }


                    totalSizeInBytes += await SaveFileAsync(fileSection.Name, fileSection, filePaths, notUploadedFiles);
                    fileCount++;
                }

                section = await multipartReader.ReadNextSectionAsync();
            }

            foreach (var fp in filePaths)
            {
                FileInfo ndir = new FileInfo(@fp);

                FileSecurity nfSecurity = ndir.GetAccessControl(AccessControlSections.All);

                bool hasRole = false;
                AuthorizationRuleCollection rules = nfSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));

                foreach (FileSystemAccessRule rule in rules)
                {
                    if (rule.IdentityReference.Value == aDUser.Sid)
                        hasRole = true;
                }

                if (!hasRole)
                {
                    nfSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(aDUser.Sid), FileSystemRights.Read, AccessControlType.Allow));
                    nfSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(aDUser.Sid), FileSystemRights.Write, AccessControlType.Allow));
                    nfSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(aDUser.Sid), FileSystemRights.Modify, AccessControlType.Allow));
                    nfSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(aDUser.Sid), FileSystemRights.ReadAndExecute, AccessControlType.Allow));
                    nfSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(aDUser.Sid), FileSystemRights.FullControl, AccessControlType.Allow));
                    ndir.SetAccessControl(nfSecurity);

                }
                try
                {
                    SetOwner(fp, RequestOwner.UserName);
                }
                catch
                { }
            }

            FileUploadSummary fileUploadSummary = new FileUploadSummary
            {
                TotalFilesUploaded = fileCount,
                TotalSizeUploaded = ConvertSizeToString(totalSizeInBytes),
                FilePaths = filePaths,
                NotUploadedFiles = notUploadedFiles
            };

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                        new OutputDictionary(OperationOutputKey.FileUploadSummary, fileUploadSummary));
        }
        public OperationOutput CreateDirectory(FileSharingInfo RequestedData)
        {

            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            ADUser aDUser = getAdUser();
            if (aDUser == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            if (aDUser.IsLockout)
                return GetOperationOutput(header: Enums.ServiceMessages.UserIsBlocked);

            string path = RequestedData.Path.Replace("//", "/");
            DirectoryInfo dir = new DirectoryInfo(@path);
            DirectorySecurity dSecurity = dir.GetAccessControl(AccessControlSections.All);

            var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, null);

            if (!per.Write)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


            string nPath = Path.Combine(path, RequestedData.Name);

            if (Directory.Exists(nPath))
                return GetOperationOutput(header: Enums.ServiceMessages.DirectoryExist);


            Directory.CreateDirectory(nPath);
            DirectoryInfo ndir = new DirectoryInfo(@nPath);

            DirectorySecurity ndSecurity = ndir.GetAccessControl(AccessControlSections.All);

            bool hasRole = false;
            AuthorizationRuleCollection rules = ndSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));

            foreach (FileSystemAccessRule rule in rules)
            {
                if (rule.IdentityReference.Value == aDUser.Sid)
                    hasRole = true;
            }

            if (!hasRole)
            {
                ndSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(aDUser.Sid), FileSystemRights.Read, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                ndSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(aDUser.Sid), FileSystemRights.Write, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                ndSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(aDUser.Sid), FileSystemRights.Modify, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                ndSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(aDUser.Sid), FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                ndSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(aDUser.Sid), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow));
                ndir.SetAccessControl(ndSecurity);

            }
            try
            {
                //WindowsIdentity identity = new WindowsIdentity(aDUser.Name);

                //ndSecurity.SetOwner(identity.Owner);
                //ndir.SetAccessControl(ndSecurity);
                SetOwner(nPath, RequestOwner.UserName);

            }
            catch
            { }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }
        public OperationOutput DeleteFileOrDirectory(List<FileSharingInfo> RequestedData)
        {

            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            ADUser aDUser = getAdUser();

            if (aDUser == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);


            if (aDUser.IsLockout)
                return GetOperationOutput(header: Enums.ServiceMessages.UserIsBlocked);


            foreach (var file in RequestedData)
            {
                string path = file.Path.Replace("//", "/");

                if (file.Type == "Folder")
                {
                    DirectoryInfo dir = new DirectoryInfo(@path);
                    DirectorySecurity dSecurity = dir.GetAccessControl(AccessControlSections.All);

                    var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, null);

                    if (!per.FullControl)
                        return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);
                }
                else
                {
                    FileInfo dir = new FileInfo(@path);
                    FileSecurity fSecurity = dir.GetAccessControl(AccessControlSections.All);

                    var per = Helper.GetDirectoryOrFileUserPermission(aDUser, null, fSecurity, null);

                    if (!per.FullControl)
                        return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);
                }
            }

            Parallel.ForEach(RequestedData, file =>
            {
                string path = file.Path.Replace("//", "/");

                if (file.Type == "Folder")
                {
                    DirectoryInfo dir = new DirectoryInfo(@path);
                    dir.Delete(true);
                }
                else
                {
                    FileInfo dir = new FileInfo(@path);
                    dir.Delete();
                }
            });

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }
        public OperationOutput RenameFileOrDirectory(FileSharingInfo RequestedData)
        {
            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            ADUser aDUser = getAdUser();

            if (aDUser == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);


            if (aDUser.IsLockout)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            string path = RequestedData.Path.Replace("//", "/");
            //      string nPath = System.IO.Path.Combine(path, RequestedData.Name);

            if (RequestedData.Type == "Folder")
            {
                DirectoryInfo dir = new DirectoryInfo(@path);
                DirectorySecurity dSecurity = dir.GetAccessControl(AccessControlSections.All);

                var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, null);

                if (!per.FullControl)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


                if (Directory.Exists(Path.Combine(dir.Parent.FullName, RequestedData.Name)))
                    return GetOperationOutput(header: Enums.ServiceMessages.DirectoryExist);

                dir.MoveTo(Path.Combine(dir.Parent.FullName, RequestedData.Name));
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(@path);

                FileInfo file = new FileInfo(@path);
                FileSecurity fSecurity = file.GetAccessControl(AccessControlSections.All);

                var per = Helper.GetDirectoryOrFileUserPermission(aDUser, null, fSecurity, null);

                if (!per.FullControl)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


                if (File.Exists(Path.Combine(dir.Parent.FullName, RequestedData.Name + file.Extension)))
                    return GetOperationOutput(header: Enums.ServiceMessages.FileExist);

                file.MoveTo(Path.Combine(dir.Parent.FullName, RequestedData.Name + file.Extension));
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }
        public OperationOutput CopyFileOrDirectory(FilesSharingInfo RequestedData)
        {

            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            ADUser aDUser = getAdUser();

            if (aDUser == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);


            if (aDUser.IsLockout)
                return GetOperationOutput(header: Enums.ServiceMessages.UserIsBlocked);


            string DestinationPath = RequestedData.Destination.Path.Replace("//", "/");

            DirectoryInfo Destination = new DirectoryInfo(@DestinationPath);
            DirectorySecurity destinationSecurity = Destination.GetAccessControl(AccessControlSections.All);

            var DestinationPer = Helper.GetDirectoryOrFileUserPermission(aDUser, destinationSecurity, null, null);

            if (!DestinationPer.Write)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


            foreach (var file in RequestedData.Sources)
            {
                string path = file.Path.Replace("//", "/");

                if (file.Type == "Folder")
                {

                    DirectoryInfo dir = new DirectoryInfo(@path);
                    DirectorySecurity dSecurity = dir.GetAccessControl(AccessControlSections.All);

                    var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, null);

                    if (!per.Read)
                        return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

                }
                else
                {
                    FileInfo dir = new FileInfo(@path);
                    FileSecurity fSecurity = dir.GetAccessControl(AccessControlSections.All);

                    var per = Helper.GetDirectoryOrFileUserPermission(aDUser, null, fSecurity, null);

                    if (!per.Read)
                        return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

                }
            }

            string proccessId = TransactionDate.Ticks.ToString();
            Helper.PermissionThreads.Add(proccessId.ToString(), (0, 0));

            //  string destination = RequestedData.Destination.Path.Replace("//", "/");
            foreach (var file in RequestedData.Sources)
            {
                Helper.Start(proccessId);
                Thread th = new Thread(() =>
                {
                    string path = file.Path.Replace("//", "/");
                    if (file.Type == "Folder")
                    {
                        DirectoryInfo source = new DirectoryInfo(@path);
                        string nPath = Path.Combine(DestinationPath, file.Name);
                        var dirToCreate = file.Name;
                        if (Directory.Exists(@nPath))
                        {
                            var count = source.Parent.GetDirectories().Where(x => x.Name.StartsWith(dirToCreate)).Count() + 1;

                            var filePathT = Path.Combine(DestinationPath, dirToCreate + " - Copy(" + count.ToString() + ")");
                            if (Directory.Exists(@filePathT))
                            {
                                dirToCreate += " - Copy(" + TransactionDate.Millisecond.ToString() + ")";
                            }
                            else
                                dirToCreate += " - Copy(" + count.ToString() + ")";

                        }

                        var folderPath = Path.Combine(DestinationPath, dirToCreate);
                        Directory.CreateDirectory(folderPath);

                        source.DeepCopy(folderPath, aDUser);
                    }
                    else
                    {
                        FileInfo source = new FileInfo(@path);

                        string fileName = file.Name;
                        string nPath = Path.Combine(DestinationPath, file.Name);

                        if (File.Exists(@nPath))
                        {
                            var fname = file.Name.Split(".");
                            var count = source.Directory.GetFiles().Where(x => x.Name.StartsWith(fname[0])).Count() + 1;

                            fileName = fname[0] + " - Copy(" + count.ToString() + ")." + fname[1];
                            var filePathT = Path.Combine(DestinationPath, fileName);
                            if (File.Exists(@filePathT))
                            {
                                fileName = fname[0] + " - Copy(" + TransactionDate.Millisecond.ToString() + ")." + fname[1];
                            }
                        }

                        var filePath = Path.Combine(DestinationPath, fileName);
                        source.CopyTo(filePath);
                    }
                    Helper.Finish(proccessId);
                });
                th.Start();
                Thread.Sleep(500);
            }

            int c = 0; int f = 0;
            (c, f) = Helper.PermissionThreads[proccessId];

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKey.ProccessId, proccessId),
                       new OutputDictionary(OperationOutputKey.StartCount, c),
                       new OutputDictionary(OperationOutputKey.FinishCount, f));

        }
        public OperationOutput MoveFileOrDirectory(FilesSharingInfo RequestedData)
        {
            OperationOutput Result = new OperationOutput();

            Models.User UserDbItem;

            if (RequestOwner.Id == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);

            ADUser aDUser = getAdUser();

            if (aDUser == null)
                return GetOperationOutput(header: Enums.ServiceMessages.UserNotExist);


            if (aDUser.IsLockout)
                return GetOperationOutput(header: Enums.ServiceMessages.UserIsBlocked);


            string DestinationPath = RequestedData.Destination.Path.Replace("//", "/");

            DirectoryInfo Destination = new DirectoryInfo(@DestinationPath);
            DirectorySecurity destinationSecurity = Destination.GetAccessControl(AccessControlSections.All);

            var DestinationPer = Helper.GetDirectoryOrFileUserPermission(aDUser, destinationSecurity, null, null);

            if (!DestinationPer.FullControl)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


            foreach (var file in RequestedData.Sources)
            {
                string path = file.Path.Replace("//", "/");
                string nPath = Path.Combine(DestinationPath, file.Name);

                if (ClearSpecial(nPath).StartsWith(ClearSpecial(path)))
                {
                    Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionErorr);
                    Result.Header.Message = "المجلد الوجهة موجود في المجلد المصدر ..!";
                    Result.Header.MessageEn = "Source Folder Contain Destination Folder ..!";
                    return Result;
                }
                if (file.Type == "Folder")
                {

                    DirectoryInfo dir = new DirectoryInfo(@path);
                    DirectorySecurity dSecurity = dir.GetAccessControl(AccessControlSections.All);

                    var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, null);

                    if (!per.FullControl)
                    {
                        Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.NoPermission);
                        return Result;
                    }
                }
                else
                {
                    FileInfo dir = new FileInfo(@path);
                    FileSecurity fSecurity = dir.GetAccessControl(AccessControlSections.All);

                    var per = Helper.GetDirectoryOrFileUserPermission(aDUser, null, fSecurity, null);

                    if (!per.FullControl)
                        return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

                }
            }

            string proccessId = TransactionDate.Ticks.ToString();
            Helper.PermissionThreads.Add(proccessId.ToString(), (0, 0));

            foreach (var file in RequestedData.Sources)
            {
                Helper.Start(proccessId);
                Thread th = new Thread(() =>
                {
                    string path = file.Path.Replace("//", "/");
                    string nPath = Path.Combine(DestinationPath, file.Name);

                    if (ClearSpecial(path) != ClearSpecial(nPath))
                    {
                        if (file.Type == "Folder")
                        {
                            DirectoryInfo source = new DirectoryInfo(@path);
                            //  var folderPath = Path.Combine(DestinationPath, !Directory.Exists(@nPath) ? file.Name : file.Name+" - Copy("+ TransactionDate.Millisecond.ToString()+")");

                            DirectorySecurity ndSecurity = source.GetAccessControl(AccessControlSections.All);

                            if (!Directory.Exists(@nPath))
                                Directory.CreateDirectory(nPath);

                            DirectoryInfo ndir = new DirectoryInfo(@nPath);
                            ndir.SetAccessControl(ndSecurity);
                            source.DeepMove(nPath);
                        }
                        else
                        {
                            FileInfo source = new FileInfo(@path);
                            source.MoveTo(nPath, true);
                        }
                    }
                    Helper.Finish(proccessId);
                });
                th.Start();
                Thread.Sleep(500);
            }

            int c = 0; int f = 0;
            (c, f) = Helper.PermissionThreads[proccessId];

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKey.ProccessId, proccessId),
                       new OutputDictionary(OperationOutputKey.StartCount, c),
                       new OutputDictionary(OperationOutputKey.FinishCount, f));

        }
        public OperationOutput GetProccessStatus(FileSharingInfo RequestedData)
        {
            if (!Helper.PermissionThreads.Keys.Contains(RequestedData.ProccessId))
            {
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                           new OutputDictionary(OperationOutputKey.StartCount, 1),
                           new OutputDictionary(OperationOutputKey.FinishCount, 1));
            }

            int c = 0; int f = 0;
            (c, f) = Helper.PermissionThreads[RequestedData.ProccessId];

            if (c == f && c != 0)
            {
                Helper.PermissionThreads.Remove(RequestedData.ProccessId);
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKey.StartCount, c),
                       new OutputDictionary(OperationOutputKey.FinishCount, f));
        }
        public OperationOutput CompressFiles(FilesSharingInfo RequestedData)
        {
            if (RequestOwner.Id == null)
                return null;

            ADUser aDUser = getAdUser();

            if (aDUser == null)
                return null;

            if (aDUser.IsLockout)
                return null;

            string proccessId = TransactionDate.Ticks.ToString();
            Helper.PermissionThreads.Add(proccessId.ToString(), (RequestedData.Sources.Count, 0));
            string zipFileName = ClearSpecial(RequestedData.Destination.Name);
            if (string.IsNullOrEmpty(zipFileName))
                zipFileName = TransactionDate.Millisecond.ToString();
            zipFileName += ".zip";

            if (File.Exists(Path.Combine(RequestedData.Destination.Path, zipFileName)))
                zipFileName = TransactionDate.Millisecond.ToString() + zipFileName;
            // DirectoryInfo parent = new DirectoryInfo(@RequestedData[0].Path.Replace("//", "/")).Parent;
            // Helper.Start(proccessId);
            Thread th = new Thread(() =>
        {
            using (MemoryStream compressedStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(compressedStream, ZipArchiveMode.Create, true))
                {
                    foreach (var dir in RequestedData.Sources)
                    {

                        string path = dir.Path.Replace("//", "/");

                        if (dir.Type == "Folder")
                        {
                            // DirectoryInfo source = new DirectoryInfo(@path);
                            AddFolderToZip(archive, path, dir.Name, aDUser);
                        }
                        else
                        {
                            FileInfo fileZ = new FileInfo(path);
                            FileSecurity fileSecurity = fileZ.GetAccessControl(AccessControlSections.All);
                            var perF = Helper.GetDirectoryOrFileUserPermission(aDUser, null, fileSecurity, null);
                            if (perF.Read)
                            {
                                string entryName = Path.GetFileName(path);
                                var entry = archive.CreateEntry(entryName);
                                using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                                {
                                    using (var entryStream = entry.Open())
                                    {
                                        fileStream.CopyTo(entryStream);
                                    }
                                }
                            }
                        }

                        if (dir != RequestedData.Sources.First())
                        {
                            Helper.Finish(proccessId);
                        }

                    }
                }

                byte[] compressedBytes = compressedStream.ToArray();
                File.WriteAllBytes(Path.Combine(RequestedData.Destination.Path, zipFileName), compressedBytes);

            }
            Helper.Finish(proccessId);
        });
            th.Start();
            Thread.Sleep(500);


            int c = 0; int f = 0;
            (c, f) = Helper.PermissionThreads[proccessId];

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKey.ProccessId, proccessId),
                       new OutputDictionary(OperationOutputKey.StartCount, c),
                       new OutputDictionary(OperationOutputKey.FinishCount, f));

        }
        private void AddFolderToZip(ZipArchive archive, string folderPath, string parentFolder, ADUser aDUser)
        {
            DirectoryInfo source = new DirectoryInfo(@folderPath);
            DirectorySecurity dSecurity = source.GetAccessControl(AccessControlSections.All);
            var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, null);

            if (per.Read)
            {

                foreach (var filePath in Directory.GetFiles(folderPath))
                {
                    var entryPath = Path.Combine(parentFolder, Path.GetFileName(filePath));
                    var entry = archive.CreateEntry(entryPath);
                    using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    using (var entryStream = entry.Open())
                    {
                        fileStream.CopyTo(entryStream);
                    }
                }
            }

            // Recursively add subfolders
            foreach (var subfolder in Directory.GetDirectories(folderPath))
            {
                AddFolderToZip(archive, subfolder, Path.Combine(parentFolder, Path.GetFileName(subfolder)), aDUser);
            }
        }


        #region Utilies


        private async Task<long> SaveFileAsync(string savePath, FileMultipartSection fileSection, IList<string> filePaths, IList<string> notUploadedFiles)
        {
            var extension = Path.GetExtension(fileSection.FileName);

            //   var FileName = Files.RemoveSpecialChars(fileSection.FileName.Replace(extension, string.Empty)) + extension;
            var FileName = ClearSpecial(fileSection.FileName.Replace(extension, string.Empty)) + extension;


            if (notAllowedExtensions.Contains(extension))
            {
                notUploadedFiles.Add(FileName);
                return 0;
            }

            string nPath = Path.Combine(savePath, FileName);

            var filePath = Path.Combine(savePath, !File.Exists(@nPath) ? FileName : TransactionDate.Millisecond.ToString() + "_" + FileName);

            await using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None, 1024);
            await fileSection.FileStream?.CopyToAsync(stream);

            filePaths.Add(filePath);

            return fileSection.FileStream.Length;
        }


        private string ConvertSizeToString(long bytes)
        {
            var fileSize = new decimal(bytes);
            var kilobyte = new decimal(1024);
            var megabyte = new decimal(1024 * 1024);
            var gigabyte = new decimal(1024 * 1024 * 1024);

            return fileSize switch
            {
                _ when fileSize < kilobyte => "Less then 1KB",
                _ when fileSize < megabyte =>
                    $"{Math.Round(fileSize / kilobyte, fileSize < 10 * kilobyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}KB",
                _ when fileSize < gigabyte =>
                    $"{Math.Round(fileSize / megabyte, fileSize < 10 * megabyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}MB",
                _ when fileSize >= gigabyte =>
                    $"{Math.Round(fileSize / gigabyte, fileSize < 10 * gigabyte ? 2 : 1, MidpointRounding.AwayFromZero):##,###.##}GB",
                _ => "n/a"
            };
        }

        private string GetBoundary(MediaTypeHeaderValue contentType)
        {
            var boundary = HeaderUtilities.RemoveQuotes(contentType.Boundary).Value;

            if (string.IsNullOrWhiteSpace(boundary))
            {
                throw new InvalidDataException("Missing content-type boundary.");
            }

            return boundary;
        }
        public ADUser GetADUserByUserPrincipalName(string UserPrincipalName)
        {
            PrincipalContext context = new PrincipalContext(ContextType.Domain, DomainName);
            UserPrincipal principal = new UserPrincipal(context);
            principal.UserPrincipalName = UserPrincipalName;
            //  principal.UserPrincipalName = "*@*";

            // principal.Enabled = true;
            PrincipalSearcher searcher = new PrincipalSearcher(principal);

            var user = searcher.FindAll().AsQueryable().Cast<UserPrincipal>().Select(x => new ADUser
            {
                Name = x.Name,
                UserPrincipalName = x.UserPrincipalName,
                Guid = x.Guid.Value,
                Sid = x.Sid.Value,
                SamAccountName = x.SamAccountName,
                IsLockout = x.IsAccountLockedOut(),

                Groups = x.GetGroups().Select(g => new ADGroup
                {
                    Name = g.Name,
                    Guid = g.Guid,
                    Sid = g.Sid.Value,
                    SamAccountName = g.SamAccountName
                }).ToList()
            }).FirstOrDefault();

            return user;

        }
        public ADUser GetADUser(string userName)
        {
            //WindowsIdentity identity = new WindowsIdentity(userName);
            //ADUser user = new ADUser();
            //user.Name = identity.Name;

            PrincipalContext context = new PrincipalContext(ContextType.Domain, DomainName);
            UserPrincipal principal = new UserPrincipal(context);
            principal.UserPrincipalName = userName + "@*";
            //  principal.UserPrincipalName = "*@*";

            // principal.Enabled = true;
            PrincipalSearcher searcher = new PrincipalSearcher(principal);

            var user = searcher.FindAll().AsQueryable().Cast<UserPrincipal>().Select(x => new ADUser
            {
                Name = x.Name,
                UserPrincipalName = x.UserPrincipalName,
                Guid = x.Guid.Value,
                Sid = x.Sid.Value,
                SamAccountName = x.SamAccountName,
                IsLockout = x.IsAccountLockedOut(),

                Groups = x.GetGroups().Select(g => new ADGroup
                {
                    Name = g.Name,
                    Guid = g.Guid,
                    Sid = g.Sid.Value,
                    SamAccountName = g.SamAccountName
                }).ToList()
            }).FirstOrDefault();


            // check if user is administrator
            if (user == null)
            {
                principal = new UserPrincipal(context);
                principal.SamAccountName = userName;

                searcher = new PrincipalSearcher(principal);

                user = searcher.FindAll().AsQueryable().Cast<UserPrincipal>().Select(x => new ADUser
                {
                    Name = x.Name,
                    UserPrincipalName = x.UserPrincipalName,
                    Guid = x.Guid.Value,
                    Sid = x.Sid.Value,
                    SamAccountName = x.SamAccountName,
                    IsLockout = x.IsAccountLockedOut(),

                    Groups = x.GetGroups().Select(g => new ADGroup
                    {
                        Name = g.Name,
                        Guid = g.Guid,
                        Sid = g.Sid.Value,
                        SamAccountName = g.SamAccountName
                    }).ToList()
                }).FirstOrDefault();
            }
            return user;


        }


        public void SetOwner(string Path, string Name)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.Start();
            cmd.StandardInput.WriteLine($"icacls {Path} /setowner {Name}");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            string output = cmd.StandardOutput.ReadToEnd();

            cmd.WaitForExit();
            cmd.Close();
        }

        public List<string> GetRoots()
        {

            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            cmd.Start();
            cmd.StandardInput.WriteLine($"net view {SharedFolderIP}");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();

            string output = cmd.StandardOutput.ReadToEnd();

            cmd.WaitForExit();
            cmd.Close();

            List<string> dirlist = new List<string>();
            if (output.Contains("Disk"))
            {
                int firstindex = output.LastIndexOf("--") + 2;
                int lastindex = output.LastIndexOf("Disk");
                string substring = output.Substring(firstindex, lastindex - firstindex).Replace("Disk", string.Empty).Trim();

                dirlist = substring.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                //  .Select(x => System.IO.Path.Combine(SharedFolderIP, x)).ToList();
            }

            return dirlist;
        }

        public bool IsListIncluded(List<string> Members, List<string> Users)
        {
            foreach (var str in Members)
                if (!Users.Contains(str))
                    return false;
            return true;
        }
        public bool IsUserLockedOut(string userName)
        {
            using (var ctx = new PrincipalContext(ContextType.Domain, DomainName))
            {
                var user = UserPrincipal.FindByIdentity(ctx, userName);
                if (user != null)
                    return !user.IsAccountLockedOut();
            }

            return false;

        }

        public FileSystemAccessRule GetAccessRole(string ntAccount, DirectorySecurity aclD = null, FileSecurity aclF = null)
        {
            var acc = new NTAccount(DomainName, ntAccount);

            AuthorizationRuleCollection rules;
            if (aclD != null)
                rules = aclD.GetAccessRules(true, true, typeof(SecurityIdentifier));
            else
                rules = aclF.GetAccessRules(true, true, typeof(SecurityIdentifier));


            if (rules == null) return null;

            foreach (FileSystemAccessRule rule in rules)
            {
                if (rule.IdentityReference.Value == acc.Value)
                    return rule;
            }

            return null;

        }

        public List<DirFileAccessRule> GetDirFileDomainAccessRules(ADUser aDUser, string Path, string type)
        {
            AuthorizationRuleCollection rules;
            List<DirFileAccessRule> dirFileAccessRule = new List<DirFileAccessRule>();
            if (Path != null)
            {
                string path = Path.Replace("//", "/");
                if (type == "Folder")
                {
                    DirectoryInfo dInfo = new DirectoryInfo(@path);
                    DirectorySecurity dSecurity = dInfo.GetAccessControl(AccessControlSections.All);
                    rules = dSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));
                }
                else
                {
                    FileInfo file = new FileInfo(@path);
                    FileSecurity fileSecurity = file.GetAccessControl(AccessControlSections.All);
                    rules = fileSecurity.GetAccessRules(true, true, typeof(SecurityIdentifier));
                }
                //  WindowsIdentity identity = WindowsIdentity.GetCurrent();


                foreach (FileSystemAccessRule rule in rules)
                {
                    if (!knownSIDs.Values.Contains(rule.IdentityReference.Value))
                    {

                        string strNTAccount;
                        ADUser user;
                        string[] DName;
                        try
                        {

                            if (Helper.NTAccounts.ContainsKey(rule.IdentityReference.Value))
                            {
                                strNTAccount = Helper.NTAccounts[rule.IdentityReference.Value];
                            }
                            else
                            {
                                strNTAccount = new SecurityIdentifier(rule.IdentityReference.Value).Translate(typeof(NTAccount)).ToString();
                                Helper.NTAccounts.Add(rule.IdentityReference.Value, strNTAccount);
                            }

                            DName = strNTAccount.Split("\\");

                            if (Helper.ADUsers.ContainsKey(rule.IdentityReference.Value))
                            {
                                user = Helper.ADUsers[rule.IdentityReference.Value];
                            }
                            else
                            {
                                user = GetADUser(DName[1] ?? strNTAccount);
                                if (user != null)
                                {
                                    Helper.ADUsers.Add(rule.IdentityReference.Value, user);
                                }
                            }


                            if (!IsAdmin(user != null ? user.SamAccountName : DName[1] != null ? DName[1] : strNTAccount, path) || IsAdmin(aDUser.SamAccountName, path))
                            {
                                if (!IsSuperAdmin(user != null ? user.SamAccountName : DName[1] != null ? DName[1] : strNTAccount) || IsSuperAdmin(aDUser.SamAccountName))
                                {
                                    DirFileAccessRule dirFileAccess = new DirFileAccessRule();
                                    //    dirFileAccess.NTAccount = rule.IdentityReference.Value;
                                    dirFileAccess.Name = rule.IdentityReference.Value == everyone.Value ? "Everyone" : user != null ? user.Name : DName[1] != null ? DName[1] : strNTAccount;
                                    dirFileAccess.UserPrincipalName = user != null ? user.UserPrincipalName : rule.IdentityReference.Value == everyone.Value ? "Everyone" : strNTAccount;
                                    dirFileAccess.Type = user != null ? "User" : "Group";
                                    dirFileAccess.Sid = rule.IdentityReference.Value;
                                    dirFileAccess.Permission = new DirFilePermission();
                                    dirFileAccess.Permission.Read = (FileSystemRights.Read & rule.FileSystemRights) == FileSystemRights.Read;
                                    dirFileAccess.Permission.Write = (FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write;
                                    dirFileAccess.Permission.ReadAndExecute = (FileSystemRights.ReadAndExecute & rule.FileSystemRights) == FileSystemRights.ReadAndExecute;
                                    dirFileAccess.Permission.FullControl = (FileSystemRights.FullControl & rule.FileSystemRights) == FileSystemRights.FullControl;
                                    dirFileAccess.Permission.Modify = (FileSystemRights.Modify & rule.FileSystemRights) == FileSystemRights.Modify;

                                    dirFileAccess.Permission.Type = rule.AccessControlType == AccessControlType.Allow ? (int)AccessControlType.Allow : (int)AccessControlType.Deny;
                                    dirFileAccess.Permission.IsInherit = rule.IsInherited;
                                    dirFileAccess.Permission.IsOwner = rule.IdentityReference.Value == aDUser.Sid;

                                    dirFileAccess.ApplyTo = rule.InheritanceFlags == InheritanceFlags.None ? 1 : rule.InheritanceFlags == InheritanceFlags.ContainerInherit ? 2 : 3;
                                    dirFileAccessRule.Add(dirFileAccess);
                                }
                            }
                        }
                        catch { }

                    }
                }
            }

            return dirFileAccessRule;

        }


        public bool HasSubWithReadPermission(ADUser aDUser, DirectoryInfo dInfo)
        {
            var subDir = dInfo.GetDirectories("*", SearchOption.AllDirectories);
            foreach (var dir in subDir)
            {
                DirectorySecurity dSecurity = dir.GetAccessControl(AccessControlSections.All);
                var per = Helper.GetDirectoryOrFileUserPermission(aDUser, dSecurity, null, null);

                if (per.Read)
                {
                    return true;
                }

                foreach (FileInfo file in dir.GetFiles())
                {
                    FileSecurity fileSecurity = file.GetAccessControl(AccessControlSections.All);
                    var perF = Helper.GetDirectoryOrFileUserPermission(aDUser, null, fileSecurity, null);
                    if (perF.Read)
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        public string GetWebDavUrl(string fileType, bool editPer, string path)
        {
            string url = "";
            if (fileType == ".doc" || fileType == ".docx")
                url = editPer ? WebDavPath.WordEdit : WebDavPath.WordRead;
            else if (fileType == ".xls" || fileType == ".xlsx")
                url = editPer ? WebDavPath.ExcelEdit : WebDavPath.ExcelRead;
            else if (fileType == ".ppt" || fileType == ".pptx")
                url = editPer ? WebDavPath.PowerPointEdit : WebDavPath.PowerPointRead;
            else if (fileType == ".vsd" || fileType == ".vsdx")
                url = editPer ? WebDavPath.MsVisioEdit : WebDavPath.MsVisioRead;
            else if (fileType == ".mpp")
                url = editPer ? WebDavPath.MsProjectEdit : WebDavPath.MsProjectRead;
            else
                url = "";

            return url + FileDomainName + path.Replace(PhysicalPath, "").Replace("\\", "/");
        }

        public string GetSizeWithUnit(long size)
        {
            if (size >= 1000000000)
                return (size / 1000000000).ToString() + " GB";
            else if (size >= 1000000)
                return (size / 1000000).ToString() + " MB";
            else if (size >= 1000)
                return (size / 1000).ToString() + " KB";
            else
                return size.ToString() + " Byte";

        }



        public DirFilePermission GetDirectoryOrFileGroupPermission(string aDGroup, DirectorySecurity aclD = null, FileSecurity aclF = null)
        {

            DirFilePermission dirFilePermission = new DirFilePermission();
            AuthorizationRuleCollection rules;

            if (aclD != null)
                rules = aclD.GetAccessRules(true, true, typeof(NTAccount));
            else
                rules = aclF.GetAccessRules(true, true, typeof(NTAccount));


            if (rules == null) return dirFilePermission;

            AuthorizationRuleCollection gRole = new AuthorizationRuleCollection();

            foreach (AuthorizationRule rule in rules)
            {
                var DRole = rule.IdentityReference.Value.Split("\\");
                var rName = rule.IdentityReference.Value;
                if (DRole.Count() > 1)
                    rName = DRole[0];

                if (aDGroup == rName || aDGroup == rule.IdentityReference.Value)
                    gRole.AddRule(rule);
            }

            foreach (FileSystemAccessRule rule in gRole)
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

                    dirFilePermission.Type = (int)rule.AccessControlType;
                    dirFilePermission.IsInherit = rule.IsInherited;


                }
            }

            return dirFilePermission;
        }

        public bool IsAdmin(string UserName, string path)
        {
            if (SharedFoldersPaths.SuperAdminsUsersList.Contains(UserName))
                return true;

            if (!string.IsNullOrEmpty(path))
            {
                if (SharedFoldersPaths.FoldersList.Where(x => ClearSpecial(path).Contains(ClearSpecial(x.FolderPath))).FirstOrDefault().AdminsUsersList.Contains(UserName))
                    return true;
            }

            var adUser = GetADUser(UserName);

            if (adUser != null && adUser.Groups.Any() && SharedFoldersPaths.SuperAdminsGroupsList.Any())
                foreach (var group in adUser.Groups)
                {
                    if (SharedFoldersPaths.SuperAdminsGroupsList.Contains(group.SamAccountName))
                        return true;
                }

            if (!string.IsNullOrEmpty(path))
            {
                var adminsGroup = SharedFoldersPaths.FoldersList.Where(x => ClearSpecial(path).Contains(ClearSpecial(x.FolderPath))).FirstOrDefault().AdminsGroupsList;
                if (adUser != null && adUser.Groups.Any() && adminsGroup.Any())
                    foreach (var group in adUser.Groups)
                    {
                        if (adminsGroup.Contains(group.SamAccountName))
                            return true;
                    }
            }

            return false;
        }

        public bool IsAdminsGroup(string GroupName, string path)
        {
            if (SharedFoldersPaths.SuperAdminsGroupsList.Contains(GroupName))
                return true;

            if (!string.IsNullOrEmpty(path))
            {
                return SharedFoldersPaths.FoldersList.Where(x => ClearSpecial(path).Contains(ClearSpecial(x.FolderPath))).FirstOrDefault().AdminsGroupsList.Contains(GroupName);
            }

            return false;
        }

        public bool IsSuperAdmin(string UserName)
        {
            if (SharedFoldersPaths.SuperAdminsUsersList.Contains(UserName))
                return true;

            var adUser = GetADUser(UserName);

            if (adUser != null && adUser.Groups.Any() && SharedFoldersPaths.SuperAdminsGroupsList.Any())
                foreach (var group in adUser.Groups)
                {
                    if (SharedFoldersPaths.SuperAdminsGroupsList.Contains(group.SamAccountName))
                        return true;
                }

            return false;
        }

        public bool IsSuperAdminsGroup(string GroupName)
        {
            if (SharedFoldersPaths.SuperAdminsGroupsList.Contains(GroupName))
                return true;

            return false;
        }

        public string ClearSpecial(string str)
        {
            return str.Trim().Replace("\\", "").Replace("/", "").Replace(".", "").Replace("$", "").ToLower();
        }

        private ADUser getAdUser()
        {
            ADUser aDUser;
            Models.User UserDbItem;
            if (string.IsNullOrEmpty(RequestOwner.UserName))
            {
                UserDbItem = _unitOfWork.Users.GetAll().Where(x => x.Id == RequestOwner.Id).FirstOrDefault();
                if (UserDbItem == null)
                    return null;

                aDUser = GetADUser(UserDbItem.UserName);
            }
            else
            {
                aDUser = GetADUser(RequestOwner.UserName);
            }

            return aDUser;
        }

        #endregion


    }
}
