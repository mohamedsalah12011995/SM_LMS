using RM.Core.Services;
using RM.Events.Dtos;

namespace RM.Events.Services
{
    public interface IEidFiterRequestService:IBaseService
    {
        Task<OperationOutput> Save(Dtos.EidFiterRequest RequestedData);
        Task<OperationOutput> GetEdiFitrRequest(Dtos.EidFiterRequest RequestedData);

        Task<OperationOutput> GetLookups();

    }
}
