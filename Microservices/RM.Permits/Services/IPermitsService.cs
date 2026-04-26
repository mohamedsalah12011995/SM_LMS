using RM.Core.Services;
using RM.Permits.Dtos;

namespace RM.Permits.Services
{
    public interface IPermitService : IBaseService
    {
        OperationOutput GetPermitRequestCPLookups();
        Task<OperationOutput> GetPermitRequestsList(PermitRequest RequestedData);

        Task<OperationOutput> GetPermitRequestDetails(PermitRequestGetByID RequestedData);
        Task<OperationOutput> ChangeActionPermitRequest(PermitAddAction RequestedData);
        Task<OperationOutput> GetIntegrationData(UserInformation userInfo);
        Task<OperationOutput> GetPermitRequestLookups(PermitRequestLookup RequestdData);
        Task<OperationOutput> SavePermitRequest(PermitRequest RequestdData);
        Task<OperationOutput> QueryPersonalPermitRequests(QueryPersonPermitRequests RequestdData);
        Task<OperationOutput> QueryCarPermitRequests(QueryCarPermitRequests RequestdData);

        OperationOutput RequestToPrintPermit(PermitRequestGetByID RequestdData);
        Task<OperationOutput> GetCompanyInfo();
        Task<OperationOutput> PrintPermit(PrintPermitDto RequestedData);
        Task<OperationOutput> SaveInteractionStatistics(InteractionStatisticsDto requestData);

    }
}
