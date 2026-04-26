using RM.Core.Services;
using RM.Officials.Dtos;
namespace RM.Officials.Services
{
    public interface IOfficialService : IBaseService
    {
        Task<OperationOutput> GetOfficialsList(OfficialDto RequestedData);
        Task<OperationOutput> GetOfficialDetails(OfficialDto RequestedData);
        Task<OperationOutput> SaveOfficial(OfficialDto RequestedData);
        Task<OperationOutput> SortOrder(List<OfficialDto> RequestedData);
        Task<OperationOutput> ModelAction(OfficialDto RequestedData);
    }
}