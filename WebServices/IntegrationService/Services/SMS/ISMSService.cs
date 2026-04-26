using IntegrationService.Records.Hars;
using IntegrationService.Records.SMS;
using Mapster;

namespace IntegrationService.Services
{
    public interface ISMSService
    {
        Task<GetSMSData> SendMsg(SendMsgRecord data);

    }
}
