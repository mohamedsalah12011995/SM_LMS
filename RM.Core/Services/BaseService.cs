using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using RM.Core.Consts;
using RM.Core.Extensions;
using RM.Core.Helpers;
using System.Text.Json;

namespace RM.Core.Services
{
    public class BaseService : IBaseService
    {
        protected DateTime TransactionDate;
        protected JWTHelper.Users RequestOwner;
        protected Enums.UsersRoles RequestUserRole;
        protected string StatisticsServiceUrl;
        protected string EmailServiceUrl;
        protected string PDFServiceUrl;
        protected string GetDyFormPath;
        protected string UsersServiceUrl = string.Empty;
        protected string SysUserName = string.Empty;
        protected string SysUserPassword = string.Empty;

        protected string GetPath;
        protected string IntranetGetPath;
        protected string GetPathInnerGate;
        protected string IntranetGetPathInnerGate;

        protected string Token = string.Empty;
        protected IHttpContextAccessor _httpContextAccessor;
        protected IConfiguration _configuration;
        protected bool IsLocal;
        protected bool IsPortal;

        protected int DefaultPaginationCount;
        protected int FileSizeMb;

        protected string ImagesSavePath = string.Empty;
        protected string ThumbsSavePath = string.Empty;
        protected string ImagesGetPath = string.Empty;
        protected string ThumbsGetPath = string.Empty;
        protected string FilesSavePath = string.Empty;
        protected string FilesGetPath = string.Empty;
        protected string VideosGetPath = string.Empty;
        protected string VideosSavePath = string.Empty;
        protected string DocumentsSavePath = string.Empty;
        protected string DocumentsGetPath = string.Empty;
        protected string DocumentsGetPathInnerGate = string.Empty;

        protected string EditorsDocsSavePath = string.Empty;
        protected string EditorsDocsGetPath = string.Empty;
        protected string EditorsDocsGetPathInnerGate = string.Empty;
        protected string IdentityPhotoSavePath;
        protected string PersonalPhotoSavePath;
        protected string IdentityPhotoGetPath;
        protected string PersonalPhotoGetPath;
        protected string CapchaSecret;
        protected string FormSurveyUrl;
        protected bool UseCapcha;
        protected string DirPath { get; set; }

        protected DateTime StartWeekDate;
        protected DateTime EndWeekDate;
        public JsonSerializerOptions jsonOptions = null;
        public RedisConfiguration _RedisConfiguration { get; set; }
        public IDistributedCache RedisCache { get; private set; }

        public BaseService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, RedisConfiguration redisConfiguration = null, IDistributedCache redisCache = null)
        {
            TransactionDate = DateTime.Now;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _RedisConfiguration = redisConfiguration;
            RedisCache = redisCache;

            if (_httpContextAccessor.HttpContext != null)
                SetRequestOwner();

            RequestUserRole = RequestOwner != null && RequestOwner.RoleId.HasValue ? (Enums.UsersRoles)RequestOwner.RoleId.Value : Enums.UsersRoles.NormalUser;
            this.IsLocal = Convert.ToBoolean(_httpContextAccessor?.HttpContext?.Request.Headers["isLocal"].FirstOrDefault());
            this.IsPortal = Convert.ToBoolean(_httpContextAccessor?.HttpContext?.Request.Headers["isPortal"].FirstOrDefault());

            StatisticsServiceUrl = _configuration.ReadConfigurationFromSection("StatisticsServiceUrl");
            EmailServiceUrl = _configuration.ReadConfigurationFromSection("EmailServiceUrl");
            PDFServiceUrl = _configuration.ReadConfigurationFromSection("PDFServiceUrl");
            CapchaSecret = _configuration.ReadConfigurationFromSection("CapchaSecret");
            FormSurveyUrl = _configuration.ReadConfigurationFromSection("FormSurveyUrl");

            UsersServiceUrl = _configuration.ReadConfigurationFromSection("UsersServiceUrl");
            SysUserName = _configuration.ReadConfigurationFromSection("SysUserName");
            SysUserPassword = _configuration.ReadConfigurationFromSection("SysUserPassword");


            int.TryParse(_configuration.ReadConfigurationFromSection("DefaultPaginationCount"), out DefaultPaginationCount);
            int.TryParse(_configuration.ReadConfigurationFromSection("FileSizeMb"), out FileSizeMb);

            SetPathForFiles();
            StartWeekDate = TransactionDate.StartOfWeek(DayOfWeek.Saturday);
            EndWeekDate = StartWeekDate.AddDays(6);

            jsonOptions = new JsonSerializerOptions()
            {
                //  Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                // PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public void SetPathForFiles()
        {
            var path = new PhysicalFileProvider(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Resources")).Root;
            var SharedStorage = _configuration.ReadConfigurationFromSection("SharedStorage");
            if (!string.IsNullOrEmpty(SharedStorage))
            {
                path = SharedStorage;
            }

            ImagesSavePath = path + nameof(FilePathRootConst.images);
            ThumbsSavePath = path + nameof(FilePathRootConst.icons);
            EditorsDocsSavePath = DocumentsSavePath = FilesSavePath = path + nameof(FilePathRootConst.files);
            VideosSavePath = path + nameof(FilePathRootConst.videos);

            GetPath = _configuration.ReadConfigurationFromSection("GetPath");
            IntranetGetPath = _configuration.ReadConfigurationFromSection("IntranetGetPath");

            GetPathInnerGate = _configuration.ReadConfigurationFromSection("GetPathInnerGate");
            IntranetGetPathInnerGate = _configuration.ReadConfigurationFromSection("IntranetGetPathInnerGate");

            ImagesGetPath = ThumbsGetPath = FilesGetPath = VideosGetPath = DocumentsGetPath = EditorsDocsGetPath = Strings.HandleGetResourcesPath(IsLocal, GetPath, IntranetGetPath);
            DocumentsGetPathInnerGate = EditorsDocsGetPathInnerGate = Strings.HandleGetResourcesPath(IsLocal, GetPathInnerGate, IntranetGetPathInnerGate);

            IdentityPhotoGetPath = PersonalPhotoGetPath = Strings.HandleGetResourcesPath(IsLocal, GetPath, IntranetGetPath);

            PersonalPhotoSavePath = IdentityPhotoSavePath = path + nameof(FilePathRootConst.images);
            DocumentsSavePath = path + nameof(FilePathRootConst.files);
            DirPath = path;
        }

        protected string SetRequestOwner()
        {
            try
            {
                Token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"]!;

                var action = _httpContextAccessor?.HttpContext?.GetRouteValue("action").ToString().ToLower();
                if (action != ConstType.IAMLoginMethod.ToLower() && action != ConstType.GenerateTokenMethod.ToLower() &&
                    action != ConstType.GenerateTokenWinAuth.ToLower() && action != ConstType.ResouresMethod.ToLower())
                {
                    RequestOwner = JWTHelper.DecryptToken(Token);
                }
                else
                {
                    RequestOwner = new JWTHelper.Users();
                    RequestOwner.Name = "Anonymous";
                }

                bool.TryParse(_httpContextAccessor?.HttpContext.Request.Headers["UseCapcha"].FirstOrDefault(), out UseCapcha);
                return Token;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public ApplicationOperation.Pagination SetDefaultData()
        {
            return new ApplicationOperation.Pagination
            {
                RecordPerPage = DefaultPaginationCount,
                TotalPagesCount = 1,
                CurrentPageIndex = 1,
                TotalItemsCount = DefaultPaginationCount
            };
        }
        public FileStreamResult GetPathOfResource(string fileName)
        {
            var filePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")).Root;
            var sharedStorage = _configuration.ReadConfigurationFromSection("SharedStorage");

            if (!string.IsNullOrEmpty(sharedStorage))
                filePath = sharedStorage;

            if (File.Exists($"{filePath}{nameof(FilePathRootConst.images)}/{fileName}"))

                return Helpers.Helpers.GetResoures($"{filePath}{nameof(FilePathRootConst.images)}/", fileName);

            else if (File.Exists($"{filePath}{nameof(FilePathRootConst.icons)}/{fileName}"))

                return Helpers.Helpers.GetResoures($"{filePath}{nameof(FilePathRootConst.icons)}/", fileName);

            else if (File.Exists($"{filePath}{nameof(FilePathRootConst.files)}/{fileName}"))

                return Helpers.Helpers.GetResoures($"{filePath}{nameof(FilePathRootConst.files)}/", fileName);

            else if (File.Exists($"{filePath}{nameof(FilePathRootConst.videos)}/{fileName}"))

                return Helpers.Helpers.GetResoures($"{filePath}{nameof(FilePathRootConst.videos)}/", fileName);
            else
                return Helpers.Helpers.GetResoures($"{filePath}{nameof(FilePathRootConst.images)}/", "noImage.png");
        }
    }
}
