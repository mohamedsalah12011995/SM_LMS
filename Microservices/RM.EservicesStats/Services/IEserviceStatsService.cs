
using RM.Core.Services;
using RM.EservicesStats.Dtos;
namespace RM.EservicesStats.Services
{
    public interface IEserviceStatsService:IBaseService
    {
        OperationOutput GetTotalStatsYearly();
        OperationOutput GetBuildingStatsYearly();
        OperationOutput GetBuildingStatsMonthly();
        OperationOutput GetMedicalStatsYearly();
        OperationOutput GetMedicalStatsMonthly();
        OperationOutput GetSWPStatsYearly();
        OperationOutput GetSWPStatsMonthly();
        OperationOutput GetTradingStatsYearly();
        OperationOutput GetTradingStatsMonthly();
    }
}
