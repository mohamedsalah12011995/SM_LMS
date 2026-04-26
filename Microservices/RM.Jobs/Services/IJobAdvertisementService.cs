
using RM.Core.Services;
using RM.Jobs.Dtos;

namespace RM.Jobs.Services
{
    public interface IJobAdvertisementService:IBaseService
    {
        Task<OperationOutput> GetAdvertismentCareerLookup(Dtos.JobAdvertisement RequestedData);
        Task<OperationOutput> GetQualifications();
        Task<OperationOutput> GetJobAdvertiesmentList(Dtos.JobAdvertisement RequestedData);
        Task<OperationOutput> SaveJobAdvertiesment(Dtos.JobAdvertisement RequestedData);
        Task<OperationOutput> GetJobAdvertiesmentDetails(Dtos.JobAdvertisement RequestedData);
        Task<OperationOutput> GetJobAdvertiesmentDetails(int? Id);
        Task<OperationOutput> ModelAction(Dtos.JobAdvertisement RequestedData);
        Task<OperationOutput> CareerModelActions(Dtos.JobCareer RequestedData);
        Task<OperationOutput> GetJobCareerDetails(Dtos.JobCareer RequestedData);
        Task<OperationOutput> SaveJobLookups(Dtos.JobLookUp RequestedData);
    }
}
