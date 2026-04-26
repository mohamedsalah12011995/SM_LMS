using IntegrationService.Records.AD;
using IntegrationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ADController : ControllerBase
    {
        private readonly IActiveDirectoryService _ActiveDirectoryService;
        public ADController(IActiveDirectoryService ActiveDirectoryService)
        {
            _ActiveDirectoryService = ActiveDirectoryService;
        }


        [HttpPost]
        public async Task<UserAD> GetADUserInfo(GetADUserRecord userInfo)
        {
            return await _ActiveDirectoryService.GetADUserInfo(userInfo);
        }

        [HttpPost]
        public async Task<UserAD> UserLogin(GetADUserRecord userInfo)
        {
            return await _ActiveDirectoryService.UserLogin(userInfo);
        }

    }
}
