using IntegrationService.Records.Hars;
using IntegrationService.Records.SMS;
using IntegrationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController:ControllerBase
    {
        private readonly ITestService _PInfoService;
        public TestController(ITestService PInfoService)
        {
            _PInfoService = PInfoService;
        }

        #region YaqeenWS

        [HttpPost]
        public async Task<GetPersonInfoDataByPassport> GetGccInfoByPassport(GetGccInfoByPassportRequestBody personInfo)
        {
            return await _PInfoService.GetGccInfoByPassport(personInfo);
        }

        [HttpPost]
        public async Task<GetPersonInfoData> GetPersonInfoWithSecuirtyStatus(GetPersonInfoRequestBody personInfo)
        {
            return await _PInfoService.GetPersonInfoWithSecuirtyStatus(personInfo);
        }

        [HttpPost]
        public async Task<GetPersonInfoData> GetGccInfoByNIN(GetPersonInfoRequestBody personInfo)
        {
            return await _PInfoService.GetGccInfoByNIN(personInfo);
        }

        [HttpPost]
        public async Task<GetPersonInfoData> GetPersonInfoWithDetailedSecuirtyStatus(GetPersonInfoRequestBody personInfo)
        {
            return await _PInfoService.GetPersonInfoWithDetailedSecuirtyStatus(personInfo);
        }

        [HttpPost]
        public async Task<GetCarInfoData> GetCarInfoByPlate(GetCarInfoByPlateRequestBody personInfo)
        {
            return await _PInfoService.GetCarInfoByPlate(personInfo);
        }

        [HttpPost]
        public async Task<GetPersonInfoData> GetPersonInfo(GetPersonInfoRequestBody personInfo)
        {

            return await _PInfoService.GetPersonInfo(personInfo);
 
        }

        [HttpPost]
        public async Task<GetSMSData> SendMsg(SendMsgRecord data)
        {
            return await _PInfoService.SendMsg(data);
        }

        #endregion


    }
}
