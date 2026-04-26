
using RM.Core.Helpers;
using RM.EservicesStats.Services;
using RM.EservicesStats.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using static RM.EservicesStats.Dtos.OperationOutput;
using RM.EservicesStats.Repository;
using RM.EservicesStats.Controllers;
using RM.Core.Services;

namespace RM.EservicesStats.Services
{
    public class EserviceStatsService:BaseService, IEserviceStatsService
    {
        IEserviceStatsRepository eserviceStatsRepository;
        public EserviceStatsService(IHttpContextAccessor httpContextAccessor,IConfiguration configuration, IEserviceStatsRepository _eserviceStatsRepository)
        :base(httpContextAccessor, configuration)
        {
            eserviceStatsRepository = _eserviceStatsRepository;
        }
        public OperationOutput GetTotalStatsYearly()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);
            
            decimal CurrRequests = eserviceStatsRepository.GetEserviceTotalRequests(  DateTime.Now.Year);
            decimal PrevRequests  = eserviceStatsRepository.GetEserviceTotalRequests(  DateTime.Now.Year-1);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.CurrEserviceStatsEntity, CurrRequests),
                    new OutputDictionary(OperationOutputKeys.PrevEserviceStatsEntity, PrevRequests));
        }

        public OperationOutput GetBuildingStatsYearly()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<EservicesStatistics.BuildingLicenseStatisitcsInfo> buildingLicenseStatisitcsInfos = eserviceStatsRepository.GetBuildingRequestsYearly(DateTime.Now.Year , DateTime.Now.Year);
            var Item = buildingLicenseStatisitcsInfos.OrderBy(i => i.LicenseYear).Select(i => new Dtos.EserviceStats
            {
                Month = i.Month,
                Year = i.LicenseYear.ToString(),
                NewCount = i.NewLicenseCount,
                ReNewCount = i.ReNewLicenseCount
            }).ToList();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.EserviceStatsEntity, Item));
        }
        public OperationOutput GetBuildingStatsMonthly()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<EservicesStatistics.BuildingLicenseStatisitcsInfo> buildingLicenseStatisitcsInfos = eserviceStatsRepository.GetBuildingRequestsMonthly(DateTime.Now.Year);
            //var PrevItem = buildingLicenseStatisitcsInfos.Where(i => i.LicenseYear.ToString() == (DateTime.Now.Year - 1).ToString())
            //    .Select(i => new Entities.EserviceStats
            //    {
            //        Year = i.LicenseYear.ToString(),
            //        Month = i.Month.ToString(),
            //        NewCount = i.NewLicenseCount,

            //        ReNewCount = i.ReNewLicenseCount
            //    }).OrderBy(i => i.Month).ToList();
           var CurrItem = buildingLicenseStatisitcsInfos.Where(i => i.LicenseYear.ToString() == (DateTime.Now.Year).ToString())
               .Select(i => new Dtos.EserviceStats
               {
                   Year = i.LicenseYear.ToString(),
                   Month = i.Month.ToString(),
                   NewCount = i.NewLicenseCount,
                   ReNewCount = i.ReNewLicenseCount
               }).OrderBy(i => i.Month).ToList();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                  //  new OutputDictionary(OperationOutputKeys.PrevEserviceStatsEntity, PrevItem),
                    new OutputDictionary(OperationOutputKeys.CurrEserviceStatsEntity, CurrItem));
        }


        public OperationOutput GetMedicalStatsYearly()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<EservicesStatistics.MedicalLicenseStatisitcsInfo> medicalLicenseStatisitcsInfos = eserviceStatsRepository.GetMedicalRequestsYearly(DateTime.Now.Year , DateTime.Now.Year);
            var Item = medicalLicenseStatisitcsInfos.OrderBy(i => i.LicenseYear).Select(i => new Dtos.EserviceStats
            {
                Month = i.Month,
                Year = i.LicenseYear.ToString(),
                NewCount = i.NewLicenseCount,
                ReNewCount = i.ReNewLicenseCount
            }).ToList();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.EserviceStatsEntity, Item));
        }
        public OperationOutput GetMedicalStatsMonthly()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<EservicesStatistics.MedicalLicenseStatisitcsInfo> medicalLicenseStatisitcsInfos = eserviceStatsRepository.GetMedicalRequestsMonthly(DateTime.Now.Year , DateTime.Now.Year);
            //var PrevItem = medicalLicenseStatisitcsInfos.Where(i => i.LicenseYear.ToString() == (DateTime.Now.Year - 1).ToString())
            //    .Select(i => new Entities.EserviceStats
            //    {
            //        Year = i.LicenseYear.ToString(),
            //        Month = i.Month.ToString(),
            //        NewCount = i.NewLicenseCount,
            //        ReNewCount = i.ReNewLicenseCount
            //    }).OrderBy(i => i.Month).ToList();
            var CurrItem = medicalLicenseStatisitcsInfos.Where(i => i.LicenseYear.ToString() == (DateTime.Now.Year).ToString())
               .Select(i => new Dtos.EserviceStats
               {
                   Year = i.LicenseYear.ToString(),
                   Month = i.Month.ToString(),
                   NewCount = i.NewLicenseCount,
                   ReNewCount = i.ReNewLicenseCount
               }).OrderBy(i => i.Month).ToList();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    //  new OutputDictionary(OperationOutputKeys.PrevEserviceStatsEntity, PrevItem),
                    new OutputDictionary(OperationOutputKeys.CurrEserviceStatsEntity, CurrItem));
        }


        public OperationOutput GetSWPStatsYearly()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<EservicesStatistics.SWPlLicenseStatisitcsInfo> swpLicenseStatisitcsInfos = eserviceStatsRepository.GetSWPRequestsYearly(DateTime.Now.Year  , DateTime.Now.Year);
            var Item = swpLicenseStatisitcsInfos.OrderBy(i => i.LicenseYear).Select(i => new Dtos.EserviceStats
            {
                Month = i.Month,
                Year = i.LicenseYear.ToString(),
                NewCount = i.NewLicenseCount,
                ReNewCount = i.ReNewLicenseCount
            }).ToList();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.EserviceStatsEntity, Item));
        }
        public OperationOutput GetSWPStatsMonthly()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<EservicesStatistics.SWPlLicenseStatisitcsInfo> swpLicenseStatisitcsInfos = eserviceStatsRepository.GetSWPRequestsMonthly(DateTime.Now.Year , DateTime.Now.Year);
            //var PrevItem = swpLicenseStatisitcsInfos.Where(i => i.LicenseYear.ToString() == (DateTime.Now.Year - 1).ToString())
            //    .Select(i => new Entities.EserviceStats
            //    {
            //        Year = i.LicenseYear.ToString(),
            //        Month = i.Month.ToString(),
            //        NewCount = i.NewLicenseCount,
            //        ReNewCount = i.ReNewLicenseCount
            //    }).OrderBy(i => i.Month).ToList();
            var CurrItem = swpLicenseStatisitcsInfos.Where(i => i.LicenseYear.ToString() == (DateTime.Now.Year).ToString())
               .Select(i => new Dtos.EserviceStats
               {
                   Year = i.LicenseYear.ToString(),
                   Month = i.Month.ToString(),
                   NewCount = i.NewLicenseCount,
                   ReNewCount = i.ReNewLicenseCount
               }).OrderBy(i => i.Month).ToList();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    //  new OutputDictionary(OperationOutputKeys.PrevEserviceStatsEntity, PrevItem),
                    new OutputDictionary(OperationOutputKeys.CurrEserviceStatsEntity, CurrItem));
        }



        public OperationOutput GetTradingStatsYearly()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<EservicesStatistics.TradingLicenseStatisitcsInfo> tradingLicenseStatisitcsInfos = eserviceStatsRepository.GetTradingRequestsYearly(DateTime.Now.Year , DateTime.Now.Year);
            var Item = tradingLicenseStatisitcsInfos.OrderBy(i => i.LicenseYear).Select(i => new Dtos.EserviceStats
            {
                Month = i.Month,
                Year = i.LicenseYear.ToString(),
                NewCount = i.NewLicenseCount,
                ReNewCount = i.ReNewLicenseCount
            }).ToList();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.EserviceStatsEntity, Item));
        }
        public OperationOutput GetTradingStatsMonthly()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<EservicesStatistics.TradingLicenseStatisitcsInfo> tradingLicenseStatisitcsInfos = eserviceStatsRepository.GetTradingRequestsMonthly(DateTime.Now.Year , DateTime.Now.Year);
            //var PrevItem = tradingLicenseStatisitcsInfos.Where(i => i.LicenseYear.ToString() == (DateTime.Now.Year - 1).ToString())
            //    .Select(i => new Entities.EserviceStats
            //    {
            //        Year = i.LicenseYear.ToString(),
            //        Month = i.Month.ToString(),
            //        NewCount = i.NewLicenseCount,
            //        ReNewCount = i.ReNewLicenseCount
            //    }).OrderBy(i => i.Month).ToList();
            var CurrItem = tradingLicenseStatisitcsInfos.Where(i => i.LicenseYear.ToString() == (DateTime.Now.Year).ToString())
               .Select(i => new Dtos.EserviceStats
               {
                   Year = i.LicenseYear.ToString(),
                   Month = i.Month.ToString(),
                   NewCount = i.NewLicenseCount,
                   ReNewCount = i.ReNewLicenseCount
               }).OrderBy(i => i.Month).ToList();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    //  new OutputDictionary(OperationOutputKeys.PrevEserviceStatsEntity, PrevItem),
                    new OutputDictionary(OperationOutputKeys.CurrEserviceStatsEntity, CurrItem));
        }
    }
}
