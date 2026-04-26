using RM.Advertisements.Dtos;
using RM.Core.Services;

namespace RM.Advertisements.Services
{
    public interface IAdvertisementsService: IBaseService
    {
        OperationOutput GetReferences();

        Task<OperationOutput> GetAdvertismentsList(Dtos.Advertisements RequestedData);
        Task<OperationOutput> GetMainSliderList(Dtos.Advertisements RequestedData);
        Task<OperationOutput> SaveAdvertisment(Dtos.Advertisements RequestedData);
        Task<OperationOutput> GetAdvertismentDetails(Dtos.Advertisements RequestedData);

        OperationOutput ModelActions(Dtos.Advertisements RequestedData);
        OperationOutput SortOrder(List<Dtos.Advertisements> RequestedData);

        OperationOutput GetPlatformLookups();

        Task<OperationOutput> GetPlatformAdvertismentsList(Dtos.Advertisements RequestedData);
        Task<OperationOutput> SavePlatformAdvertisment(Dtos.Advertisements RequestedData);
        Task<OperationOutput> GetPlatformAdvertismentDetails(Dtos.Advertisements RequestedData);
    }
}
