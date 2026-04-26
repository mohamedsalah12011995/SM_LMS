using IntegrationService.Records.Hars;
using IntegrationService.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationService.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HarsController:ControllerBase
    {
        private readonly IPInfoService _PInfoService;
        public HarsController(IPInfoService PInfoService)
        {
            _PInfoService = PInfoService;
        }

        #region WSData

        [HttpPost]
        public async Task<IActionResult> GetWSData(GetWSData data)
        {
            return Ok(await _PInfoService.GetWSData(data));
        }

        [HttpPost]
        public async Task<IActionResult> PostWSData(GetWSData data)
        {
            return Ok(await _PInfoService.PostWSData(data));
        }

        #endregion

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

        #endregion

        #region PayrollWS

        [HttpPost]
        public async Task<GetLastPayrollResponseBody> GetLastPayroll(GetLastPayroll personInfo)
        {
            return await _PInfoService.GetLastPayroll(personInfo);
        }

        #endregion

        #region passws
        [HttpPost]
        public async Task<IActionResult> GetDetailedStatistics(GetDetailedStatistics data)
        {
            return Ok(await _PInfoService.GetDetailedStatistics(data));
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> GetExternalData(GetExternalData data)
        {
            return Ok(await _PInfoService.GetExternalData(data));
        }
    }
}
