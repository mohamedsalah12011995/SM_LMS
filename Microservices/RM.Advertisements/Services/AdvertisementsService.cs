using RM.Core.Consts;
using Microsoft.EntityFrameworkCore;
using RM.Advertisements.Dtos;
using RM.Core.Helpers;
using static RM.Advertisements.Dtos.OperationOutput;
using RM.Advertisements.UnitOfWorks;
using RM.Models;
using Mapster;
using RM.Core.Services;
using RM.Core.Integrations;
using System.Linq.Expressions;
using Microsoft.Extensions.Caching.Memory;
using static RM.Core.Helpers.Enums;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using Common;

namespace RM.Advertisements.Services
{
    public class AdvertisementsService : BaseService, IAdvertisementsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdvertisementsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, RedisConfiguration RedisConfiguration, IDistributedCache redisCache)
            : base(httpContextAccessor, unitOfWork.Configuration, RedisConfiguration, redisCache)
        {
            _unitOfWork = unitOfWork;
        }

        public OperationOutput GetReferences()
        {
            var references = _unitOfWork.References.FindAll(c => c.IsPortal == true && c.ParentId == null);
            var referencesDto = references.Adapt<List<Dtos.Reference>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.References, referencesDto));
        }

        public async Task<OperationOutput> GetAdvertismentsList(Dtos.Advertisements RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser)
                RequestedData.IsActive = true;

            var publishedAdvertisments = _unitOfWork.PublishEntities.FindAll(c => c.EntityId == (int)Enums.Entities.Advertisments && c.ReferenceId == RequestedData.ReferenceId).ToList();
            var filteration = RequestedData.Filteration(publishedAdvertisments.Select(x => x.ItemId).ToList());

            var thenByList = new Expression<Func<Advertisement, object>>[]
                {    x => x.FromDate,
                     x => x.CreatedDate
                };

            var advertisements = await _unitOfWork.Advertisements.FindAllByPaginationWithThenBy(filteration, RequestedData.Pagination, DefaultPaginationCount, x => x.ToDate, OrderBy.Descending,
                thenByList, ThenBy.Descending, x => x.CreatedByNavigation, x => x.UpdatedByNavigation);

            var advertisementsDto = advertisements.Data.Adapt<List<Dtos.Advertisements>>(Dtos.Advertisements.SelectConfig(ImagesGetPath, publishedAdvertisments));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.Advertisments, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.AdvertisementsEntity, advertisementsDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Advertisments)),
                   new OutputDictionary(OperationOutputKeys.Pagination, advertisements.Pagination));

        }

        public async Task<OperationOutput> GetMainSliderList(Dtos.Advertisements RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            if (_RedisConfiguration.UseRedis)
            {
                DateTime LastUpdate;
                DateTime.TryParse(await RedisCache.GetStringAsync("Advertisements_LastUpdate"), out LastUpdate);

                var CacheDate = await RedisCache.GetStringAsync("Advertisements_MainSliders_CacheDate");
                var MainSliders = await RedisCache.GetStringAsync("Advertisements_MainSliders");
                if (MainSliders == null || CacheDate == null || DateTime.Parse(CacheDate) < LastUpdate)
                {
                    Result = await GetMainSliders(RequestedData);
                    if (Result.Header.Success)
                    {
                        await RedisCache.SetStringAsync("Advertisements_MainSliders", JsonSerializer.Serialize(Result, jsonOptions), _RedisConfiguration.RedisOptions);
                        await RedisCache.SetStringAsync("Advertisements_MainSliders_CacheDate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
                    }
                    return Result;
                }

                Result = JsonSerializer.Deserialize<OperationOutput>(MainSliders, jsonOptions);
                return Result;
            }
            else
            {
                return await GetMainSliders(RequestedData);
            }
        }
        public async Task<OperationOutput> GetMainSliders(Dtos.Advertisements RequestedData)
        {

            List<Dtos.Advertisements> advertisementsDto = null;
            ApplicationOperation.Pagination pagination = SetDefaultData();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            var filteration = RequestedData.Filteration(null);

            if (!_RedisConfiguration.UseRedis && IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser)
                advertisementsDto = GetMainSliderForPortal(filteration);
            else
            {
                var advertisements = await _unitOfWork.Advertisements.FindAllByPaginationAsync(filteration, RequestedData.Pagination, DefaultPaginationCount, x => x.AdvertisementOrder ?? x.Id, OrderBy.Ascending,
                   x => x.CreatedByNavigation, x => x.UpdatedByNavigation);

                advertisementsDto = advertisements.Data.Adapt<List<Dtos.Advertisements>>(Dtos.Advertisements.SelectConfig(ImagesGetPath, null));
                pagination = advertisements.Pagination;
            }
            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.MainSlider, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.AdvertisementsEntity, advertisementsDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.MainSlider)),
                   new OutputDictionary(OperationOutputKeys.Pagination, pagination));
        }

        private List<Dtos.Advertisements> GetMainSliderForPortal(Expression<Func<Models.Advertisement, bool>> filteration)
        {
            var MainSliderLastUpdateDate = _unitOfWork.MemoryCache.Get<DateTime?>("MainSliderLastUpdateDate");
            var LatestMainSliderGetDate = _unitOfWork.MemoryCache.Get<DateTime?>("LatestMainSliderGetDate");
            var MainSliders = _unitOfWork.MemoryCache.Get<List<Dtos.Advertisements>>("MainSliders");

            if (MainSliderLastUpdateDate == null)
            {
                MainSliderLastUpdateDate = TransactionDate;
                _unitOfWork.MemoryCache.Set("MainSliderLastUpdateDate", MainSliderLastUpdateDate, DateTimeOffset.Now.AddDays(1));
            }

            if (LatestMainSliderGetDate == null || MainSliders == null || MainSliderLastUpdateDate > LatestMainSliderGetDate)
            {
                MainSliders = _unitOfWork.Advertisements.GetAll().Where(filteration)
                           .OrderByDescending(x => x.AdvertisementOrder ?? x.Id)
                           .Take(10).ToList()
                           .Adapt<List<Dtos.Advertisements>>(Dtos.Advertisements.SelectConfig(ImagesGetPath,null));
                _unitOfWork.MemoryCache.Set("MainSliders", MainSliders, DateTimeOffset.Now.AddDays(1));
                _unitOfWork.MemoryCache.Set("LatestMainSliderGetDate", TransactionDate, DateTimeOffset.Now.AddDays(1));
            }

            return MainSliders;
        }

        public async Task<OperationOutput> SaveAdvertisment(Dtos.Advertisements RequestedData)
        {
            Advertisement DbItem = new Advertisement();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.ImageUrlBase64 != null)
                RequestedData.ImageUrl = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.ImageUrlBase64)
                    ? Images.SaveSingleImageOnServer(RequestedData.ImageUrlBase64, null, ImagesSavePath, false) : null;

            if (RequestedData.Id.HasValue)
            {
                DbItem = _unitOfWork.Advertisements.GetById(RequestedData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Advertisements.Update(DbItem);

                var deleteReferences = _unitOfWork.PublishEntities.FindAll(c => c.ItemId == DbItem.Id).ToList();
                if (deleteReferences.Any()) { _unitOfWork.PublishEntities.DeleteRange(deleteReferences); }
            }
            else
            {
                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
                _unitOfWork.Advertisements.Add(DbItem);
            }

            #region Save Publish Advertisements
            if (RequestedData.PublishEntity.Any())
            {
                foreach (var item in RequestedData.PublishEntity)
                {
                    var publish = new PublishEntities();
                    publish.EntityId = (int)Enums.Entities.Advertisments;
                    publish.ReferenceId = item.Id.Value;
                    publish.ItemId = DbItem.Id;
                    publish.CreatedBy = DbItem.UpdatedBy;
                    publish.CreatedDate = TransactionDate;
                    _unitOfWork.PublishEntities.Add(publish);
                }
            }
            #endregion

            await _unitOfWork.CompleteAsync();

            if (_RedisConfiguration.UseRedis)
                await RedisCache.SetStringAsync("Advertisements_LastUpdate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
            else
                _unitOfWork.MemoryCache.Set("MainSliderLastUpdateDate", TransactionDate, DateTimeOffset.Now.AddDays(1));

            return await GetAdvertismentsDetails(DbItem.Id);

        }

        public async Task<OperationOutput> GetAdvertismentDetails(Dtos.Advertisements RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            if (_RedisConfiguration.UseRedis)
            {
                DateTime LastUpdate;
                DateTime.TryParse(await RedisCache.GetStringAsync("Advertisements_LastUpdate"), out LastUpdate);

                var CacheDate = await RedisCache.GetStringAsync("Advertisement_CacheDate");
                var Advertisement = await RedisCache.GetStringAsync("Advertisement_" + RequestedData.ID);
                if (Advertisement == null || CacheDate == null || DateTime.Parse(CacheDate) < LastUpdate)
                {
                    Result = await GetAdvertismentsDetails(RequestedData.Id.Value);
                    if (Result.Header.Success)
                    {
                        await RedisCache.SetStringAsync("Advertisement_" + RequestedData.ID, JsonSerializer.Serialize(Result, jsonOptions), _RedisConfiguration.RedisOptions);
                        await RedisCache.SetStringAsync("Advertisement_CacheDate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
                    }
                    return Result;
                }

                Result = JsonSerializer.Deserialize<OperationOutput>(Advertisement, jsonOptions);
                return Result;
            }
            else
            {
                return await GetAdvertismentsDetails(RequestedData.Id.Value);
            }

        }

        public async Task<OperationOutput> GetAdvertismentsDetails(int Id)
        {
            Dtos.Advertisements ItemDto = new Dtos.Advertisements();
            var publishReferences = _unitOfWork.PublishEntities.FindAll(x => x.ItemId == Id).ToList();

            var Item = _unitOfWork.Advertisements.GetAll()
                .Include(x => x.Entity)
                .Where(x => x.Id == Id && x.IsDeleted != true)
                .Where(x => (IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser) ? x.IsActive == true : true)
                .AsNoTracking().FirstOrDefault();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Item.Adapt(ItemDto, Dtos.Advertisements.SelectConfig(ImagesGetPath, publishReferences));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, (int)Enums.Entities.Advertisments, Item.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.AdvertisementsEntity, ItemDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, ItemDto.entityId));

        }

        public OperationOutput ModelActions(Dtos.Advertisements RequestedData)
        {
            Advertisement DbItem = null;
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            DbItem = _unitOfWork.Advertisements.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

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

            _unitOfWork.Advertisements.Update(DbItem);
            _unitOfWork.Complete();

            _unitOfWork.MemoryCache.Set("MainSliderLastUpdateDate", TransactionDate, DateTimeOffset.Now.AddDays(1));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        public OperationOutput SortOrder(List<Dtos.Advertisements> RequestedData)
        {
            List<Advertisement> DbItem = new List<Advertisement>();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            DbItem = _unitOfWork.Advertisements.GetAll().Where(x => x.IsDeleted == false
             && RequestedData.Select(v => v.Id).Contains(x.Id)).ToList();
            foreach (var item in DbItem)
            {
                item.AdvertisementOrder = RequestedData.Where(v => v.Id == item.Id).FirstOrDefault().AdvertisementOrder;
                _unitOfWork.Advertisements.Update(item);
            }
            _unitOfWork.Complete();
            _unitOfWork.MemoryCache.Set("MainSliderLastUpdateDate", TransactionDate, DateTimeOffset.Now.AddDays(1));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }


        #region PlatformAdvertisments

        public OperationOutput GetPlatformLookups()
        {
            var districts = _unitOfWork.MajorLookup.GetAll()
               .OrderByDescending(x => x.Id)
               .Where(x => x.TypeId == (int)Enums.MajorLookupsTypes.District).AsNoTracking().ToList();
            var districtsDto= districts.Adapt<List<AdvertismentLookups>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.DistrictLookup, districtsDto));
        }

        public async Task<OperationOutput> GetPlatformAdvertismentsList(Dtos.Advertisements RequestedData)
        {
            List<Dtos.Advertisements> Item = null;

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestUserRole == Enums.UsersRoles.NormalUser)
                RequestedData.IsActive = true;

            var filteration = RequestedData.PlatformFilteration();

            var thenByList = new Expression<Func<Advertisement, object>>[]
                {    x => x.FromDate,
                     x => x.CreatedDate
                };

            var PlatformAdvertisements = await _unitOfWork.Advertisements.FindAllByPaginationWithThenBy(filteration, RequestedData.Pagination, DefaultPaginationCount, x => x.ToDate , OrderBy.Descending,
                                 thenByList, ThenBy.Descending, x => x.CreatedByNavigation, x => x.UpdatedByNavigation, c => c.Region);

            var PlatformAdvertisementsDto = PlatformAdvertisements.Data.Adapt<List<Dtos.Advertisements>>(Dtos.Advertisements.SelectPlatformConfig(FilesGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.Platforms, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.AdvertisementsEntity, PlatformAdvertisementsDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, RequestedData.EntityId),
                   new OutputDictionary(OperationOutputKeys.Pagination, PlatformAdvertisements.Pagination));
        }

        public async Task<OperationOutput> SavePlatformAdvertisment(Dtos.Advertisements RequestedData)
        {
            Advertisement DbItem = new Advertisement();
            string fileName = Strings.GenerateGUID() + ".pdf";
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!string.IsNullOrEmpty(RequestedData.FileUrlBase64))
            {
                if (Files.GetBase64FileSizeMb(RequestedData.FileUrlBase64) > FileSizeMb)
                    return GetOperationOutput(header: Enums.ServiceMessages.FileSizeError);
                RequestedData.FileUrl = Files.SaveBase64FileToServer(fileName, RequestedData.FileUrlBase64, FilesSavePath);
            }

            if (RequestedData.Id.HasValue)
            {
                DbItem = _unitOfWork.Advertisements.GetById(RequestedData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Advertisements.Update(DbItem);
            }
            else
            {
                DbItem.SerialNum = _unitOfWork.Advertisements.Count(x => x.CreatedDate.Value.Year == TransactionDate.Year && x.EntityId == (int)Enums.Entities.Platforms && x.ReferenceId == RequestedData.ReferenceId) + 1;
                DbItem.Code = DbItem.SerialNum.ToString() + "/" + TransactionDate.ToString("yy");

                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
                _unitOfWork.Advertisements.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            RequestedData.Id = DbItem.Id;
            return await GetPlatformAdvertismentDetails(RequestedData);

        }

        public async Task<OperationOutput> GetPlatformAdvertismentDetails(Dtos.Advertisements RequestedData)
        {
            Dtos.Advertisements ItemDto = new Dtos.Advertisements();

            var Item = _unitOfWork.Advertisements.GetAll()
                        .Include(x => x.Region)
                        .Where(x => x.Id == RequestedData.Id && x.IsDeleted != true)
                        .Where(x => (IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser) ? x.IsActive == true : true)
                        .AsNoTracking().FirstOrDefault();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Item.Adapt(ItemDto, Dtos.Advertisements.SelectPlatformConfig(FilesGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, (int)Enums.Entities.Platforms, Item.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.AdvertisementsEntity, ItemDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, ItemDto.entityId));
        }

        #endregion


    }
}
