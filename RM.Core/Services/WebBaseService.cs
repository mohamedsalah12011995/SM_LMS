using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RM.Core.Helpers;

namespace RM.Core.Services
{
    public class WebBaseService
    {
        protected DateTime TransactionDate;
        protected JWTHelper.Users RequestOwner;
        protected IConfiguration _configuration;
        protected IHttpContextAccessor _httpContextAccessor;
        protected Enums.UsersRoles RequestUserRole;
        protected bool IsLocal;
        protected bool IsPortal;
        protected string Token = string.Empty;


        public WebBaseService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            TransactionDate = DateTime.Now;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            SetRequestOwner();

            RequestUserRole = RequestOwner != null && RequestOwner.RoleId.HasValue ? (Enums.UsersRoles)RequestOwner.RoleId.Value : Enums.UsersRoles.NormalUser;
            this.IsLocal = Convert.ToBoolean(_httpContextAccessor?.HttpContext?.Request.Headers["isLocal"].FirstOrDefault());
            this.IsPortal = Convert.ToBoolean(_httpContextAccessor?.HttpContext?.Request.Headers["isPortal"].FirstOrDefault());
        }
        private void SetRequestOwner()
        {
            Token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"]!;
            RequestOwner = JWTHelper.DecryptToken(Token);
        }
    }
}
