
using RM.Regulations.Dtos;

namespace RM.Regulations.Services
{
    public interface IRegulationService
    {
        Task<OperationOutput> GetRegulationsLookups(Dtos.Regulations RequestedData);
        Task<OperationOutput> GetRegulationsList(Dtos.Regulations RequestedData);
        Task<OperationOutput> GetRegulationDetails(Dtos.Regulations RequestedData);
        Task<OperationOutput> SaveRegulationCategory(Dtos.Regulations RequestedData);
        Task<OperationOutput> SaveRegulations(Dtos.Regulations RequestedData);
        Task<OperationOutput> ModelAction(Dtos.Regulations RequestedData);
    }
}