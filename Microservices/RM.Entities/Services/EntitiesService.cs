
using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Entities.Dtos;
using RM.Entities.UnitOfWorks;
using static RM.Entities.Dtos.OperationOutput;

namespace RM.Entities.Services
{
    public class EntitiesService : BaseService, IEntitiesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EntitiesService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public OperationOutput GetEntityLookups()
        {
            var entityTypes = _unitOfWork.EntitiesType.GetAll().AsNoTracking().ToList().Adapt<List<Dtos.EntitiesType>>();
            var entities = _unitOfWork.Entity.GetAll().AsNoTracking().ToList().Adapt<List<Dtos.Entity>>();
            var dynamicForms = _unitOfWork.Forms.GetAll().AsNoTracking().Where(c => c.IsActive == true).ToList().Adapt<List<Dtos.Lookup>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.EntityTypes, entityTypes),
                    new OutputDictionary(OperationOutputKeys.Entity, entities),
                    new OutputDictionary(OperationOutputKeys.DynamicForms, dynamicForms));
        }

        public async Task<OperationOutput> SaveEntity(Dtos.Entity RequestedData)
        {
            Models.Entity DbItem = new Models.Entity();
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.Id.HasValue)
            {
                DbItem = _unitOfWork.Entity.GetById(RequestedData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig());
                _unitOfWork.Entity.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem, RequestedData.AddConfig());
                _unitOfWork.Entity.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();

            if (!string.IsNullOrEmpty(RequestedData.FormId))
            {
                SaveFormEntity(RequestedData, DbItem);
            }

            RequestedData.Id = DbItem.Id;
            if (DbItem.ParentId == null)
                return await GetEntityDetails(RequestedData);
            else
            {
                RequestedData.Id = DbItem.ParentId;
                return await GetEntityDetails(RequestedData);
            }
        }
        #region HELPER METHODS
        private void SaveFormEntity(Entity RequestedData, Models.Entity DbItem)
        {
            var formsEntity = _unitOfWork.FormsEntity.Find(c => c.EntityId == DbItem.Id);
            if (formsEntity == null)
            {
                formsEntity = new Models.FormsEntity();
                _unitOfWork.FormsEntity.Add(formsEntity);
            }
            formsEntity.EntityId = DbItem.Id;
            formsEntity.FormId = Accessor.Set(RequestedData.FormId);
            formsEntity.Url = DbItem.CmsIdentity;
            _unitOfWork.Complete();
        }
        #endregion
        public async Task<OperationOutput> GetEntityDetails(Dtos.Entity RequestedData)
        {
            var formEntity = _unitOfWork.FormsEntity.GetAll()
                .AsNoTracking().FirstOrDefault(c => c.EntityId == RequestedData.Id);

            var Item = await _unitOfWork.Entity.GetAll()
                .Include(c => c.InverseParent)
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == RequestedData.Id.Value);

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<Dtos.Entity>(Dtos.Entity.SelectConfig(formEntity != null ? formEntity.FormId : null));

            if (formEntity == null && ItemDto.SubEntities.Any())
            {
                var subEntityForms = _unitOfWork.FormsEntity.GetAll()
                    .Where(c => ItemDto.SubEntities.Select(c => c.Id).Contains(c.EntityId)).AsNoTracking().ToList();

                if (subEntityForms.Any())
                {
                    foreach (var item in ItemDto.SubEntities)
                    {
                        item.FormId = Accessor.Get<int?>(subEntityForms.First(c => c.EntityId == item.Id).FormId);
                    }
                }
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.Entity, ItemDto));
        }

        public async Task<OperationOutput> GetEntitiesList(Dtos.Entity RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser)
                RequestedData.IsActive = true;

            var filteration = RequestedData.EntitiesFilteration();
            var Entites = await _unitOfWork.Entity.FindAllByPaginationAsync(filteration, RequestedData.Pagination, DefaultPaginationCount);

            var EntitesDto = Entites.Data.Adapt<List<Dtos.Entity>>(Dtos.Entity.SelectConfig(null));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.Entity, EntitesDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, Entites.Pagination));

        }

        public OperationOutput EntityActivation(Dtos.Entity RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsActive.HasValue || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.Entity.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            DbItem.IsActive = RequestedData.IsActive.HasValue ? RequestedData.IsActive.Value : DbItem.IsActive;

            _unitOfWork.Entity.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }


    }
}
