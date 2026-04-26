using IntegrationService.Records.Hars;
using IntegrationService.Records.Nafath;
using Mapster;

namespace IntegrationService.Services
{
    public interface INafathService
    {
        Task<SendRequestResponse> SendRequestForGetRandom(SendRequestRecord data);
        Task<NaFathCallbackStatusResult> NaFathCallbackStatus(NaFathCallbackStatusRecord data);
        Task<NaFathCallbackStatusResult> GetNaFathCallbackStatus(NaFathCallbackStatusResult data);
        Task<CheckRequestResponse> CheckRequestStatusNaFath(CheckRequestRecord data);
    }
}
