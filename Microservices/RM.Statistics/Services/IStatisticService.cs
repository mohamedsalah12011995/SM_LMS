
using RM.Statistics.Dtos;

namespace RM.Statistics.Services
{
    public interface IStatisticService
    {
        Task<OperationOutput> GetEntitiesStatistics(Dtos.Statistics RequestedData);
        OperationOutput GetPortalLatestUpdate(Dtos.Statistics RequestedData);
        Task<OperationOutput> SaveInteractionStatistics(int? ReferenceId, int? EntityId, int? ItemId, int? StatisticsType, string ItemUrl = null,int? value = null);
        OperationOutput GetEntitiesLatestUpdate(int? ReferenceId);
        Task<OperationOutput> SaveIsHelpful(int? ReferenceId, int? EntityId, int? ItemId, bool isHelpful, string ItemUrl = null);
        Task<OperationOutput> GetTotalStatistics(Dtos.Statistics RequestedData);
    }
}