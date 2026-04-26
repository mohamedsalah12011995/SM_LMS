
using RM.Lookups.Dtos;
namespace RM.Lookups.Services
{
    public interface ILookupsService
    {
        Task<OperationOutput> GetLookups(Dtos.MajorLookups RequestedData);
        Task<OperationOutput> GetLookupsList(Dtos.MajorLookupsType RequestedData);
        Task<OperationOutput> GetLookupDetails(Dtos.MajorLookupsType RequestedData);
        Task<OperationOutput> SaveLookup(Dtos.MajorLookupsType RequestedData);
        Task<OperationOutput> Activation(Dtos.MajorLookupsType RequestedData);
        Task<OperationOutput> DeleteMajorLookup(Dtos.MajorLookups RequestedData);
    }
}
