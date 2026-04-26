using Microsoft.Extensions.Configuration;
using RM.Investments.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using static RM.Investments.Dtos.OperationOutput;
using System.Threading.Tasks;
using RM.Core.Services;
using RM.Investments.UnitOfWorks;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Consts;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace RM.Investments.Services
{
    public class InvestmentsService:BaseService, IInvestmentsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public InvestmentsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationOutput> GetInvestmentDetails(Dtos.Investments RequestedData)
        {
            return await GetInvestmentDetails(RequestedData.Id.Value);
        }
        public async Task<OperationOutput> GetInvestmentDetails(int Id)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var DbItem = _unitOfWork.Investments.GetById(Id);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = DbItem.Adapt<Dtos.Investments>(Dtos.Investments.SelectConfig());

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, ItemDto.ReferenceId, ItemDto.EntityId, ItemDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.InvestmentsEntity, ItemDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, ItemDto.EntityId));
        }

        public async Task<OperationOutput> GetInvestmentsTypes()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var ItemList = await _unitOfWork.InvestmentTypes.GetAll().Where(x => x.IsDeleted != true).AsNoTracking().ToListAsync();

            var ItemListDto = ItemList.Adapt<List<Dtos.InvestmentTypes>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                       new OutputDictionary(OperationOutputKeys.InvestmentsEntity, ItemListDto));
        }

        public async Task<OperationOutput> GetInvestmentsList(Dtos.Investments RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser)
                RequestedData.IsActive = true;

            var Investments = await _unitOfWork.Investments.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending,t => t.OpportunityTypeNavigation);

            var InvestmentsDto = Investments.Data.ToList().Adapt<List<Dtos.Investments>>(Dtos.Investments.SelectConfig());

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.Articles, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.InvestmentsEntity, InvestmentsDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, InvestmentsDto.Count > 0 ? InvestmentsDto[0].EntityId : string.Empty),
                   new OutputDictionary(OperationOutputKeys.Pagination, Investments.Pagination));
        }

        public async Task<OperationOutput> SaveInvestment(Dtos.Investments RequestdData)
        {
            Models.Investment DbItem = new Models.Investment();
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestdData.Id.HasValue)
            {
                DbItem = _unitOfWork.Investments.GetById(RequestdData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);
                RequestdData.Adapt(DbItem, RequestdData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Investments.Update(DbItem);
            }
            else
            {
                RequestdData.Adapt(DbItem, RequestdData.AddConfig(RequestOwner.Id));
                _unitOfWork.Investments.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            return await GetInvestmentDetails(DbItem.Id);
        }


        public async Task<OperationOutput> ModelAction(Dtos.Investments RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.Investments.GetById(RequestedData.Id.Value);
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

            _unitOfWork.Investments.Update(DbItem);
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
    }
}
