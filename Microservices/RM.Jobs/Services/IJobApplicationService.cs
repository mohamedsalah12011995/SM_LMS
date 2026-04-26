
using RM.Core.Services;
using RM.Jobs.Dtos;
namespace RM.Jobs.Services
{
    public interface IJobApplicationService: IBaseService
    {

        Task<OperationOutput> GetJobApplicationsLookups(JobApplication RequestedData);
        Task<OperationOutput> GetJobApplicationList(JobApplication RequestedData);
        Task<OperationOutput> GetApplicationDetails(JobApplication RequestedData);
        Task<OperationOutput> GetApplicationDetails(int? Id);
        Task<OperationOutput> SaveJobApplicationList(List<JobApplication> RequestedData);
        Task<OperationOutput> SendJobApplicationNotification(JobApplicationNotification RequestedData);
        Task<OperationOutput> AddJobApplication(JobApplication RequestdData);
        Task<OperationOutput> RejectJobAppList(List<JobApplication> RequestedData);
        Task<OperationOutput> ReAgreeJobAppList(List<JobApplication> RequestedData);
        Task<OperationOutput> DeleteJobAppList(List<JobApplication> RequestedData);
        Task<OperationOutput> ModelActions(JobApplication RequestedData);
        Task<OperationOutput> QueryJobApplication(JobApplication RequestedData);
        Task<OperationOutput> QueryApplication(JobApplication RequestedData);
        Task<OperationOutput> GetMilitaryJobApplicationInfo(JobApplication RequestedData);
        Task<OperationOutput> SearchApplication(JobApplication RequestedData);

    }
}
