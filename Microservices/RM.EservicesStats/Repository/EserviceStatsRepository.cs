using EservicesStatistics;
using System.Collections.Generic;
using System.Linq;

namespace RM.EservicesStats.Repository
{
    public class EserviceStatsRepository: IEserviceStatsRepository
    {
       
        private bool IsTransaction = false;
        ServicesStatisitcsClient servicesStatisitcsClient = new ServicesStatisitcsClient();
        public EserviceStatsRepository()
        {
             
        }
    
        //public bool Delete(int ID)
        //{
        //    User? Item = _dbContext.Users.Find(ID);
        //    if (Item == null)
        //    {
        //        return false;
        //    }
        //    Item.IsDeleted = true;
        //    return Save();
        //}

        public decimal GetEserviceTotalRequests(int year)
        {
            
            decimal totalRequests=servicesStatisitcsClient.GetTotalRequestsCount(year, year);
            return totalRequests;
        }
        public List<BuildingLicenseStatisitcsInfo> GetBuildingRequestsYearly(int FromYear,int ToYear)
        {

            List<BuildingLicenseStatisitcsInfo> totalRequests = servicesStatisitcsClient.GetBuildingLicenseYearlyLicense(FromYear, ToYear).ToList();
            return totalRequests;
        }
        public List<BuildingLicenseStatisitcsInfo> GetBuildingRequestsMonthly( int ToYear)
        {

            List<BuildingLicenseStatisitcsInfo> totalRequests = servicesStatisitcsClient.GetBuildingLicenseMonthlyLicense(ToYear, ToYear).ToList();
            return totalRequests;
        }
        public List<MedicalLicenseStatisitcsInfo> GetMedicalRequestsYearly(int FromYear, int ToYear)
        {

            List<MedicalLicenseStatisitcsInfo> totalRequests = servicesStatisitcsClient.GetMedicalLicenseYearlyLicense(FromYear, ToYear).ToList();
            return totalRequests;
        }
        public List<MedicalLicenseStatisitcsInfo> GetMedicalRequestsMonthly(int FromYear, int ToYear)
        {

            List<MedicalLicenseStatisitcsInfo> totalRequests = servicesStatisitcsClient.GetMedicalLicenseMonthlyLicense(FromYear, ToYear).ToList();
            return totalRequests;
        }
        public List<SWPlLicenseStatisitcsInfo> GetSWPRequestsYearly(int FromYear, int ToYear)
        {

            List<SWPlLicenseStatisitcsInfo> totalRequests = servicesStatisitcsClient.GetSWPLicenseYearlyLicense(FromYear, ToYear).ToList();
            return totalRequests;
        }
        public List<SWPlLicenseStatisitcsInfo> GetSWPRequestsMonthly(int FromYear, int ToYear)
        {

            List<SWPlLicenseStatisitcsInfo> totalRequests = servicesStatisitcsClient.GetSWpLicenseMonthlyLicense(FromYear, ToYear).ToList();
            return totalRequests;
        }
        public List<TradingLicenseStatisitcsInfo> GetTradingRequestsYearly(int FromYear, int ToYear)
        {

            List<TradingLicenseStatisitcsInfo> totalRequests = servicesStatisitcsClient.GetTradingLicenseYearlyLicense(FromYear, ToYear).ToList();
            return totalRequests;
        }
        public List<TradingLicenseStatisitcsInfo> GetTradingRequestsMonthly(int FromYear, int ToYear)
        {

            List<TradingLicenseStatisitcsInfo> totalRequests = servicesStatisitcsClient.GetTradingLicenseMonthlyLicense(FromYear, ToYear).ToList();
            return totalRequests;
        }
        //public decimal GetBuildingRequestsMonthly(int year)
        //{

        //    BuildingRequestsStatisitcsInfo totalRequests = servicesStatisitcsClient.buil(year, year);
        //    return totalRequests.TotalCount;
        //}




    }
}
