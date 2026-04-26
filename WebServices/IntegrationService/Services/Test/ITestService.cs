using IntegrationService.Records.Hars;
using IntegrationService.Records.SMS;
using Mapster;

namespace IntegrationService.Services
{
    public interface ITestService
    {

        Task<GetPersonInfoDataByPassport> GetGccInfoByPassport(GetGccInfoByPassportRequestBody personInfo);

        Task<GetPersonInfoData> GetPersonInfoWithSecuirtyStatus(GetPersonInfoRequestBody personInfo);

        Task<GetPersonInfoData> GetGccInfoByNIN(GetPersonInfoRequestBody personInfo);

        Task<GetPersonInfoData> GetPersonInfoWithDetailedSecuirtyStatus(GetPersonInfoRequestBody personInfo);

        Task<GetCarInfoData> GetCarInfoByPlate(GetCarInfoByPlateRequestBody personInfo);
        Task<GetPersonInfoData> GetPersonInfo(GetPersonInfoRequestBody personInfo);
        Task<GetSMSData> SendMsg(SendMsgRecord data);


    }
}
