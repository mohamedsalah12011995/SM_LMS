
using RM.Core.Services;
using RM.Partners.Dtos;

namespace RM.Partners.Services
{
    public interface IPartnersService:IBaseService
    {
        Task<OperationOutput> GetPartners(Dtos.Partner RequestedData);
        Task<OperationOutput> GetPartnerDetails(Dtos.Partner RequestedData);
        Task<OperationOutput> SavePartners(Dtos.Partner RequestdData);
        Task<OperationOutput> ModelAction(Dtos.Partner RequestedData);
    }
}
