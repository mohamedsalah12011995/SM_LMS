
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using RM.OpenData.UnitOfWorks;
using RM.Core.Services;
using RM.Core.Integrations;
using RM.OpenData.Dtos;
using static RM.OpenData.Dtos.OperationOutput;
using RM.Core.Helpers;
using RM.Core.Consts;
using Mapster;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Extensions.Caching.Memory;
using System.IO.Compression;
using RM.Core.CommonDtos;
using PuppeteerSharp.Media;
using PuppeteerSharp;


namespace RM.OpenData.Services
{
    public class OpenDataService:BaseService,IOpenDataService
    {
        private readonly IUnitOfWork _unitOfWork;
        private static string HtmlOpenDataReportUrl = string.Empty;

        public OpenDataService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor,unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            try { HtmlOpenDataReportUrl = _configuration.GetSection("AppSettings").GetSection("HtmlOpenDataReportUrl").Value; } catch { }

        }

        public async Task<OperationOutput> RequestOpenData(Dtos.OpenDataRequest RequestedData)
        {
            Models.OpenDataRequest DbItem = new Models.OpenDataRequest();

            if (UseCapcha)
                if (string.IsNullOrEmpty(RequestedData.Capcha) || !GoogleCapcha.CheckCapchaSession(CapchaSecret, RequestedData.Capcha))
                    return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.Id.HasValue)
            {
                DbItem = _unitOfWork.OpenDataRequest.GetById(RequestedData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.OpenDataRequest.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
                _unitOfWork.OpenDataRequest.Add(DbItem);
            }

            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                        new OutputDictionary(OperationOutputKeys.OpenDataEntity, DbItem.Adapt<Dtos.OpenDataRequest>()));
        }
        public async Task<OperationOutput> OpenDataStats()
        {
            Integrations.OpenData openDataIntegration = new();
   
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var openDataStats = openDataIntegration.GetOpenDataStats();
            if (openDataStats==null || openDataStats.Count==0)
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKeys.OpenDataEntity, openDataStats));
        }


        public async Task< OperationOutput> GetOpenDataRequestList(Dtos.OpenDataRequest RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var result = _unitOfWork.OpenDataRequest.GetAll()
                .Where(RequestedData.Filteration())
                .OrderByDescending(x => x.CreatedDate ?? TransactionDate)
                .AsNoTracking().TakePaggination(RequestedData.Pagination, DefaultPaginationCount);

            var resultDto = result.Data.ToList().Adapt<List<Dtos.OpenDataRequest>>(Dtos.OpenDataRequest.SelectConfig());

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.OpenData_Request, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OpenDataEntity, resultDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, resultDto.Count > 0 ? resultDto[0].EntityId : ""),
                   new OutputDictionary(OperationOutputKeys.Pagination, result.Pagination));   
        }
        public async Task< OperationOutput> GetOpenDataRequestDetails(Dtos.OpenDataRequest RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

           var Item = await _unitOfWork.OpenDataRequest.GetAll()
                .Where(x => x.IsDeleted == false && x.Id == RequestedData.Id)
                .AsNoTracking().FirstOrDefaultAsync();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<Dtos.OpenDataRequest>(Dtos.OpenDataRequest.SelectConfig());

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, (int)Enums.Entities.OpenData_Request, Item.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OpenDataEntity, ItemDto));
        }

        public async Task<OperationOutput> GetOpenDataLookups(Dtos.OpenData RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Districts = _unitOfWork.MajorLookup.GetAll()
               .Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.District && x.ReferenceId == RequestedData.ReferenceId)
               .ToList().Adapt<List<Dtos.OpenDataLookups>>();

            var Types = _unitOfWork.MajorLookup.GetAll()
              .Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.OpenDataType && x.ParentId==null && x.ReferenceId == RequestedData.ReferenceId)
              .Select(x => new Dtos.OpenDataLookups()
              {
                  Id = x.Id,
                  MapId = x.MapId,
                  NameAr = x.NameAr,
                  NameEn = x.NameEn,
                  SubTypes = _unitOfWork.MajorLookup.GetAll()
                              .Where(s => s.ParentId == x.Id)
                              .ToList().Adapt<List<Dtos.OpenDataLookups>>()
              }).ToList();

            var SubTypes = _unitOfWork.MajorLookup.GetAll()
              .Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.OpenDataType && x.ParentId != null && x.ReferenceId == RequestedData.ReferenceId)
               .ToList().Adapt<List<Dtos.OpenDataLookups>>();


            DateTimeFormatInfo DTFormat = new System.Globalization.CultureInfo("ar-SA", false).DateTimeFormat;

            var Years = Enumerable.Range(1441, DTFormat.Calendar.GetYear(DateTime.Now) - 1440).ToList();

            var GregorianYears = Enumerable.Range(2019, DateTime.Now.Year - 2018).ToList();

            var Months = Enumerable.Range(1, 12).ToList();


            var StatisticTypes = new List<Dtos.OpenDataLookups>
            {
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.View,NameAr=StatisticTypeString((int)Enums.OpenDataStatisticTypes.View, "ar"),NameEn=StatisticTypeString((int)Enums.OpenDataStatisticTypes.View, "en")},
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.PDF,NameAr=StatisticTypeString((int)Enums.OpenDataStatisticTypes.PDF, "ar"),NameEn=StatisticTypeString((int)Enums.OpenDataStatisticTypes.PDF, "en")},
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.Excel,NameAr = StatisticTypeString((int) Enums.OpenDataStatisticTypes.Excel, "ar"), NameEn = StatisticTypeString((int) Enums.OpenDataStatisticTypes.Excel, "en")},
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.CSV,NameAr = StatisticTypeString((int) Enums.OpenDataStatisticTypes.CSV, "ar"), NameEn = StatisticTypeString((int) Enums.OpenDataStatisticTypes.CSV, "en")},
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.XML,NameAr = StatisticTypeString((int) Enums.OpenDataStatisticTypes.XML, "ar"), NameEn = StatisticTypeString((int) Enums.OpenDataStatisticTypes.XML, "en")},
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.HTML,NameAr = StatisticTypeString((int) Enums.OpenDataStatisticTypes.HTML, "ar"), NameEn = StatisticTypeString((int) Enums.OpenDataStatisticTypes.HTML, "en")},
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.JSON,NameAr = StatisticTypeString((int) Enums.OpenDataStatisticTypes.JSON, "ar"), NameEn = StatisticTypeString((int) Enums.OpenDataStatisticTypes.JSON, "en")},
            };

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.OpenData_Management, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKeys.Districts, Districts),
                       new OutputDictionary(OperationOutputKeys.Types, Types),
                       new OutputDictionary(OperationOutputKeys.SubTypes, SubTypes),
                       new OutputDictionary(OperationOutputKeys.Years, Years),
                       new OutputDictionary(OperationOutputKeys.GregorianYears, GregorianYears),
                       new OutputDictionary(OperationOutputKeys.Months, Months),
                       new OutputDictionary(OperationOutputKeys.StatisticTypes, StatisticTypes));
        }

        public async Task<OperationOutput> GetOpenDataStatistics(Dtos.OpenData RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var OpenDataLastUpdateDate = _unitOfWork.MemoryCache.Get<DateTime?>("OpenDataLastUpdateDate");
            var LatestOpenDataGetDate = _unitOfWork.MemoryCache.Get<DateTime?>("LatestOpenDataGetDate");
            var Rescue = _unitOfWork.MemoryCache.Get<double?>("Rescue");
            var Smuggling = _unitOfWork.MemoryCache.Get<double?>("Smuggling");
            var Infiltrations = _unitOfWork.MemoryCache.Get<double?>("Infiltrations");
            var Confiscations = _unitOfWork.MemoryCache.Get<double?>("Confiscations");
            var Arrests = _unitOfWork.MemoryCache.Get<double?>("Arrests");

            if (OpenDataLastUpdateDate == null)
            {
                OpenDataLastUpdateDate = TransactionDate;
                _unitOfWork.MemoryCache.Set("OpenDataLastUpdateDate", OpenDataLastUpdateDate, DateTimeOffset.Now.AddDays(1));
            }

            if (LatestOpenDataGetDate == null || Rescue == null || Smuggling == null || Infiltrations == null || Confiscations == null || Arrests == null || OpenDataLastUpdateDate > LatestOpenDataGetDate)
            {
                 Rescue = _unitOfWork.OpenData.GetAll().Include(t => t.Type).Where(x => x.ReferenceId == RequestedData.ReferenceId && x.Type.ParentId == (int)Enums.OpenDataTypes.Rescue && x.IsGregorian != true).Sum(x => x.Value);
                 Smuggling = _unitOfWork.OpenData.GetAll().Include(t => t.Type).Where(x => x.ReferenceId == RequestedData.ReferenceId && x.Type.ParentId == (int)Enums.OpenDataTypes.Smuggling && x.IsGregorian != true).Sum(x => x.Value);
                 Infiltrations = _unitOfWork.OpenData.GetAll().Include(t => t.Type).Where(x => x.ReferenceId == RequestedData.ReferenceId && x.Type.ParentId == (int)Enums.OpenDataTypes.Infiltrations && x.IsGregorian != true).Sum(x => x.Value);
                 Confiscations = _unitOfWork.OpenData.GetAll().Include(t => t.Type).Where(x => x.ReferenceId == RequestedData.ReferenceId && x.Type.ParentId == (int)Enums.OpenDataTypes.Confiscations && x.IsGregorian != true).Sum(x => x.Value);
                 Arrests = _unitOfWork.OpenData.GetAll().Include(t => t.Type).Where(x => x.ReferenceId == RequestedData.ReferenceId && x.Type.ParentId == (int)Enums.OpenDataTypes.Arrests && x.IsGregorian != true).Sum(x => x.Value);

                _unitOfWork.MemoryCache.Set("Rescue", Rescue, DateTimeOffset.Now.AddDays(1));
                _unitOfWork.MemoryCache.Set("Smuggling", Smuggling, DateTimeOffset.Now.AddDays(1));
                _unitOfWork.MemoryCache.Set("Infiltrations", Infiltrations, DateTimeOffset.Now.AddDays(1));
                _unitOfWork.MemoryCache.Set("Confiscations", Confiscations, DateTimeOffset.Now.AddDays(1));
                _unitOfWork.MemoryCache.Set("Arrests", Arrests, DateTimeOffset.Now.AddDays(1));
                _unitOfWork.MemoryCache.Set("LatestOpenDataGetDate", TransactionDate, DateTimeOffset.Now.AddDays(1));
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKeys.Rescue, Rescue),
                       new OutputDictionary(OperationOutputKeys.Smuggling, Smuggling),
                       new OutputDictionary(OperationOutputKeys.Infiltrations, Infiltrations),
                       new OutputDictionary(OperationOutputKeys.Confiscations, Confiscations),
                       new OutputDictionary(OperationOutputKeys.Arrests, Arrests));
        }

        public async Task<OperationOutput> SaveOpenData(Dtos.OpenData RequestedData)
        {
            Models.OpenData DbItem;

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            DbItem = _unitOfWork.OpenData.GetAll().Where(x => x.ReferenceId==RequestedData.ReferenceId && x.Year == RequestedData.Year && x.Month == RequestedData.Month && x.DistrictId == RequestedData.DistrictId && x.TypeId == RequestedData.TypeId).FirstOrDefault();
            
            if (DbItem != null)
            {
                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.OpenData.Update(DbItem);
            }
            else
            {
                DbItem = new Models.OpenData();
                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
                _unitOfWork.OpenData.Add(DbItem);
            }

           await _unitOfWork.CompleteAsync();

            _unitOfWork.MemoryCache.Set("OpenDataLastUpdateDate", TransactionDate, DateTimeOffset.Now.AddDays(1));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OpenDataEntity, DbItem.Adapt<Dtos.OpenData>(Dtos.OpenData.SelectConfig())));
        }

        public async Task<OperationOutput> GetOpenDataList(Dtos.OpenData RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var result = _unitOfWork.OpenData.GetAll()
                .Where(RequestedData.Filteration())
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.ModifiedByNavigation)
                .Include(x => x.District)
                .Include(x => x.Type)
                .OrderByDescending(x => x.ModifiedDate ?? x.CreatedDate).ThenByDescending(x=>x.CreatedDate)
                .AsNoTracking().TakePaggination(RequestedData.Pagination, DefaultPaginationCount);

            var resultDto = result.Data.ToList().Adapt<List<Dtos.OpenData>>(Dtos.OpenData.SelectConfig());

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.OpenData_Management, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OpenDataEntity, resultDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, result.Pagination));
        }

        public async Task<OperationOutput> GetOpenDataGroupbyList(Dtos.OpenData RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var result = _unitOfWork.OpenData.GetAll()
                .Where(RequestedData.Filteration())
                .Include(x => x.District)
                .Include(x => x.Type)
                .OrderByDescending(x => x.Year).ThenBy(a => a.Month)
                .AsNoTracking().TakePaggination(RequestedData.Pagination,DefaultPaginationCount);

            var resultDto = result.Data.ToList().Adapt<List<Dtos.OpenData>>(Dtos.OpenData.SelectConfig());

            var GroupedItem = resultDto.GroupBy(x => new { x.Year, x.DistrictId, x.TypeId }, (key, group) => new
            {
                Year = key.Year,
                DistrictId = group.First().DistrictId,
                DistrictNameAr = group.First().DistrictNameAr,
                DistrictNameEn = group.First().DistrictNameEn != null ? group.First().DistrictNameEn : group.First().DistrictNameAr,
                TypeId = group.First().TypeId,
                TypeNameAr = group.First().TypeNameAr,
                TypeNameEn = group.First().TypeNameEn,
                SumValue = group.Sum(x => x.Value),
                Months = group.Select(x => new
                {
                    Month = x.Month,
                    Value = x.Value

                }).ToList(),
                IsGregorian = group.First().IsGregorian
            });

            await SaveOpenDataStatistics(RequestedData);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OpenDataEntity, GroupedItem),
                   new OutputDictionary(OperationOutputKeys.OpenDataEntityMonth, resultDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, result.Pagination));
        }

        public async Task<OperationOutput> GetOpenDataDetails(Dtos.OpenData RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var DbItem = await _unitOfWork.OpenData.GetAll()
                 .Where(x => x.Id == RequestedData.Id)
                 .AsNoTracking().FirstOrDefaultAsync();

            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = DbItem.Adapt<Dtos.OpenData>(Dtos.OpenData.SelectConfig());

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, ItemDto.ReferenceId, (int)Enums.Entities.OpenData_Management, ItemDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OpenDataEntity, ItemDto));
        }

        public async Task<OperationOutput> GetOpenDataByFiledDetails(Dtos.OpenData RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var DbItem = await _unitOfWork.OpenData.GetAll()
                .Where(x => x.Year == RequestedData.Year && x.Month==RequestedData.Month && x.DistrictId == RequestedData.DistrictId && x.TypeId == RequestedData.TypeId)
                .AsNoTracking().FirstOrDefaultAsync();

            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = DbItem.Adapt<Dtos.OpenData>(Dtos.OpenData.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OpenDataEntity, ItemDto));
        }

        public async Task<OperationOutput> DeleteOpenData(Dtos.OpenData RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.OpenData.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            _unitOfWork.OpenData.Delete(DbItem);
            _unitOfWork.Complete();
            _unitOfWork.MemoryCache.Set("OpenDataLastUpdateDate", TransactionDate, DateTimeOffset.Now.AddDays(1));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> ModelAction(Dtos.OpenDataRequest RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.OpenDataRequest.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            DbItem.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : DbItem.IsDeleted;

            DbItem.ModifiedBy = RequestOwner.Id;
            DbItem.ModifiedDate = DateTime.Now;
            _unitOfWork.OpenDataRequest.Update(DbItem);
            _unitOfWork.Complete();
            _unitOfWork.MemoryCache.Set("OpenDataLastUpdateDate", TransactionDate, DateTimeOffset.Now.AddDays(1));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetOpenDataSearchStatistics(Dtos.OpenDataSearchStatistics RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = _unitOfWork.OpenDataStatistics.FindAll(RequestedData.Filteration(), d => d.District, t => t.Type)
                 .OrderByDescending(x => x.CreatedDate)
                 .GroupBy(x => new { x.CreatedDate.Value.Date.Year, x.StatisticType, x.FromYear, x.ToYear, x.FromMonth, x.ToMonth, x.DistrictId, x.TypeId }, (key, group) => new
                  {
                     Year = key.Year,
                     StatisticType = key.StatisticType,
                     StatisticTypeAr = StatisticTypeString(key.StatisticType.Value,"ar"),
                     StatisticTypeEn = StatisticTypeString(key.StatisticType.Value,"en"),
                     FromYear = key.FromYear,
                     ToYear = key.ToYear,
                     FromMonth = key.FromMonth,
                     ToMonth = key.ToMonth,
                     DistrictId = key.DistrictId != null ? Accessor.Get<int?>(key.DistrictId) : null,
                     DistrictNameAr = group.First()?.District?.NameAr!=null? group.First()?.District?.NameAr:"جميع المناطق",
                     DistrictNameEn = group.First()?.District?.NameEn!=null?group.First()?.District?.NameEn:"All Districts",
                     TypeId = Accessor.Get<int?>(key.TypeId),
                     TypeNameAr = group.First()?.Type?.NameAr,
                     TypeNameEn = group.First()?.Type?.NameEn,
                     YearCount = group.Sum(x => x.Count),
                     Months = group.GroupBy(x => new { x.CreatedDate.Value.Date.Month, x.StatisticType, x.FromYear, x.ToYear, x.FromMonth, x.ToMonth, x.DistrictId, x.TypeId }, (key, group) => new
                        {
                           Month = key.Month,
                           StatisticType = key.StatisticType,
                           StatisticTypeAr = StatisticTypeString(key.StatisticType.Value, "ar"),
                           StatisticTypeEn = StatisticTypeString(key.StatisticType.Value, "en"),
                           FromYear = key.FromYear,
                           ToYear = key.ToYear,
                           FromMonth = key.FromMonth,
                           ToMonth = key.ToMonth,
                           DistrictId = key.DistrictId !=null?Accessor.Get<int?>(key.DistrictId):null,
                           DistrictNameAr = group.First()?.District?.NameAr != null ? group.First()?.District?.NameAr : "جميع المناطق",
                           DistrictNameEn = group.First()?.District?.NameEn != null ? group.First()?.District?.NameEn : "All Districts",
                           TypeId = Accessor.Get<int?>(key.TypeId),
                           TypeNameAr = group.First()?.Type?.NameAr,
                           TypeNameEn = group.First()?.Type?.NameEn,
                           MonthCount = group.Sum(x => x.Count),
                           Days = group.Select(x=> new 
                              {
                                Day = x.CreatedDate.Value.Day,
                                DistrictId = x.DistrictId != null ? Accessor.Get<int?>(x.DistrictId) : null,
                                TypeId = Accessor.Get<int?>(x.TypeId),
                                StatisticType =x.StatisticType,
                                StatisticTypeAr = StatisticTypeString(x.StatisticType.Value, "ar"),
                                StatisticTypeEn = StatisticTypeString(x.StatisticType.Value, "en"),
                                DistrictNameAr =x.District?.NameAr!=null? x.District?.NameAr: "جميع المناطق",
                                DistrictNameEn=x.District?.NameEn!=null? x.District?.NameEn: "All Districts",
                                TypeNameAr=x.Type?.NameAr,
                                TypeNameEn=x.Type?.NameEn,
                                FromYear=x.FromYear,
                                ToYear=x.ToYear,
                                FromMonth = x.FromMonth,
                                ToMonth = x.ToMonth,
                                CreatedDate =x.CreatedDate,
                                CreatedDateString = x.CreatedDate.HasValue ? x.CreatedDate.Value.ToString("yyyy-MM-dd") : null,
                                DayCount =x.Count
                              }).ToList()
                        })
                  });

            var groupedItem = Item.GroupBy(x => new { x.Year }, (key, group) => new
            {
                Year = key.Year,
                TotalYearCount = group.Sum(x => x.YearCount),
                Result = group.SelectMany(x=>x.Months).ToList()           
            });

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.OpenDataStatistics, Item),
                   new OutputDictionary("GroupedOpenDataStatistics", groupedItem),
                   new OutputDictionary("FromDate", RequestedData.FromCreatedDate != null ? RequestedData.FromCreatedDate.Value.ToString("yyyy-MM-dd") :string.Empty),
                   new OutputDictionary("ToDate", RequestedData.ToCreatedDate != null ? RequestedData.ToCreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty),
                   new OutputDictionary("StatisticTypeAr", StatisticTypeString(RequestedData.StatisticTypeId.Value, "ar")),
                   new OutputDictionary("StatisticTypeEn", StatisticTypeString(RequestedData.StatisticTypeId.Value, "en")));
        }

        public async Task<OperationOutput> GetOpenDataSearchStatisticsReport(OpenDataSearchStatistics RequestedData)
        {
            OperationOutput Result = _unitOfWork.MemoryCache.Get<OperationOutput>(RequestedData.statisticTypeId);
            if (Result != null) 
            return Result;

            return await GetOpenDataSearchStatistics(RequestedData);
        }

        private string StatisticTypeString(int StatisticType,string lang)
        {
            string StatisticTypeString = "";
            switch (StatisticType)
            {
                case (int) Enums.OpenDataStatisticTypes.View:{ StatisticTypeString = lang == "ar" ? "تصفح البيانات" : "View Data";break;}
                case (int) Enums.OpenDataStatisticTypes.PDF:{ StatisticTypeString = lang == "ar" ? "تصدير PDF" : "Export PDF";break;}
                case (int) Enums.OpenDataStatisticTypes.HTML:{StatisticTypeString = lang == "ar" ? "تصدير Excel" : "Export Excel";break;}
                case (int) Enums.OpenDataStatisticTypes.CSV:{StatisticTypeString = lang == "ar" ? "تصدير CSV" : "Export CSV";break;}
                case (int) Enums.OpenDataStatisticTypes.XML:{ StatisticTypeString = lang == "ar" ? "تصدير XML" : "Export XML"; break;}
                case (int) Enums.OpenDataStatisticTypes.Excel:{ StatisticTypeString = lang == "ar" ? "تصدير HTML" : "Export HTML"; break;}
                case (int) Enums.OpenDataStatisticTypes.JSON:{ StatisticTypeString = lang == "ar" ? "تصدير JSON" : "Export JSON";break;}
                default:{StatisticTypeString = lang == "ar" ? "" : "";break;}
            }
            return StatisticTypeString;
        }

        public async Task SaveOpenDataStatistics(Dtos.OpenData RequestedData)
        {
            var dbItem = _unitOfWork.OpenDataStatistics.GetAll().Where(x => x.ReferenceId == RequestedData.ReferenceId && x.StatisticType==RequestedData.StatisticType && x.FromYear == RequestedData.FromYear && x.ToYear == RequestedData.ToYear && x.DistrictId == RequestedData.DistrictId && x.TypeId == RequestedData.ParentTypeId && x.FromMonth==RequestedData.FromMonth && x.ToMonth==RequestedData.ToMonth && x.CreatedDate.Value.Date==TransactionDate.Date).FirstOrDefault();
            if (dbItem != null)
            {
                dbItem.Count += 1;
                _unitOfWork.OpenDataStatistics.Update(dbItem);
            }
            else
            {
                dbItem = new Models.OpenDataStatistics();
                dbItem.ReferenceId = RequestedData.ReferenceId;
                dbItem.StatisticType = RequestedData.StatisticType;
                dbItem.DistrictId = RequestedData.DistrictId;
                dbItem.TypeId = RequestedData.ParentTypeId;
                dbItem.FromYear = RequestedData.FromYear;
                dbItem.ToYear = RequestedData.ToYear;
                dbItem.FromMonth = RequestedData.FromMonth;
                dbItem.ToMonth = RequestedData.ToMonth;
                dbItem.CreatedDate = TransactionDate;
                dbItem.IsGregorian= RequestedData.IsGregorian;
                dbItem.Count = 1;
                _unitOfWork.OpenDataStatistics.Add(dbItem);
            }
            _unitOfWork.Complete();
        }



        #region SendEmail

        public async Task<OperationOutput> CronJobSendReportsByEmail(CronJobRecord cron)
        {
            OperationOutput Result = new OperationOutput();
            Dtos.OpenDataSearchStatistics RequestedData = new Dtos.OpenDataSearchStatistics();
            var Emails = _unitOfWork.CronSettings.GetAll()
                  .Where(x => x.EntityId == (int) Enums.Entities.OpenData_Management)
                  .Where(x => x.IsActive == true && x.CronTypeId == cron.CronTypeId)
                  .AsNoTracking().Select(x => x.Emails).ToList();

            RequestedData.Emails = Emails.SelectMany(x => Strings.ConvertStringToList(x, "$")).ToList();

            if (cron.CronTypeId == (int)Enums.CronType.EveryDay)
                RequestedData.FromCreatedDate = TransactionDate.Date.AddDays(-1);
            else if (cron.CronTypeId == (int)Enums.CronType.EveryWeek)
                RequestedData.FromCreatedDate = TransactionDate.Date.AddDays(-7);
            else if (cron.CronTypeId == (int)Enums.CronType.EveryMonth)
                RequestedData.FromCreatedDate = TransactionDate.Date.AddMonths(-1);
            else if (cron.CronTypeId == (int)Enums.CronType.EveryQuaters)
                RequestedData.FromCreatedDate = TransactionDate.Date.AddMonths(-3);
            else RequestedData.FromCreatedDate = TransactionDate;

            RequestedData.ToCreatedDate = TransactionDate;
            
            await SendEmailOpenDataStatistics(RequestedData);

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }

        public async Task<OperationOutput> SendEmailOpenDataStatistics(OpenDataSearchStatistics RequestedData)
        {
            OperationOutput Result = new OperationOutput();

            List<(byte[], string)> filesAr = new List<(byte[], string)>();
            List<(byte[], string)> filesEn = new List<(byte[], string)>();

            var StatisticTypes = new List<Dtos.OpenDataLookups>
            {
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.View,NameAr=StatisticTypeString((int)Enums.OpenDataStatisticTypes.View, "ar"),NameEn=StatisticTypeString((int)Enums.OpenDataStatisticTypes.View, "en")},
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.PDF,NameAr=StatisticTypeString((int)Enums.OpenDataStatisticTypes.PDF, "ar"),NameEn=StatisticTypeString((int)Enums.OpenDataStatisticTypes.PDF, "en")},
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.Excel,NameAr = StatisticTypeString((int) Enums.OpenDataStatisticTypes.Excel, "ar"), NameEn = StatisticTypeString((int) Enums.OpenDataStatisticTypes.Excel, "en")},
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.CSV,NameAr = StatisticTypeString((int) Enums.OpenDataStatisticTypes.CSV, "ar"), NameEn = StatisticTypeString((int) Enums.OpenDataStatisticTypes.CSV, "en")},
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.XML,NameAr = StatisticTypeString((int) Enums.OpenDataStatisticTypes.XML, "ar"), NameEn = StatisticTypeString((int) Enums.OpenDataStatisticTypes.XML, "en")},
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.HTML,NameAr = StatisticTypeString((int) Enums.OpenDataStatisticTypes.HTML, "ar"), NameEn = StatisticTypeString((int) Enums.OpenDataStatisticTypes.HTML, "en")},
                new Dtos.OpenDataLookups {Id=(int)Enums.OpenDataStatisticTypes.JSON,NameAr = StatisticTypeString((int) Enums.OpenDataStatisticTypes.JSON, "ar"), NameEn = StatisticTypeString((int) Enums.OpenDataStatisticTypes.JSON, "en")},
            };

          //  var Districts = _unitOfWork.MajorLookup.GetAll().Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.District).ToList();

            foreach (var st in StatisticTypes)
            {
                RequestedData.StatisticTypeId = st.Id;
                var data = await GetOpenDataSearchStatistics(RequestedData);
                _unitOfWork.MemoryCache.Set(RequestedData.statisticTypeId, data, DateTimeOffset.Now.AddSeconds(30));

                var urlAr = HtmlOpenDataReportUrl + "/ar/" + RequestedData.statisticTypeId + "?token=" + Token.Replace("bearer ", "");
                var urlEn = HtmlOpenDataReportUrl + "/en/" + RequestedData.statisticTypeId + "?token=" + Token.Replace("bearer ", "");

                var pdfOption = new PdfOptions
                {
                    Format = PaperFormat.Tabloid,
                    PrintBackground = true,
                    Landscape = true
                };
                var pdffFileAr = await PDF.GeneratePdfFromUrlAsync(urlAr, PDFServiceUrl, pdfOption, Token);
                var pdffFileEn = await PDF.GeneratePdfFromUrlAsync(urlEn, PDFServiceUrl, pdfOption, Token);

                filesAr.Add((pdffFileAr, st.NameAr.Replace(" ", "_") + ".pdf"));
                filesEn.Add((pdffFileEn, st.NameEn.Replace(" ", "_") + ".pdf"));

            }

            var compressAr = Files.CompressToZip(filesAr);
            var compressEn = Files.CompressToZip(filesEn);

            RequestedData.Subject = "Open Data Search Statistics";
            RequestedData.FileName = "OpenDataSearchStatistics";
            foreach (var email in RequestedData.Emails)
            {
                if (compressAr.Length < 7000000)
                {
                    await Email.SendEmailAsync(email, RequestedData.Subject, RequestedData.Body, EmailServiceUrl, Token, null, new List<EmailAttachment> { new EmailAttachment { FileName = RequestedData.FileName + "_Ar.zip", FileBytes = compressAr }, new EmailAttachment { FileName = RequestedData.FileName + "_En.zip", FileBytes = compressEn } });
                    Thread.Sleep(500);
                }
                else
                {
                    await Email.SendEmailAsync(email, RequestedData.Subject, RequestedData.Body, EmailServiceUrl, Token, null, new List<EmailAttachment> { new EmailAttachment { FileName = RequestedData.FileName + "_Ar.zip", FileBytes = compressAr } });
                    Thread.Sleep(500);

                    await Email.SendEmailAsync(email, RequestedData.Subject, RequestedData.Body, EmailServiceUrl, Token, null, new List<EmailAttachment> { new EmailAttachment { FileName = RequestedData.FileName + "_En.zip", FileBytes = compressEn } });
                    Thread.Sleep(500);
                }
            }

            Result.Header = ApplicationOperation.OperationResult(Enums.ServiceMessages.TransactionSuccess);
            return Result;
        }

        #endregion


    }
}
