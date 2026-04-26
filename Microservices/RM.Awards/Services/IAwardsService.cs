using RM.Awards.Dtos;
using RM.Core.Services;

namespace RM.Awards.Services
{
    public interface IAwardsService : IBaseService
    {
        Task<OperationOutput> GetAwardsList(Dtos.Awards RequestedData);
        Task<OperationOutput> SaveAwards(Dtos.Awards RequestdData);
        Task<OperationOutput> GetAwardsDetails(Dtos.Awards RequestedData);
        OperationOutput SortOrder(List<Dtos.Awards> RequestedData);
        OperationOutput ModelActions(Dtos.Awards RequestedData);
    }
}
