using Common;
using Mapster;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Models;
using RM.News.Dtos;
using RM.News.Integrations;
using RM.News.UnitOfWorks;
using System.Data;

using System.Linq.Expressions;
using System.Text.Json;
using static RM.Core.Helpers.Cryptography;
using static RM.Core.Helpers.Enums;
using static RM.News.Dtos.OperationOutput;

namespace RM.News.Services
{
    public class NewsService : BaseService, INewsService
    {

        private string newsImagesSavePath = string.Empty;
        private string newsThumbsSavePath = string.Empty;
        private string newsImagesGetPath = string.Empty;
        private string newsThumbsGetPath = string.Empty;
        private readonly IUnitOfWork _unitOfWork;

        public NewsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, RedisConfiguration RedisConfiguration, IDistributedCache redisCache)
            : base(httpContextAccessor, unitOfWork.Configuration, RedisConfiguration, redisCache)
        {
            _unitOfWork = unitOfWork;
            SetNewsImagesPath();
        }

        #region HELPER METHOD >> CONSTRACTOR
        void SetNewsImagesPath()
        {
            newsImagesSavePath = DirPath + nameof(FilePathRootConst.images);
            newsThumbsSavePath = DirPath + nameof(FilePathRootConst.icons);
            newsImagesGetPath = newsThumbsGetPath = Strings.HandleGetResourcesPath(IsLocal, GetPath, IntranetGetPath);

        }
        #endregion


        public async Task<OperationOutput> GetReferences()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var references = await _unitOfWork.References.GetAll().Where(c => c.IsPortal == true && c.ParentId == null)
                .AsNoTracking().ToListAsync();

            var referencesDto = references.Adapt<List<Dtos.Reference>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
              new OutputDictionary(OperationOutputKeys.References, referencesDto));

        }

        public async Task<OperationOutput> GetNewsRedis(Dtos.NewsDto RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            if (_RedisConfiguration.UseRedis && RequestedData.IsHomePage == true)
            {
                DateTime LastUpdate;
                DateTime.TryParse(await RedisCache.GetStringAsync("News_LastUpdate"), out LastUpdate);

                var CacheDate = await RedisCache.GetStringAsync("News_Home_CacheDate");
                var News = await RedisCache.GetStringAsync("News_Home");
                if (News == null || CacheDate == null || DateTime.Parse(CacheDate) < LastUpdate)
                {
                    Result = await GetNews(RequestedData);
                    if (Result.Header.Success)
                    {
                        await RedisCache.SetStringAsync("News_Home", JsonSerializer.Serialize(Result, jsonOptions), _RedisConfiguration.RedisOptions);
                        await RedisCache.SetStringAsync("News_Home_CacheDate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
                    }
                    return Result;
                }

                Result = JsonSerializer.Deserialize<OperationOutput>(News, jsonOptions);
                return Result;
            }
            else
            {
                return await GetNews(RequestedData);
            }
        }
        public async Task<OperationOutput> GetNews(NewsDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            List<NewsDto> newsDto = null;
            ApplicationOperation.Pagination pagination = SetDefaultData();

            var publishedNews = _unitOfWork.PublishEntities.FindAll(c => c.EntityId == (int)Enums.Entities.News && c.ReferenceId == RequestedData.ReferenceId).ToList();
            var filteration = RequestedData.Filteration(publishedNews.Select(x => x.ItemId).ToList());

            if (!_RedisConfiguration.UseRedis && RequestedData.IsHomePage == true)
                newsDto = GetLatestNewsForPortal(filteration);

            else
            {
                var news = await _unitOfWork.News.GetAll().Where(filteration)
                    .Include(c => c.CreatedByNavigation).Include(u => u.UpdatedByNavigation)
                    .OrderByDescending(x => x.NewsDate).AsNoTracking().TakePagginationAsync(RequestedData.Pagination, DefaultPaginationCount);

                newsDto = news.Data.Adapt<List<NewsDto>>(NewsDto.SelectConfig(RequestUserRole, newsThumbsGetPath, newsImagesGetPath));
                pagination = news.Pagination;
            }


            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, RequestedData.ReferenceId, (int)Enums.Entities.News, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.NewsEntity, newsDto),
            new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.News)),
            new OutputDictionary(OperationOutputKeys.Pagination, pagination));

        }


        #region HELPER METHOD >> GetNews

        private List<NewsDto> GetLatestNewsForPortal(Expression<Func<Models.News, bool>> filteration)
        {
            var NewsLastUpdateDate = _unitOfWork.MemoryCache.Get<DateTime?>("NewsLastUpdateDate");
            var LatestNewsGetDate = _unitOfWork.MemoryCache.Get<DateTime?>("LatestNewsGetDate");
            var LatestNews = _unitOfWork.MemoryCache.Get<List<NewsDto>>("LatestNews");

            if (NewsLastUpdateDate == null)
            {
                NewsLastUpdateDate = TransactionDate;
                _unitOfWork.MemoryCache.Set("NewsLastUpdateDate", NewsLastUpdateDate, DateTimeOffset.Now.AddDays(1));
            }

            if (LatestNewsGetDate == null || LatestNews == null || NewsLastUpdateDate > LatestNewsGetDate)
            {
                    LatestNews = _unitOfWork.News.FindAll(filteration)
                                .OrderByDescending(o => o.NewsDate)
                                .Take(10).ToList()
                                .Adapt<List<NewsDto>>(NewsDto.SelectConfig(RequestUserRole, newsThumbsGetPath, newsImagesGetPath));

                    _unitOfWork.MemoryCache.Set("LatestNews", LatestNews, DateTimeOffset.Now.AddDays(1));
                    _unitOfWork.MemoryCache.Set("LatestNewsGetDate", TransactionDate, DateTimeOffset.Now.AddDays(1));
            }

            return LatestNews;
        }

        #endregion


        public async Task<OperationOutput> GetNewsByID(NewsDetailDto RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            if (_RedisConfiguration.UseRedis)
            {
                DateTime LastUpdate;
                DateTime.TryParse(await RedisCache.GetStringAsync("News_LastUpdate"), out LastUpdate);

                var CacheDate = await RedisCache.GetStringAsync("News_CacheDate");
                var News = await RedisCache.GetStringAsync("News_" + RequestedData.ID);
                if (News == null || CacheDate == null || DateTime.Parse(CacheDate) < LastUpdate)
                {
                    Result = await GetNewsById(RequestedData);
                    if (Result.Header.Success)
                    {
                        await RedisCache.SetStringAsync("News_" + RequestedData.ID, JsonSerializer.Serialize(Result, jsonOptions), _RedisConfiguration.RedisOptions);
                        await RedisCache.SetStringAsync("News_CacheDate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
                    }
                    return Result;
                }

                Result = JsonSerializer.Deserialize<OperationOutput>(News, jsonOptions);
                return Result;
            }
            else
            {
                return await GetNewsById(RequestedData);
            }
        }

        public async Task<OperationOutput> GetNewsById(NewsDetailDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var newsModel = await _unitOfWork.News.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == RequestedData.Id.Value);

            var publishReferences = await _unitOfWork.PublishEntities.GetAll()
                .Where(x => x.ItemId == RequestedData.Id.Value).AsNoTracking().ToListAsync();

            var newsDto = newsModel.Adapt<NewsDto>(NewsDto.SelectConfigNewsDetail(IsLocal, GetPath, IntranetGetPath
                                 , newsThumbsGetPath, newsImagesGetPath));

            newsDto.NextNews = await GetNextPrevNews(newsDto, true);
            newsDto.PrevNews = await GetNextPrevNews(newsDto, false);
            newsDto.RelatedNews = GetLastRelatedNews(newsDto);
            newsDto.PublishEntity = publishReferences.Any() ? publishReferences.Select(x => new Dtos.Reference
            {
                Id = x.ReferenceId,
            }).ToList() : new List<Dtos.Reference>();


            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, newsDto.ReferenceId, (int)Enums.Entities.News, newsDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.NewsEntity, newsDto));
        }

        #region HELPER METHODS >> GetItemByID
        private async Task<NewsDto> GetNextPrevNews(NewsDto RequestItem, bool IsNext)
        {
            var Query =  _unitOfWork.News.GetAll()
                .Where(x => (IsNext ? x.Id > RequestItem.Id.Value : x.Id < RequestItem.Id.Value) && x.IsDeleted == false)
                .Where(x => RequestUserRole == Enums.UsersRoles.NormalUser ? x.IsActive == true : true);


            var newsModel = !IsNext ? await Query.OrderByDescending(x => x.Id).AsNoTracking().FirstOrDefaultAsync() : await Query.AsNoTracking().FirstOrDefaultAsync();

            var newsDto = newsModel.Adapt<NewsDto>(NewsDto.SelectConfigNewsDetail(IsLocal, GetPath, IntranetGetPath
                                 , newsThumbsGetPath, newsImagesGetPath));

            return newsDto;

        }

        private List<NewsDto> GetLastRelatedNews(NewsDto RequestedData)
        {
            var parameters = new[] {
            new SqlParameter("@TargetedTags", SqlDbType.NVarChar) { Direction = ParameterDirection.InputOutput, Value = RequestedData.TagsAr!=null?RequestedData.TagsAr:RequestedData.TagsEn!=null?RequestedData.TagsEn:string.Empty },
            new SqlParameter("@NewsId", SqlDbType.Int) { Direction = ParameterDirection.InputOutput, Value = RequestedData.Id },
            new SqlParameter("@splitterChar", SqlDbType.NVarChar) { Direction = ParameterDirection.InputOutput, Value = "$" },
            new SqlParameter("@NumberOfRecord", SqlDbType.Int) { Direction = ParameterDirection.InputOutput, Value = 3 },
            new SqlParameter("@referenceId", SqlDbType.Int) { Direction = ParameterDirection.InputOutput, Value = RequestedData.ReferenceId }

            };
            string ProcedureName = "Sp_GetAnotherNewsRelatedByTags";
            string ProcedureParameters = " @TargetedTags,@splitterChar,@NewsId,@NumberOfRecord,@referenceId";
            return _unitOfWork.News.GetFromProcdure(ProcedureName, ProcedureParameters, parameters).Select(x => new NewsDto()
            {
                Id = x.Id,
                TitleAr = x.TitleAr,
                TitleEn = x.TitleEn,
                BriefeContentAr = x.BriefeContentAr,
                BriefeContentEn = x.BriefeContentEn,
                CreatedDate = x.CreatedDate,
                NewsDate = x.NewsDate,
                ThumpPic = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(x.ThumpPic) ? newsThumbsGetPath + "/" + x.ThumpPic : null,
                OriginalPic = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(x.OriginalPic) ? newsImagesGetPath + "/" + x.OriginalPic : null,

            }).ToList();


        }

        #endregion

        public async Task<OperationOutput> SaveNews(NewsDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);
            Models.News news = new();

            if (RequestedData.OriginalPicBase64 != null)
                RequestedData.OriginalPic = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.OriginalPicBase64) ?
                      Images.SaveSingleImageOnServer(RequestedData.OriginalPicBase64, null, newsImagesSavePath, true, 400, newsThumbsSavePath) : null;

            if (RequestedData.Id.HasValue)
            {
                news = await _unitOfWork.News.GetAll().FirstOrDefaultAsync(c => c.Id == RequestedData.Id.Value);

                if (news is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(news, RequestedData.UpdateConfig(RequestOwner.Id, newsImagesSavePath, newsThumbsSavePath));
                _unitOfWork.News.Update(news);
            }
            else
            {
                RequestedData.Adapt(news, RequestedData.AddConfig(RequestOwner.Id, newsImagesSavePath, newsThumbsSavePath));
                _unitOfWork.News.Add(news);
            }

            await SaveTags(RequestedData);

            await SavePublishEntities(RequestedData, news);

            _unitOfWork.Complete();

            if (_RedisConfiguration.UseRedis)
                await RedisCache.SetStringAsync("News_LastUpdate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
            else
            _unitOfWork.MemoryCache.Set("NewsLastUpdateDate", TransactionDate, DateTimeOffset.Now.AddDays(1));

            return await GetNewsByID(new NewsDetailDto { Id = news.Id });
        }

        #region HELPER METHOD >> SaveNews
        private async Task SaveTags(NewsDto RequestedData)
        {
            foreach (var item in RequestedData.ListOfTags)
            {
                if (!item.Id.HasValue)
                    _unitOfWork.Tags.Add(new Tag { NameAr = item.NameAr, NameEn = item.NameEn });
                else
                {

                    await _unitOfWork.Tags.ExecuteUpdateAsync(x => x.Id == item.Id,
                          sett => sett.SetProperty(x => x.NameAr, item.NameAr)
                         .SetProperty(x => x.NameEn, item.NameEn));
                }
            }
        }
        private async Task SavePublishEntities(NewsDto RequestedData, Models.News news)
        {
            if (RequestedData.Id.HasValue)
                await _unitOfWork.PublishEntities.ExecuteDeleteAsync(c => c.ItemId == news.Id);

            if (RequestedData.PublishEntity.Any())
            {
                foreach (var item in RequestedData.PublishEntity)
                {
                    var publishNews = new PublishEntities();
                    publishNews.EntityId = (int)Enums.Entities.News;
                    publishNews.ReferenceId = item.Id.Value;
                    publishNews.ItemId = news.Id;
                    publishNews.CreatedBy = news.UpdatedBy;
                    publishNews.CreatedDate = TransactionDate;
                    _unitOfWork.PublishEntities.Add(publishNews);
                }


            }
        }

        #endregion

        public async Task<OperationOutput> GetTagsList()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var tags = await _unitOfWork.Tags.GetAll().AsNoTracking().ToListAsync();
            var tagsDto = tags.Adapt<List<TagsDto>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.TagsEntity, tagsDto));

        }

        public async Task<OperationOutput> SaveTags(TagsDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Tag tag = new();

            if (RequestedData.Id.HasValue)
            {
                tag = await _unitOfWork.Tags.GetAll().AsNoTracking().FirstOrDefaultAsync(c => c.Id == RequestedData.Id.Value);

                if (tag is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(tag, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Tags.Update(tag);
            }
            else
            {
                RequestedData.Adapt(tag, RequestedData.AddConfig(RequestOwner.Id));
                _unitOfWork.Tags.Add(tag);
            }

            await _unitOfWork.CompleteAsync();
            RequestedData.Id = tag.Id;

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.TagsEntity, RequestedData));
        }

        public async Task<OperationOutput> ModelAction(NewsDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (RequestedData.IsDeleted.HasValue)
            {
                await _unitOfWork.News.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                       sett => sett.SetProperty(x => x.IsDeleted, RequestedData.IsDeleted.Value)
                       .SetProperty(d => d.DeletedBy, RequestOwner.Id)
                       .SetProperty(y => y.DeletedDate, TransactionDate)
                       .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                       .SetProperty(y => y.UpdatedDate, TransactionDate));
            }
            if (RequestedData.IsActive.HasValue)
            {
                await _unitOfWork.News.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                        sett => sett.SetProperty(x => x.IsActive, RequestedData.IsActive.Value)
                        .SetProperty(d => d.ActivatedBy, RequestOwner.Id)
                        .SetProperty(y => y.ActivatedDate, TransactionDate)
                        .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                        .SetProperty(y => y.UpdatedDate, TransactionDate));
            }

            if (_RedisConfiguration.UseRedis)
                await RedisCache.SetStringAsync("News_LastUpdate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
            else
                _unitOfWork.MemoryCache.Set("NewsLastUpdateDate", TransactionDate, DateTimeOffset.Now.AddDays(1));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
    }
}

