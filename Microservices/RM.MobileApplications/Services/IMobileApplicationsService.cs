
using RM.Core.Services;
using RM.MobileApplications.Dtos;

namespace RM.MobileApplications.Services
{
    public interface IMobileApplicationsService:IBaseService
    {

        Task<OperationOutput> GetMobileApplicationList(Dtos.MobileApplications RequestedData);
        Task<OperationOutput> GetMobileApplicationDetails(Dtos.MobileApplications RequestedData);
        Task<OperationOutput> GetMobileApplicationDetails(int Id);
        Task<OperationOutput> SaveApplicationInformation(Dtos.MobileApplications RequestedData);
        Task<OperationOutput> ModelAction(Dtos.MobileApplications RequestedData);
    }
}
