using RM.Core.Helpers;
using RM.Users.Dtos;

namespace RM.Users.Services
{
    public interface IUsersService
    {
        public Task<OperationOutput> SaveUser(Dtos.Users RequestedData);
        public OperationOutput GenerateToken2(string userName);
        public Task<OperationOutput> UserLogin(Dtos.Users RequestedData);
        public Task<OperationOutput> ChangeUserReference(Dtos.Users RequestedData);
        public OperationOutput CheckUserLogged();
        public Task<OperationOutput> GetUserInfoByToken(string Token);
        public Task<OperationOutput> IAMLogin(string IdNo, string DobHijri, string IdType, string LoginSources);
        public Task<OperationOutput> GetUserLookps();
        public Task<OperationOutput> GetTotalUserLookups();
        public OperationOutput GenerateUserToken(Dtos.Users user = null);
        public Task<OperationOutput> GetUsersList(Dtos.Users RequestedData);
        public Task<OperationOutput> GetUserDetails(Dtos.Users RequestedData);
        public Task<OperationOutput> UserAction(Dtos.Users RequestedData);
        public Task<OperationOutput> CompleteUserRegistration(Dtos.Users RequestedData);
        public Task<int> CheckUserName(string userName);
        public Task<OperationOutput> CheckUserLoginFromActiveDirectory(Dtos.Users RequestedData);
        public Task<OperationOutput> GetAllMajorsWithReferencesTree(ReferencesTree RequestedData);
        public Task<OperationOutput> RefreshToken(Dtos.Users RequestedData);

        Task<OperationOutput> UserLoginOTP(Dtos.Users RequestedData);
        Task<OperationOutput> CheckOTP(Dtos.Users RequestedData);
    }
}
