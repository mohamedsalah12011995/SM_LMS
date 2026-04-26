using IntegrationService.Records.Nafath;
using IntegrationService.Records.SMS;
using IntegrationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NafathController : ControllerBase
    {
        private readonly INafathService _NafathService;
        public NafathController(INafathService NafathService)
        {
            _NafathService = NafathService;
        }

        [HttpPost]
        public async Task<SendRequestResponse> SendRequestForGetRandom(SendRequestRecord data)
        {
            return await _NafathService.SendRequestForGetRandom(data);
        }

        [ApiKeyAuth]
        [HttpPost]
        public async Task<NaFathCallbackStatusResult> NaFathCallbackStatus(NaFathCallbackStatusRecord data)
        {
            return await _NafathService.NaFathCallbackStatus(data);
        }

        [HttpPost]
        public async Task<NaFathCallbackStatusResult> GetNaFathCallbackStatus(NaFathCallbackStatusResult data)
        {
            return await _NafathService.GetNaFathCallbackStatus(data);
        }

        [HttpPost]
        public async Task<CheckRequestResponse> CheckRequestStatusNaFath(CheckRequestRecord data)
        {
            return await _NafathService.CheckRequestStatusNaFath(data);
        }

    }
}
