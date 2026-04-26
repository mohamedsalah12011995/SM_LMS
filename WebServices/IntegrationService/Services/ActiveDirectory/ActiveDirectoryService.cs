
using IntegrationService.Records.AD;
using System.DirectoryServices.AccountManagement;

namespace IntegrationService.Services
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {
        public string DomainName;
        public IConfiguration _configuration;
        public ActiveDirectoryService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            DomainName = _configuration.GetSection("AppSettings").GetSection("DomainName").Value;
        }

        public async Task<UserAD> GetADUserInfo(GetADUserRecord userInfo)
        {
            var user = new UserAD();
            PrincipalContext context = new PrincipalContext(ContextType.Domain, DomainName);
            UserPrincipal principal = new UserPrincipal(context);

            if(string.IsNullOrEmpty(userInfo.UserName) && string.IsNullOrEmpty(userInfo.DesiredUserName))
                return new UserAD() { Header = new Header() { IsSuccess = false, Message = "Please Insert UserName", Code = 200 } };

            principal.UserPrincipalName = string.IsNullOrEmpty(userInfo.UserName) ? userInfo.UserName + "@*" : userInfo.DesiredUserName + "@*";
            
            PrincipalSearcher searcher = new PrincipalSearcher(principal);

            user = searcher.FindAll().AsQueryable().Cast<UserPrincipal>().Select(x => new UserAD
            {
                Header = new Header() { IsSuccess = true, Message = "Success", Code = 200 },
                Body = new Records.AD.Body
                {
                    UserInformation = new UserInformation()
                    {
                        FullName = x.DisplayName,
                        Name = x.Name,
                        Email = x.EmailAddress,
                        IdentityNumber = x.EmployeeId,
                        MobileNumber = x.VoiceTelephoneNumber,
                        UnitName = x.UserPrincipalName,
                        UserName = userInfo.UserName,
                        SamAccountName = x.SamAccountName,
                        IsLockout = x.IsAccountLockedOut(),
                        Sid = x.Sid.Value,
                    }
                }
            }).FirstOrDefault();

            // check if user is administrator
            if (user == null)
            {
                principal = new UserPrincipal(context);
                principal.SamAccountName = userInfo.UserName;

                searcher = new PrincipalSearcher(principal);

                user = searcher.FindAll().AsQueryable().Cast<UserPrincipal>().Select(x => new UserAD
                {
                    Header = new Header() { IsSuccess = true, Message = "Success", Code = 200 },
                    Body = new Records.AD.Body
                    {
                        UserInformation = new UserInformation()
                        {
                            FullName = x.DisplayName,
                            Name = x.Name,
                            Email = x.EmailAddress,
                            IdentityNumber = x.EmployeeId,
                            MobileNumber = x.VoiceTelephoneNumber,
                            UnitName = x.UserPrincipalName,
                            UserName = userInfo.UserName,
                            SamAccountName = x.SamAccountName,
                            IsLockout = x.IsAccountLockedOut(),
                            Sid = x.Sid.Value,
                        }
                    }
                }).FirstOrDefault();

                if (user == null)
                    return new UserAD { Header = new Header() { IsSuccess = false, Message = "Please Insert UserName", Code = 200 } };
            }

            return user;
        }

        public async Task<UserAD> UserLogin(GetADUserRecord user)
        {
            var userAD = new UserAD();
            try
            {
                userAD = await GetADUserInfo(user);
                if (userAD != null)
                {
                    using (var pc = new PrincipalContext(ContextType.Domain, DomainName))
                    {
                        userAD.Body.LoginSuccess = pc.ValidateCredentials(user.UserName, user.Password);
                    }

                    return userAD;
                }
                else
                {
                    return new UserAD
                    {
                        Header = new Header() { IsSuccess = false, Message = "Wrong UserName Or Password", Code = 200 },
                        Body = new Body() { LoginSuccess = false }
                    };
                }
            }
            catch (Exception ex)
            {
               return new UserAD
               {
                   Header = new Header() { IsSuccess = false, Message = ex.Message, Code = 200 },
                   Body = new Body() { LoginSuccess = false }
               };
            }
        }
    }
}
