using IntegrationService.Records.SMS;
using IntegrationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SMSController : ControllerBase
    {
        private readonly ISMSService _SMSService;
        public SMSController(ISMSService SMSService)
        {
            _SMSService = SMSService;
        }

        [HttpPost]
        public async Task<GetSMSData> SendMsg(SendMsgRecord data)
        {
            return await _SMSService.SendMsg(data);
        }

    }
}
