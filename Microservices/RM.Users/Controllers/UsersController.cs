


using Microsoft.AspNetCore.Mvc;
using RM.Users.Services;
using RM.Users.UnitOfWorks;
using RM.Users.Dtos;
using RM.Core.Extensions;
using RM.Core.Helpers;
using static RM.Users.Dtos.OperationOutput;
using RM.Users.Records;
using Mapster;

namespace RM.Users.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController 
    {

        private readonly IUsersService _usersService;
        private readonly IEmployeeService _employeeService;
        private readonly IUnitOfWork _unitOfWork;
        private OperationOutput ResultToken = new OperationOutput();
        private readonly ILogger<UsersController> _logger;
        string Token;
        private string IamUlr;
        private string IamServiceUrl;
        string DomainName;
        private IHttpContextAccessor _httpContextAccessor;


        public UsersController(IHttpContextAccessor httpContextAccessor, ILogger<UsersController> logger, IUsersService usersService,
                               IEmployeeService employeeService, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _usersService = usersService;
            _employeeService = employeeService;

            _logger.LogInformation("new Tyring to launch user CTRl ");
            DomainName = _unitOfWork.Configuration.ReadConfigurationFromSection("DomainName");

            CheckUserLoginFromServiceOrUrl(unitOfWork);
        }

        #region HELPER METHOD >> CONSTRACTOR
        private void CheckUserLoginFromServiceOrUrl(IUnitOfWork unitOfWork)
        {
            try
            {
                Token = _httpContextAccessor.HttpContext!.Request.Headers["Authorization"]!;
                IamServiceUrl = _unitOfWork.Configuration.ReadConfigurationFromSection("IamServiceUrl");
                IamUlr = _unitOfWork.Configuration.ReadConfigurationFromSection("IamUlr");

            }
            catch (Exception ex)
            {
                _logger.LogError("{ExceptionMessage} | {ExceptionTrace}", ex.Message, ex.StackTrace);
            }

            _logger.LogInformation("success to launch user CTRl");
        }

        #endregion

        [HttpPost]
        public async Task<OperationOutput> SaveUser(SaveUserRecord RequestedData)
            => await _usersService.SaveUser(RequestedData.Adapt<Dtos.Users>());

        [HttpPost]
        public async Task<OperationOutput> UserLogin(UserLoginRecord RequestedData)
            => await _usersService.UserLogin(RequestedData.Adapt<Dtos.Users>());

        [HttpPost]
        public async Task<OperationOutput> UserLoginOTP(UserLoginRecord RequestedData)
            => await _usersService.UserLoginOTP(RequestedData.Adapt<Dtos.Users>());

        [HttpPost]
        public async Task<OperationOutput> CheckOTP(UserLoginRecord RequestedData)
            => await _usersService.CheckOTP(RequestedData.Adapt<Dtos.Users>());


        [HttpGet]
        public async Task<ActionResult> IAMLogin(string Source)
        {
            OperationOutput Result = new OperationOutput();

            bool IsCallback = false;
            string CallBackToken = string.Empty;
            string IamCallbackUrl = string.Empty;
            string RequestSource = string.Empty;
            string IdCard = _httpContextAccessor.HttpContext.Request.Query["IdNo"];
            string IdType = _httpContextAccessor.HttpContext.Request.Query["IdType"];
            string dobHijri = _httpContextAccessor.HttpContext.Request.Query["dobHijri"];
            string callbackinfo = _httpContextAccessor.HttpContext.Request.Query["callbackinfo"];
            IamCallbackUrl = _httpContextAccessor.HttpContext.Request.Query["url"];
            IsCallback = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(callbackinfo);

            RequestSource = _httpContextAccessor.HttpContext.Request.Query["Source"];

            if (IsCallback)
            {
                RequestSource = callbackinfo.Split('$')[0];
                IamCallbackUrl = callbackinfo.Split('$')[1].Split('?')[0];
                IdCard = callbackinfo.Split('$')[1].Split('?')[1].Replace("IdNo=", string.Empty);
            }
            try
            {

                //string CurrentUrl = "https://localhost:44389/gateway/users/IAMLogin";
                //if (Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(IdCard))
                if (!IsCallback)
                {
                    return new RedirectResult(IamUlr + "?lang=ar&RedirectURL=" + IamServiceUrl + "?callbackinfo=" + RequestSource + "$" + IamCallbackUrl);
                }
                Result = await _usersService.IAMLogin(IdCard, dobHijri, IdType, RequestSource);
                CallBackToken = Result.Header.Success ? (string)Result.Output[OperationOutputKeys.UserJWT] : "0";
            }
            catch (Exception ex)
            {
                _logger.LogError("{ExceptionMessage} | {ExceptionTrace}", ex.Message, ex.StackTrace);
                Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionErorr);
            }
            return new RedirectResult(IamCallbackUrl + "?token=" + CallBackToken);
        }


        [HttpPost]

        public async Task<OperationOutput> GetUserLookps()
           => await _usersService.GetUserLookps();


        [HttpPost]

        public async Task<OperationOutput> BlockActivateUser(ActivateUserRecord RequestedData)
              => await _usersService.UserAction(RequestedData.Adapt<Dtos.Users>());


        [HttpPost]

        public async Task<OperationOutput> DeleteUser(DeleteUserRecord RequestedData)
        => await _usersService.UserAction(RequestedData.Adapt<Dtos.Users>());


        [HttpPost]

        public OperationOutput GenerateToken()
        {
            try
            {
                ResultToken = _usersService.GenerateUserToken();
            }
            catch (Exception ex)
            {
                _logger.LogError("{ExceptionMessage} | {ExceptionTrace}", ex.Message, ex.StackTrace);
                ResultToken = _usersService.GenerateUserToken();
            }
            return ResultToken;
        }


        [HttpPost]

        public OperationOutput GenerateTokenWinAuth(GenerateTokenWinAuthRecord RequestedData)
        {
            try
            {
                ResultToken = _usersService.GenerateToken2(RequestedData.UserName);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ExceptionMessage} | {ExceptionTrace}", ex.Message, ex.StackTrace);
                ResultToken = _usersService.GenerateUserToken();

            }
            return ResultToken;
        }


        [HttpPost]

        public async Task<OperationOutput> GetUsersList(GetUsersListRecord RequestedData)
           => await _usersService.GetUsersList(RequestedData.Adapt<Dtos.Users>());


        [HttpPost]

        public async Task<OperationOutput> GetUserDetails(GetUserDetailsRecord RequestedData)
        => await _usersService.GetUserDetails(RequestedData.Adapt<Dtos.Users>());


        [HttpPost]

        public async Task<OperationOutput> GetTotalUserLookups()
        => await _usersService.GetTotalUserLookups();


        [HttpPost]

        public async Task<OperationOutput> GetUserInfoByToken()
            => await _usersService.GetUserInfoByToken(Token);


        [HttpPost]

        public async Task<OperationOutput> CompleteUserRegistration(CompleteUserRegistrationRecord RequestedData)
           => await _usersService.CompleteUserRegistration(RequestedData.Adapt<Dtos.Users>());


        [HttpPost]

        public async Task<int> CheckUserName(CheckUserNameRecord RequestedData)
        {
            return await _usersService.CheckUserName(RequestedData.UserName);
        }


        [HttpPost]

        public async Task<OperationOutput> CheckUserLoginFromActiveDirectory(CheckUserLoginFromActiveDirectoryRecord RequestedData)
            => await _usersService.CheckUserLoginFromActiveDirectory(RequestedData.Adapt<Dtos.Users>());


        [HttpPost]

        public async Task<OperationOutput> GetEmployeeInformation(GetEmployeeInformationRecord RequestedData)
            => await _employeeService.GetEmployeeInformation(RequestedData.Adapt<Dtos.Users>());

        [HttpPost]

        public async Task<OperationOutput> GetPayrollInformation(GetPayrollInformationRecord RequestedData)
            => await _employeeService.GetPayrollInformation(RequestedData.Adapt<Dtos.Users>());

        [HttpPost]

        public OperationOutput CheckUserLogged()
             => _usersService.CheckUserLogged();


        [HttpPost]

        public async Task<OperationOutput> ChangeUserReference(ChangeUserReferenceRecord RequestedData)
           => await _usersService.ChangeUserReference(RequestedData.Adapt<Dtos.Users>());


        [HttpPost]
        public async Task<OperationOutput> GetAllMajorsWithReferencesTree(GetAllMajorsWithReferencesTreeRecord RequestedData)
             => await _usersService.GetAllMajorsWithReferencesTree(RequestedData.Adapt<Dtos.ReferencesTree>());


        [HttpPost]

        public async Task<OperationOutput> RefreshToken(RefreshTokenRecord RequestedData)
            => await _usersService.RefreshToken(RequestedData.Adapt<Dtos.Users>());


    }
}