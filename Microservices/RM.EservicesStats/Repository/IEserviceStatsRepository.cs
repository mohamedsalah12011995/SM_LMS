using EservicesStatistics;

namespace RM.EservicesStats.Repository
{
    public interface IEserviceStatsRepository
    {
         decimal GetEserviceTotalRequests(int year);
         List<BuildingLicenseStatisitcsInfo> GetBuildingRequestsYearly(int FromYear, int ToYear);
         List<BuildingLicenseStatisitcsInfo> GetBuildingRequestsMonthly(int ToYear);
         List<MedicalLicenseStatisitcsInfo> GetMedicalRequestsYearly(int FromYear, int ToYear);
         List<MedicalLicenseStatisitcsInfo> GetMedicalRequestsMonthly(int FromYear, int ToYear);
         List<SWPlLicenseStatisitcsInfo> GetSWPRequestsYearly(int FromYear, int ToYear);
         List<SWPlLicenseStatisitcsInfo> GetSWPRequestsMonthly(int FromYear, int ToYear);
         List<TradingLicenseStatisitcsInfo> GetTradingRequestsYearly(int FromYear, int ToYear);
         List<TradingLicenseStatisitcsInfo> GetTradingRequestsMonthly(int FromYear, int ToYear);
    }
}
