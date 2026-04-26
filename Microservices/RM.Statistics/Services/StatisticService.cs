using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Statistics.Dtos;
using RM.Statistics.UnitOfWorks;
using System.Data;

using static RM.Statistics.Dtos.OperationOutput;

namespace RM.Statistics.Services
{
    public class StatisticService : BaseService, IStatisticService
    {

        private readonly IUnitOfWork _unitOfWork;

        public StatisticService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetEntitiesStatistics(Dtos.Statistics RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Dtos.Statistics item = new();

            var lastUpdate = GetLatestUpdateEntityDate(RequestedData.ReferenceId, RequestedData.EntityId, RequestedData.Id);

            item.CommentsCount = await GetCommentsCount(RequestedData);

            var ItemInteraction = await GetInteractionStatistics(RequestedData, Enums.InteractionStatisticsType.ViewsCount);

            item.ViewsCount = ItemInteraction == null ? 0 : ItemInteraction.Value ?? 0;
            item.HelpfulCount = ItemInteraction == null ? 0 : ItemInteraction.IsHelpfulCount.HasValue ? ItemInteraction.IsHelpfulCount.Value : 0;
            item.NotHelpfulCount = ItemInteraction == null ? 0 : ItemInteraction.NotHelpfulCount.HasValue ? ItemInteraction.NotHelpfulCount.Value : 0;

            var RatingCal = await GetRatingCalculation(RequestedData);

            item.RatingCount = RatingCal.Item1;
            item.RatingValue = RatingCal.Item2;

            item.LatestUpdate = lastUpdate;
            item.LatestUpdateString = lastUpdate.HasValue ? lastUpdate.Value.ToString("yyyy-MM-dd") : null;

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.Statistics, item));

        }

        public OperationOutput GetPortalLatestUpdate(Dtos.Statistics RequestedData)
        {
            return GetEntitiesLatestUpdate(RequestedData.ReferenceId);
        }

        public async Task<OperationOutput> SaveInteractionStatistics(int? ReferenceId, int? EntityId, int? ItemId, int? StatisticsType, string ItemUrl = null,int? value = null)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var item = await _unitOfWork.InteractionStatistic.GetAll()
                  .FirstOrDefaultAsync(x => x.InteractionStatisticsType == StatisticsType
                   && (EntityId.HasValue ? x.EntityId == EntityId : x.EntityId == null)
                   && (ItemId.HasValue ? x.ItemId == ItemId : x.ItemId == null)
                   && (!string.IsNullOrEmpty(ItemUrl) ? x.ItemUrl == ItemUrl : true)
                   && x.ReferenceId == ReferenceId && x.CreatedDate.Value.Date == TransactionDate.Date);

            if (value is null) value = 1;

            if (item is null)
            {
                item = new Models.InteractionStatistic();
                item.ItemId = ItemId.HasValue ? ItemId.Value : null;
                item.ItemUrl = ItemUrl;
                item.EntityId = EntityId;
                item.ReferenceId = ReferenceId;
                item.InteractionStatisticsType = StatisticsType;
                item.Value = value;
                item.CreatedDate = TransactionDate.Date;
                _unitOfWork.InteractionStatistic.Add(item);
            }
            else
            {
                if(StatisticsType == (int) Enums.InteractionStatisticsType.TimeSpend)
                    item.Value = item.Value == null ? value : (item.Value + value)/2;
                else 
                    item.Value = item.Value == null ? value : item.Value + value;

                _unitOfWork.InteractionStatistic.Update(item);
            }

            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
              new OutputDictionary(OperationOutputKeys.VisitorId, StatisticsType == (int) Enums.InteractionStatisticsType.NewVisitor ? Guid.NewGuid().ToString():null));
        }


        public OperationOutput GetEntitiesLatestUpdate(int? ReferenceId)
        {
            List<EntitiesLatestUpdate> itemList = GetEntitiesLatest(ReferenceId, null);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
              new OutputDictionary(OperationOutputKeys.EntitiesLatestUpdate, itemList));
        }

        #region HELPER METHODS   
        private List<EntitiesLatestUpdate> GetEntitiesLatest(int? ReferenceId, int? Id)
        {
            string ProcedureName = "sp_EntitiesLatestUpdate";
            var parameters = new[] {
                new SqlParameter("@Id", SqlDbType.Int) {Direction = ParameterDirection.Input, Value = Id.HasValue?Id:DBNull.Value },
                new SqlParameter("@ReferenceId", SqlDbType.Int) { Direction = ParameterDirection.Input, Value =ReferenceId },
            };
            var ProcedureParameters = " @Id,@ReferenceId";

            var itemList = _unitOfWork.GetFromProcdure(ProcedureName, ProcedureParameters, parameters).Select(x => new Dtos.EntitiesLatestUpdate
            {
                EntityId = x.EntityId,
                Id = x.ItemId,
                LastUpdate = x.LastUpdate,
                NameAr = x.NameAr,
                NameEn = x.NameEn,
            }).ToList();
            return itemList;
        }

        private async Task<(int, double)> GetRatingCalculation(Dtos.Statistics RequestedData)
        {
            List<int> ItemListOfRates = null;
            int CountOfRating = 0;
            double RatingCalculation = 0;

            ItemListOfRates = await _unitOfWork.Rates.GetAll().AsNoTracking()
            .Where(x => x.EntityId == RequestedData.EntityId
                  && (RequestedData.Id.HasValue ? x.ItemId == RequestedData.Id : x.ItemId == null)
                  && (!string.IsNullOrEmpty(RequestedData.ItemUrl) ? x.ItemUrl == RequestedData.ItemUrl : true)
                  && x.ReferenceId == RequestedData.ReferenceId).Select(x => x.Value.Value).ToListAsync<int>();

            CountOfRating = ItemListOfRates.Count;
            foreach (var calc in ItemListOfRates.GroupBy(x => x).ToList())
            {
                RatingCalculation += calc.Key * calc.Count();
            }

            RatingCalculation = CountOfRating != 0 ? Math.Round(RatingCalculation / CountOfRating) : 0;

            return (CountOfRating, RatingCalculation);

        }

        private async Task<int> GetCommentsCount(Dtos.Statistics RequestedData)
        {
            return await _unitOfWork.Comments.GetAll().AsNoTracking()
                .CountAsync(x => x.EntityId == RequestedData.EntityId && x.ReferenceId == RequestedData.ReferenceId
                 && (RequestedData.Id.HasValue ? x.ItemId == RequestedData.Id : x.ItemId == null)
                 && (!string.IsNullOrEmpty(RequestedData.ItemUrl) ? x.ItemUrl == RequestedData.ItemUrl : x.ItemUrl == null));


        }
        private async Task<Models.InteractionStatistic> GetInteractionStatistics(Dtos.Statistics RequestedData, Enums.InteractionStatisticsType Type)
        {
            var result = await _unitOfWork.InteractionStatistic.GetAll().AsNoTracking()
                         .Where(x => x.InteractionStatisticsType == (int)Type
                         && x.EntityId == RequestedData.EntityId
                         && (RequestedData.Id.HasValue ? x.ItemId == RequestedData.Id : x.ItemId == null)
                         && (!string.IsNullOrEmpty(RequestedData.ItemUrl) ? x.ItemUrl == RequestedData.ItemUrl : true)
                         && x.ReferenceId == RequestedData.ReferenceId)
                         .GroupBy(x=> new { x.CreatedDate }, (key, group) => new Models.InteractionStatistic
                         {
                              Id = group.First().Id,
                              EntityId = group.First().EntityId,
                              InteractionStatisticsType = group.First().InteractionStatisticsType,
                              IsHelpfulCount = group.Sum(x=> x.IsHelpfulCount),
                              ItemId = group.First().ItemId,
                              ItemUrl = group.First().ItemUrl,
                              ReferenceId = group.First().ReferenceId,
                              NotHelpfulCount = group.Sum(x=> x.IsHelpfulCount),
                              ReferenceMajorId = group.First().ReferenceMajorId,
                              Value = group.Sum(x => x.Value)
                         }).FirstOrDefaultAsync();

            return result;
        }
        private DateTime? GetLatestUpdateEntityDate(int? ReferenceId, int? EntityId, int? Id)
        {
            DateTime? latestUpdate = DateTime.Now;

            List<EntitiesLatestUpdate> itemList = GetEntitiesLatest(ReferenceId, Id);

            if (EntityId.HasValue)
            {
                latestUpdate = itemList.Where(x => x.EntityId == EntityId).FirstOrDefault() != null ? itemList.Where(x => x.EntityId == EntityId).FirstOrDefault().LastUpdate : TransactionDate;
            }
            return latestUpdate;

        }

        #endregion


        public async Task<OperationOutput> SaveIsHelpful(int? ReferenceId, int? EntityId, int? ItemId, bool isHelpful, string ItemUrl = null)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            bool isNew = false;

            var item = await _unitOfWork.InteractionStatistic.GetAll()
                 .FirstOrDefaultAsync(x => x.EntityId == EntityId
                 && (ItemId.HasValue ? x.ItemId == ItemId : x.ItemId == null)
                 && (!string.IsNullOrEmpty(ItemUrl) ? x.ItemUrl == ItemUrl : true)
                 && x.ReferenceId == ReferenceId && x.CreatedDate.Value.Date == TransactionDate.Date);

            if (item is null)
            {
                isNew = true;
                item = new Models.InteractionStatistic();
                item.ItemId = ItemId.HasValue ? ItemId.Value : null;
                item.ItemUrl = ItemUrl;
                item.EntityId = EntityId;
                item.ReferenceId = ReferenceId;
                item.InteractionStatisticsType = (int)Enums.InteractionStatisticsType.ViewsCount;
                item.CreatedDate = TransactionDate.Date;

                if (isHelpful)
                {
                    item.IsHelpfulCount = 1;
                    item.NotHelpfulCount = 0;
                }
                else
                {
                    item.IsHelpfulCount = 0;
                    item.NotHelpfulCount = 1;
                }

            }
            else
            {
                if (isHelpful)
                    _ = item.IsHelpfulCount == null ? item.IsHelpfulCount = 1 : item.IsHelpfulCount += 1;

                else
                    _ = item.NotHelpfulCount == null ? item.NotHelpfulCount = 1 : item.NotHelpfulCount += 1;
            }

            if (isNew) _unitOfWork.InteractionStatistic.Add(item);
            else _unitOfWork.InteractionStatistic.Update(item);

            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> GetTotalStatistics(Dtos.Statistics RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var totalGeneralNumber = GetTotalGeneralNumbers(RequestedData);

            var EntityTotalVisitorCount = GetTotalVisitedByEntitis(RequestedData);

            var mostVisitedArticals = MostVisitedArticals(RequestedData);
            var lowestVisitedArticals = LowestVisitedArticals(RequestedData);

            var mostRatedArticals = MostRatedArticals(RequestedData);

            var lowestRatedArticals = LowestRatedArticals(RequestedData);

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, RequestedData.ReferenceId, (int)Enums.Entities.Statistics, null, Enums.InteractionStatisticsType.ViewsCount, null);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.TotalGeneralNumber, totalGeneralNumber),
             new OutputDictionary(OperationOutputKeys.EntityTotalVisitorCount, EntityTotalVisitorCount),
             new OutputDictionary(OperationOutputKeys.MostVisitedArticals, mostVisitedArticals),
             new OutputDictionary(OperationOutputKeys.LowestVisitedArticals, lowestVisitedArticals),
             new OutputDictionary(OperationOutputKeys.MostRatedArticals, mostRatedArticals),
             new OutputDictionary(OperationOutputKeys.LowestRatedArticals, lowestRatedArticals));
        }


        #region HELPER METHODS
        private List<Dtos.StatisticsResult> LowestRatedArticals(Dtos.Statistics RequestedData)
        {
            return MapResultRatesFromStordProcedure("sp_LowestRatedArticles", RequestedData);

        }

        private Dtos.GeneralNumbersResults GetTotalGeneralNumbers(Dtos.Statistics RequestedData)
        {
            List<Tuple<string, object>> Params = new List<Tuple<string, object>>();

            Params.Add(new Tuple<string, object>("referenceId", RequestedData.ReferenceId.HasValue ? RequestedData.ReferenceId : DBNull.Value));

            var GeneralNumbersResults = _unitOfWork.GeneralNumbersResult.QueryProcedure("sp_TotalGenralNumbers", Params).ToList()
                .Select(x => new Dtos.GeneralNumbersResults
                {
                    TotalVisit = x.totalVisit ?? 0,
                    TotalComment = x.totalComment ?? 0,
                    TotalRates = x.totalRates ?? 0

                }).FirstOrDefault();

            return GeneralNumbersResults;
        }

        private List<Dtos.TotalVisitedByEntityResult> GetTotalVisitedByEntitis(Dtos.Statistics RequestedData)
        {
            List<Tuple<string, object>> Params = new List<Tuple<string, object>>();
            Params.Add(new Tuple<string, object>("referenceId", RequestedData.ReferenceId.HasValue ? RequestedData.ReferenceId : DBNull.Value));


            var TotalVisitedByEntityResult = _unitOfWork.TotalVisitedByEntityResult.QueryProcedure("sp_TotalVisitedByEntiteis", Params).ToList()
                 .Select(x => new Dtos.TotalVisitedByEntityResult()
                 {
                     EntityName = x.NameAr,
                     EntityNameEn = x.NameEn,
                     ViewsCount = x._value ?? 0
                 }).ToList();

            return TotalVisitedByEntityResult;
        }

        private List<Dtos.StatisticsResult> MostRatedArticals(Dtos.Statistics RequestedData)
        {
            return MapResultRatesFromStordProcedure("sp_TopRatedArticles", RequestedData);
        }


        private List<Dtos.StatisticsResult> MostVisitedArticals(Dtos.Statistics RequestedData)
        {

            return MapResultInteractionsFromStordProcedure("sp_MostVisitedArticles", RequestedData);
        }

        private List<Dtos.StatisticsResult> LowestVisitedArticals(Dtos.Statistics RequestedData)
        {

            return MapResultInteractionsFromStordProcedure("sp_LowestVisitedArticles", RequestedData);
        }

        List<Dtos.StatisticsResult> MapResultInteractionsFromStordProcedure(string ProcedureName, Dtos.Statistics RequestedData)
        {
            List<Dtos.StatisticsResult> StatisticsResult = null;
            List<Tuple<string, object>> Params = new List<Tuple<string, object>>();
            Params.Add(new Tuple<string, object>("referenceId", RequestedData.ReferenceId.HasValue ? RequestedData.ReferenceId : DBNull.Value));


            StatisticsResult = _unitOfWork.StatisticsResult.QueryProcedure(ProcedureName, Params).ToList()
            .Select(x => new Dtos.StatisticsResult()
            {
                ItemId = x.ItemId,
                ArticleNameAr = x.NameAr,
                ArticleNameEn = x.NameEn,
                FrontIdentity = x.FrontIdentity,
                ViewsCount = x._value ?? 0,
                Url = $"{x.FrontIdentity}/"
            }).ToList();

            return StatisticsResult;
        }

        List<Dtos.StatisticsResult> MapResultRatesFromStordProcedure(string ProcedureName, Dtos.Statistics RequestedData)
        {
            List<Dtos.StatisticsResult> StatisticsResult = null;
            List<Tuple<string, object>> Params = new List<Tuple<string, object>>();
            Params.Add(new Tuple<string, object>("referenceId", RequestedData.ReferenceId.HasValue ? RequestedData.ReferenceId : DBNull.Value));


            StatisticsResult = _unitOfWork.StatisticsResult.QueryProcedure(ProcedureName, Params).ToList()
            .Select(x => new Dtos.StatisticsResult()
            {
                ItemId = x.ItemId,
                ArticleNameAr = x.NameAr,
                ArticleNameEn = x.NameEn,
                FrontIdentity = x.FrontIdentity,
                RatingValue = x._value ?? 0,
                Url = $"{x.FrontIdentity}/"
            }).ToList();

            return StatisticsResult;
        }

        #endregion


    }
}
