
using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using RM.Models;
using RM.GovServices.Dtos;
using RM.GovServices.UnitOfWorks;
using static RM.GovServices.Dtos.OperationOutput;
using Common;
using Microsoft.Extensions.Caching.Distributed;
using static RM.Core.Helpers.Enums;
using System.Text.Json;

namespace RM.GovServices.Services
{
    public class GovServicesService : BaseService, IGovServicesService
    {
        private readonly IUnitOfWork _unitOfWork;
        public GovServicesService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, RedisConfiguration RedisConfiguration, IDistributedCache redisCache)
            : base(httpContextAccessor, unitOfWork.Configuration, RedisConfiguration, redisCache)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetGovServicesCategories(Dtos.Govservice RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = _unitOfWork.GovServices.GetAll().Where(x => x.IsDeleted == false)
                  .Where(x => RequestedData.ParentId == null ? x.IsRoot == true : x.ParentId == RequestedData.ParentId)
                  .Adapt<List<Dtos.Govservice>>(Dtos.Govservice.SelectConfig(ImagesGetPath));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.GovServicesEntity, Item),
                   new OutputDictionary(OperationOutputKey.EntityID, Item.Count > 0 ? Item[0].entityId : null));
        }

        public async Task<OperationOutput> GetEservicesCategories(Dtos.Govservice RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = _unitOfWork.Eservices.GetAll().Where(x => x.IsDeleted == false)
                .Where(x => RequestedData.ParentId == null ? x.IsRoot == true : x.ParentId == RequestedData.ParentId)
                .Adapt<List<Dtos.Eservices>>(Dtos.Eservices.SelectConfig(ImagesGetPath));


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.EservicesEntity, Item),
                   new OutputDictionary(OperationOutputKey.EntityID, Item.Count > 0 ? Item[0].entityId : null));
        }

        public async Task<OperationOutput> GetGovserviceList(Dtos.Govservice RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var List = await _unitOfWork.GovServices.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending, x => x.CreatedByNavigation, x => x.UpdatedByNavigation);

            var ListDto = List.Data.Adapt<List<Dtos.Govservice>>(Dtos.Govservice.SelectConfig(ImagesGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.GovServices, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.GovServicesEntity, ListDto),
                   new OutputDictionary(OperationOutputKey.EntityID, ListDto.Count > 0 ? ListDto[0].entityId : null),
                   new OutputDictionary(OperationOutputKey.Pagination, List.Pagination));
        }

        public async Task<OperationOutput> GetEserviceList(Dtos.Eservices RequestedData)
        {

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var List = await _unitOfWork.Eservices.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending, x => x.CreatedByNavigation, x => x.UpdatedByNavigation);

            var ListDto = List.Data.Adapt<List<Dtos.Eservices>>(Dtos.Eservices.SelectConfig(ImagesGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.GovServices, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.EservicesEntity, ListDto),
                   new OutputDictionary(OperationOutputKey.EntityID, ListDto.Count > 0 ? ListDto[0].entityId : null),
                   new OutputDictionary(OperationOutputKey.Pagination, List.Pagination));
        }

        public async Task<OperationOutput> GetGovserviceDetails(Dtos.Govservice RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            if (_RedisConfiguration.UseRedis)
            {
                DateTime LastUpdate;
                DateTime.TryParse(await RedisCache.GetStringAsync("Govservice_LastUpdate"), out LastUpdate);

                var CacheDate = await RedisCache.GetStringAsync("Govservice_CacheDate");
                var Article = await RedisCache.GetStringAsync("Govservice_" + RequestedData.ID);
                if (Article == null || CacheDate == null || DateTime.Parse(CacheDate) < LastUpdate)
                {
                    Result = await GetGovserviceDetails(RequestedData.Id.Value);
                    if (Result.Header.Success)
                    {
                        await RedisCache.SetStringAsync("Govservice_" + RequestedData.ID, JsonSerializer.Serialize(Result, jsonOptions), _RedisConfiguration.RedisOptions);
                        await RedisCache.SetStringAsync("Govservice_CacheDate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
                    }
                    return Result;
                }

                Result = JsonSerializer.Deserialize<OperationOutput>(Article, jsonOptions);
                return Result;
            }
            else
            {
                return await GetGovserviceDetails(RequestedData.Id.Value);
            }

        }

        public async Task<OperationOutput> GetGovserviceDetails(int Id)
        {

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var DbItem = _unitOfWork.GovServices.GetAll()
                 .Where(x => x.Id == Id && x.IsDeleted != true)
                 .AsNoTracking().FirstOrDefault();

            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = DbItem.Adapt<Dtos.Govservice>(Dtos.Govservice.SelectConfig(ImagesGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, ItemDto.ReferenceId, (int)Enums.Entities.GovServices, ItemDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.GovServicesEntity, ItemDto),
                   new OutputDictionary(OperationOutputKey.EntityID, ItemDto.entityId));

        }

        public async Task<OperationOutput> GetEserviceDetails(Dtos.Eservices RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            if (_RedisConfiguration.UseRedis)
            {
                DateTime LastUpdate;
                DateTime.TryParse(await RedisCache.GetStringAsync("Eservices_LastUpdate"), out LastUpdate);

                var CacheDate = await RedisCache.GetStringAsync("Eservices_CacheDate");
                var Article = await RedisCache.GetStringAsync("Eservices_" + RequestedData.ID);
                if (Article == null || CacheDate == null || DateTime.Parse(CacheDate) < LastUpdate)
                {
                    Result = await GetEserviceDetails(RequestedData.Id.Value);
                    if (Result.Header.Success)
                    {
                        await RedisCache.SetStringAsync("Eservices_" + RequestedData.ID, JsonSerializer.Serialize(Result, jsonOptions), _RedisConfiguration.RedisOptions);
                        await RedisCache.SetStringAsync("Eservices_CacheDate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);
                    }
                    return Result;
                }

                Result = JsonSerializer.Deserialize<OperationOutput>(Article, jsonOptions);
                return Result;
            }
            else
            {
                return await GetEserviceDetails(RequestedData.Id.Value);
            }

        }

        public async Task<OperationOutput> GetEserviceDetails(int Id)
        {

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var DbItem = _unitOfWork.Eservices.GetAll()
                 .Where(x => x.Id == Id && x.IsDeleted != true)
                 .AsNoTracking().FirstOrDefault();

            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = DbItem.Adapt<Dtos.Eservices>(Dtos.Eservices.SelectConfig(ImagesGetPath));
            ItemDto.ServiceUrl = ItemDto.ID;

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, ItemDto.ReferenceId, (int)Enums.Entities.EServices, ItemDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.EservicesEntity, ItemDto),
                   new OutputDictionary(OperationOutputKey.EntityID, ItemDto.entityId));

        }

        public async Task<OperationOutput> SaveGovService(Dtos.Govservice RequestdData)
        {
            Models.GovService DbItem = new Models.GovService();
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestdData.Id.HasValue)
            {
                DbItem = await _unitOfWork.GovServices.GetByIdAsync(RequestdData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestdData.Adapt(DbItem, RequestdData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.GovServices.Update(DbItem);
            }
            else
            {
                RequestdData.Adapt(DbItem, RequestdData.AddConfig(RequestOwner.Id));
                _unitOfWork.GovServices.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();

            if (_RedisConfiguration.UseRedis)
                RedisCache.SetString("Govservice_LastUpdate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);

            return await GetGovserviceDetails(DbItem.Id);
        }

        public async Task<OperationOutput> SaveEservice(Dtos.Eservices RequestdData)
        {
            Models.Eservice DbItem = new Models.Eservice();
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestdData.Id.HasValue)
            {
                DbItem = await _unitOfWork.Eservices.GetByIdAsync(RequestdData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestdData.Adapt(DbItem, RequestdData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Eservices.Update(DbItem);
            }
            else
            {
                RequestdData.Adapt(DbItem, RequestdData.AddConfig(RequestOwner.Id));
                _unitOfWork.Eservices.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();

            if (_RedisConfiguration.UseRedis)
                RedisCache.SetString("Eservices_LastUpdate", TransactionDate.ToString(), _RedisConfiguration.RedisOptions);

            return await GetEserviceDetails(DbItem.Id);

        }

        public async Task<OperationOutput> GovServicesModelActions(Dtos.Govservice RequestedData)
        {
            GovService DbItem;
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            DbItem = _unitOfWork.GovServices.GetById(RequestedData.Id.Value);
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

            _unitOfWork.GovServices.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        public async Task<OperationOutput> EservicesModelActions(Dtos.Eservices RequestedData)
        {
            Eservice DbItem;
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            DbItem = _unitOfWork.Eservices.GetById(RequestedData.Id.Value);
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

            _unitOfWork.Eservices.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

    }
}
