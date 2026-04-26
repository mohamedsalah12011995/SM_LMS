using IntegrationService.Records.Hars;
using Mapster;

namespace IntegrationService.Services
{
    public interface IPInfoService
    {
        Task<object> GetWSData(GetWSData data);

        Task<object> PostWSData(GetWSData data);

        Task<GetPersonInfoDataByPassport> GetGccInfoByPassport(GetGccInfoByPassportRequestBody personInfo);

        Task<GetPersonInfoData> GetPersonInfoWithSecuirtyStatus(GetPersonInfoRequestBody personInfo);

        Task<GetPersonInfoData> GetGccInfoByNIN(GetPersonInfoRequestBody personInfo);

        Task<GetPersonInfoData> GetPersonInfoWithDetailedSecuirtyStatus(GetPersonInfoRequestBody personInfo);

        Task<GetCarInfoData> GetCarInfoByPlate(GetCarInfoByPlateRequestBody personInfo);
        Task<GetPersonInfoData> GetPersonInfo(GetPersonInfoRequestBody personInfo);

        Task<GetLastPayrollResponseBody> GetLastPayroll(GetLastPayroll personInfo);
        Task<object> GetDetailedStatistics(GetDetailedStatistics data);
        Task<object> GetExternalData(GetExternalData data);


    }
}
