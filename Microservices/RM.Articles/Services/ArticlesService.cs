

using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Articles.Dtos;
using RM.Core.Extensions;
using RM.Articles.UnitOfWorks;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using RM.Models;
using static RM.Articles.Dtos.OperationOutput;
using Common;
using Microsoft.Extensions.Caching.Distributed;
using static RM.Core.Helpers.Enums;
using System.Text.Json;

namespace RM.Articles.Services
{
    public class ArticlesService:BaseService, IArticlesService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ArticlesService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, RedisConfiguration RedisConfiguration, IDistributedCache redisCache)
            : base(httpContextAccessor,unitOfWork.Configuration, RedisConfiguration, redisCache)
            {
                 _unitOfWork = unitOfWork;
            }

        public async Task<OperationOutput> GetArticlesList(Dtos.Articles RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            var articles = await _unitOfWork.Articles.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending, x => x.CreatedByNavigation, x => x.UpdatedByNavigation);
                
            var articlesDto = articles.Data.Adapt<List<Dtos.Articles>>(Dtos.Articles.SelectConfig(IsLocal, GetPath, IntranetGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.Articles, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ArticlesEntity, articlesDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Articles)),
                   new OutputDictionary(OperationOutputKeys.Pagination, articles.Pagination));
        }

        public async Task<OperationOutput> GetArticleById(int Id, int? specificEntityId = null, string itemUrl = null)
        {

            Article DbItem = new Article();
            Dtos.Articles Item = new Dtos.Articles();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            DbItem = _unitOfWork.Articles.GetAll()
                .Include(x => x.Entity)
                .Where(x => x.Id == Id && x.IsDeleted!=true)
                .Where(x => (IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser) ? x.IsActive == true : true)
                .AsNoTracking().FirstOrDefault();

            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Item=DbItem.Adapt<Dtos.Articles>(Dtos.Articles.SelectConfig(IsLocal, GetPath, IntranetGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, specificEntityId != null ? specificEntityId : (int)Enums.Entities.Articles, specificEntityId != null ? null : Item.Id, Enums.InteractionStatisticsType.ViewsCount, itemUrl);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ArticlesEntity, Item),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Articles)));      
        }

        public async Task<OperationOutput> GetArticleDetails(Dtos.Articles RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            if (_RedisConfiguration.UseRedis)
            {
                DateTime LastUpdate;
                DateTime.TryParse(await RedisCache.GetStringAsync("Article_LastUpdate"), out LastUpdate);

                var CacheDate = await RedisCache.GetStringAsync("Article_CacheDate");
                var Article = await RedisCache.GetStringAsync("Article_" + RequestedData.ID);
                if (Article == null || CacheDate == null || DateTime.Parse(CacheDate) < LastUpdate)
                {
                    Result = await GetArticleById(RequestedData.Id.Value, RequestedData.SpecificEntityId, RequestedData.ItemUrl);
                    if (Result.Header.Success)
                    {
                        await RedisCache.SetStringAsync("Article_" + RequestedData.ID, JsonSerializer.Serialize(Result, jsonOptions), _RedisConfiguration.RedisOptions);
                        await RedisCache.SetStringAsync("Article_CacheDate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
                    }
                    return Result;
                }

                Result = JsonSerializer.Deserialize<OperationOutput>(Article, jsonOptions);
                return Result;
            }
            else
            {
                return await GetArticleById(RequestedData.Id.Value, RequestedData.SpecificEntityId, RequestedData.ItemUrl);
            }

        }

        public async Task<OperationOutput> SaveArticle(Dtos.Articles RequestdData)
        {
            Article DbItem=new Article();
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestdData.Id.HasValue)
            {
                DbItem = await _unitOfWork.Articles.GetByIdAsync(RequestdData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestdData.Adapt(DbItem, RequestdData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Articles.Update(DbItem);
            }
            else
            {
                RequestdData.Adapt(DbItem, RequestdData.AddConfig(RequestOwner.Id));
                _unitOfWork.Articles.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();

            if (_RedisConfiguration.UseRedis)
                RedisCache.SetString("Article_LastUpdate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);

            return await GetArticleById(DbItem.Id);

        }

        public OperationOutput ModelActions(Dtos.Articles RequestedData)
        {
            Article DbItem ;
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            DbItem = _unitOfWork.Articles.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            DbItem.IsActive = RequestedData.IsActive.HasValue ? RequestedData.IsActive.Value : DbItem.IsActive;
            DbItem.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : DbItem.IsDeleted;

            if (RequestedData.IsDeleted.HasValue && RequestedData.IsDeleted.Value == true)
            {
                DbItem.DeletedBy = RequestOwner.Id;
                DbItem.DeletedDate = TransactionDate;
            }
            if (RequestedData.IsActive.HasValue && RequestedData.IsActive.Value == true)
            {
                DbItem.ActivatedBy = RequestOwner.Id;
                DbItem.ActivatedDate = TransactionDate;
            }
            DbItem.UpdatedBy = RequestOwner.Id;
            DbItem.UpdatedDate = DateTime.Now;

            _unitOfWork.Articles.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
    }
}
