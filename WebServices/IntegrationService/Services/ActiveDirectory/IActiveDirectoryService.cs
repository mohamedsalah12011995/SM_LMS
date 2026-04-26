using IntegrationService.Records.AD;

namespace IntegrationService.Services
{
    public interface IActiveDirectoryService
    {
        Task<UserAD> GetADUserInfo(GetADUserRecord userInfo);
        Task<UserAD> UserLogin(GetADUserRecord user);

    }
}
