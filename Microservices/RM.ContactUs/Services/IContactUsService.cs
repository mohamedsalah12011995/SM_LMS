
using RM.ContactUs.Dtos;
using RM.Core.Services;
namespace RM.ContactUs.Services
{
    public interface IContactUsService:IBaseService
    {
        Task<OperationOutput> GetContactUsList(Dtos.ContactUs RequestedData);
        Task<OperationOutput> GetContactUsDetails(Dtos.ContactUs RequestedData);
        OperationOutput CpGetContactUsDetails(Dtos.ContactUs RequestedData);
        Task<OperationOutput> GetAmana940Categories(Dtos.Amana940Category RequestedData);
        OperationOutput SaveContactUs(Dtos.ContactUs RequestedData);

        OperationOutput GetContactDetails(Dtos.ContactUs RequestedData);

        OperationOutput GetRegionRefernces();
    }
}
