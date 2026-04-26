


using RM.Lookups.Dtos;

namespace RM.Lookups.Services
{
    public interface IRecommendionService
    {
        Task<OperationOutput> GetLookups(Recommendations RequestedData);
        Task<OperationOutput> GetAllAsync(Recommendations dto);
        Task<OperationOutput> GetAsync(Recommendations dto);
        Task<OperationOutput> AddAsync(Recommendations dto);
        Task<OperationOutput> UpdateAsync(Recommendations dto);
        Task<OperationOutput> DeleteAsync(Recommendations dto);
    }
}
