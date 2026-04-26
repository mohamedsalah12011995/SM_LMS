using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RM.Core.Helpers;
using RM.GateWay.Integrations;


namespace RM.GateWay.Controllers
{
    [EnableCors("CORSPolicy")]
    [Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
    [Route("[controller]/[action]")]
    [ApiController]

    public class GateWayController : IGateWayController
    {
        private readonly ILogger<GateWayController> _logger;
        private IHttpContextAccessor _httpContextAccessor;
        private OperationOutput ResultToken = new OperationOutput();
        private IConfiguration _configuration;
        public GateWayController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILogger<GateWayController> logger)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        [HttpPost]
        [Produces("application/json")]
        public async Task<OperationOutput> GenerateTokenWinAuthAsync()
        {
            try
            {
                var domain = _httpContextAccessor.HttpContext.User.Identity.Name.Split("\\");
                var url = _configuration.GetSection("AppSettings").GetSection("GenerateTokenWinAuthUrl").Value;

                ResultToken = await UserWinAuth.GenerateTokenWinAuth(url, domain[1]);
                ResultToken.Output.Add("IsAuthenticated", _httpContextAccessor.HttpContext.User?.Identity?.IsAuthenticated);
                ResultToken.Output.Add("UserName", _httpContextAccessor.HttpContext.User?.Identity?.Name);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ExceptionMessage} | {ExceptionTrace}", ex.Message, ex.StackTrace);
                ResultToken.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionErorr);
                ResultToken.Header.Message = ex.Message;
            }
            return ResultToken;
        }
    }
}
