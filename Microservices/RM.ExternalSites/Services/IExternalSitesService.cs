
using RM.Core.Services;
using RM.ExternalSites.Dtos;

namespace RM.ExternalSites.Services
{
    public interface IExternalSitesService: IBaseService
    {
        Task<OperationOutput> GetCategories(Dtos.ExternalSites RequestedData);
        Task<OperationOutput> GetExternalSites(Dtos.ExternalSites RequestedData);
        Task<OperationOutput> GetExternalSitesList(Dtos.ExternalSites RequestedData);
        Task<OperationOutput> GetExternalSitesDetails(Dtos.ExternalSites RequestedData);
        Task<OperationOutput> GetExternalSitesDetails(int Id);
        Task<OperationOutput> SaveExternalSite(Dtos.ExternalSites RequestedData);
        Task<OperationOutput> ModelActions(Dtos.ExternalSites RequestedData);
    }
}
