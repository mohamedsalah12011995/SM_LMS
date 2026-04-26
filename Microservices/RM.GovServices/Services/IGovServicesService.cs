using RM.Core.Services;
using RM.GovServices.Dtos;

namespace RM.GovServices.Services
{
    public interface IGovServicesService:IBaseService
    {
        Task<OperationOutput> GetGovServicesCategories(Dtos.Govservice RequestedData);
        Task<OperationOutput> GetEservicesCategories(Dtos.Govservice RequestedData);
        Task<OperationOutput> GetGovserviceList(Dtos.Govservice RequestedData);
        Task<OperationOutput> GetEserviceList(Dtos.Eservices RequestedData);
        Task<OperationOutput> GetGovserviceDetails(Dtos.Govservice RequestedData);
        Task<OperationOutput> GetGovserviceDetails(int Id);
        Task<OperationOutput> GetEserviceDetails(Dtos.Eservices RequestedData);
        Task<OperationOutput> GetEserviceDetails(int Id);
        Task<OperationOutput> SaveGovService(Dtos.Govservice RequestedData);
        Task<OperationOutput> SaveEservice(Dtos.Eservices RequestedData);
        Task<OperationOutput> GovServicesModelActions(Dtos.Govservice RequestedData);
        Task<OperationOutput> EservicesModelActions(Dtos.Eservices RequestedData);
    }
}
