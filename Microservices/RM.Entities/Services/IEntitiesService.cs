
using RM.Entities.Dtos;

namespace RM.Entities.Services
{
    public interface IEntitiesService
    {
        OperationOutput GetEntityLookups();
        Task<OperationOutput> SaveEntity(Dtos.Entity RequestedData);
        Task<OperationOutput> GetEntityDetails(Dtos.Entity RequestedData);
        Task<OperationOutput> GetEntitiesList(Dtos.Entity RequestedData);
        OperationOutput EntityActivation(Dtos.Entity RequestedData);

    }
}
