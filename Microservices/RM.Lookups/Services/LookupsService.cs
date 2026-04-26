

using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Lookups.Dtos;
using RM.Lookups.UnitOfWorks;
using static RM.Lookups.Dtos.OperationOutput;

namespace RM.Lookups.Services
{
    public class LookupsService : BaseService, ILookupsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LookupsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationOutput> GetLookups(Dtos.MajorLookups RequestedData)
        {
            var Items = _unitOfWork.MajorLookup.GetAll().Where(x => x.TypeId == RequestedData.TypeId.Value)
                .Where(x => RequestedData.ParentId.HasValue ? x.ParentId == RequestedData.ParentId.Value : true)
                .AsNoTracking().ToList().Adapt<List<Dtos.MajorLookups>>(Dtos.MajorLookups.SelectConfig());

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.LookupsEntity, Items));
        }

        public async Task<OperationOutput> GetLookupsList(Dtos.MajorLookupsType RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            var result = _unitOfWork.MajorLookupsType.GetAll()
                .Where(RequestedData.Filteration())
                .OrderByDescending(x => x.Id)
                .AsNoTracking().TakePaggination(RequestedData.Pagination, DefaultPaginationCount);

            var resultDto = result.Data.Adapt<List<Dtos.MajorLookupsType>>(Dtos.MajorLookupsType.SelectConfig(RequestedData.ReferenceId));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.LookupsEntity, resultDto),
                   new OutputDictionary(OperationOutputKeys.Pagination, result.Pagination));
        }

        public async Task<OperationOutput> GetLookupDetails(Dtos.MajorLookupsType RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var DbItem = await _unitOfWork.MajorLookupsType.GetAll()
                .Include(x => x.MajorLookups)
                .ThenInclude(c => c.Reference)
                .Include(x => x.MajorLookups)
                .ThenInclude(x => x.InverseParent)
                .Where(x => x.Id == RequestedData.Id).FirstOrDefaultAsync();

            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = DbItem.Adapt<Dtos.MajorLookupsType>(Dtos.MajorLookupsType.SelectConfig(RequestedData.ReferenceId));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.LookupsEntity, ItemDto));
        }

        public async Task<OperationOutput> SaveLookup(Dtos.MajorLookupsType RequestedData)
        {
            Models.MajorLookupsType DbItem = new Models.MajorLookupsType();
            Models.MajorLookup MLookup = null;
            Models.MajorLookup subLookup = null;
            using (var dbContextTransaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    if (RequestOwner == null)
                        return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

                    if (RequestedData.Id.HasValue)
                    {
                        DbItem = await _unitOfWork.MajorLookupsType.GetAll()
                             .Include(c => c.MajorLookups)
                             .ThenInclude(c => c.InverseParent)
                             .FirstOrDefaultAsync(c => c.Id == RequestedData.Id.Value);

                        if (DbItem == null)
                            return GetOperationOutput(header: Enums.ServiceMessages.WrongeData);

                        RequestedData.Adapt(DbItem, RequestedData.UpdateConfig());
                        _unitOfWork.MajorLookupsType.Update(DbItem);
                    }
                    else
                    {
                        DbItem = new Models.MajorLookupsType();
                        RequestedData.Adapt(DbItem, RequestedData.AddConfig());
                        _unitOfWork.MajorLookupsType.Add(RequestedData.Adapt(DbItem, RequestedData.AddConfig()));
                    }

                    _unitOfWork.Complete();
                    AddOrUpdateLookUps(RequestedData, DbItem, ref MLookup, ref subLookup);

                    dbContextTransaction.Commit();
                    RequestedData.Id = DbItem.Id;
                    return await GetLookupDetails(RequestedData);

                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);
                }
            }

        }

        private void AddOrUpdateLookUps(MajorLookupsType RequestedData, Models.MajorLookupsType DbItem, ref Models.MajorLookup MLookup, ref Models.MajorLookup subLookup)
        {
            if (RequestedData.MajorLookups.Count > 0)
            {
                foreach (var item in RequestedData.MajorLookups)
                {
                    if (!item.Id.HasValue)
                    {
                        MLookup = new Models.MajorLookup();
                        item.Adapt(MLookup, item.AddConfig());
                    }
                    else
                    {
                        MLookup = DbItem.MajorLookups.FirstOrDefault(c => c.Id == item.Id.Value);
                        item.Adapt(MLookup, item.UpdateConfig());
                    }

                    if (item.SubLookups.Any())
                    {
                        foreach (var sub in item.SubLookups)
                        {
                            if (!sub.Id.HasValue)
                            {
                                subLookup = new Models.MajorLookup();
                                sub.Adapt(subLookup, sub.AddConfig());
                                subLookup.ParentId = MLookup.Id;
                                MLookup.InverseParent.Add(subLookup);
                            }
                            else
                            {
                                subLookup = MLookup.InverseParent.FirstOrDefault(c => c.Id == sub.Id.Value);
                                sub.Adapt(subLookup, sub.UpdateConfig());
                                _unitOfWork.MajorLookup.Update(subLookup);
                            }
                        }
                    }

                    if (!item.Id.HasValue) DbItem.MajorLookups.Add(MLookup);
                    else _unitOfWork.MajorLookup.Update(MLookup);
                }
                _unitOfWork.Complete();
            }
        }

        public async Task<OperationOutput> Activation(Dtos.MajorLookupsType RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsActive.HasValue || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.MajorLookupsType.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            DbItem.IsActive = RequestedData.IsActive.HasValue ? RequestedData.IsActive.Value : DbItem.IsActive;

            _unitOfWork.MajorLookupsType.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        public async Task<OperationOutput> DeleteMajorLookup(Dtos.MajorLookups RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsDeleted.HasValue || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.MajorLookup.GetAll().Include(c => c.InverseParent).FirstOrDefault(c => c.Id == RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            DbItem.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : DbItem.IsDeleted;
            if (DbItem.InverseParent.Count() > 0)
            {
                foreach (var item in DbItem.InverseParent)
                {
                    item.IsDeleted = true;
                }
            }

            _unitOfWork.MajorLookup.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
    }
}
