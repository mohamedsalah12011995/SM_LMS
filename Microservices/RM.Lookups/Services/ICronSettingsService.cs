using RM.Lookups.Dtos;

namespace RM.Lookups.Services
{
    public interface ICronSettingsService
    {
        Task<OperationOutput> GetLookups(CronSettings RequestedData);
        Task<OperationOutput> GetUsersLookup(CronSettings RequestedData);
        Task<OperationOutput> GetAllAsync(CronSettings dto);
        Task<OperationOutput> SaveAsync(List<CronSettings> RequestedData);
        Task<OperationOutput> GetAsync(CronSettings dto);
        Task<OperationOutput> AddAsync(CronSettings dto);
        Task<OperationOutput> UpdateAsync(CronSettings dto);
        Task<OperationOutput> DeleteAsync(CronSettings dto);
    }
}
