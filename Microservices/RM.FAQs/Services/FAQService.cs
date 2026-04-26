

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RM.FAQs.Dtos;
using static RM.FAQs.Dtos.OperationOutput;
using RM.FAQs.UnitOfWorks;
using RM.Core.Services;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Consts;
using Mapster;
using RM.Models;

namespace RM.FAQs.Services
{
    public class FAQService:BaseService,IFAQService
    {
        private readonly IUnitOfWork _unitOfWork;
        public FAQService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<OperationOutput> GetFAQList(Dtos.FAQ RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var FAQs = await _unitOfWork.FAQs.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending, x => x.CreatedByNavigation, x => x.UpdatedByNavigation);

            var FAQsDto = FAQs.Data.Adapt<List<Dtos.FAQ>>(Dtos.FAQ.SelectConfig());

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.FAQ, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.FAQEntity, FAQsDto),
                   new OutputDictionary(OperationOutputKey.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.FAQ)),
                   new OutputDictionary(OperationOutputKey.Pagination, FAQs.Pagination));
        }
        public async Task<OperationOutput> GetFAQDetails(Dtos.FAQ RequestedData)
        {
            return await GetFAQDetails(RequestedData.Id.Value);

        }
        public async Task<OperationOutput> GetFAQDetails(int Id)
        {

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

           var DbItem = _unitOfWork.FAQs.GetAll()
                .Where(x => x.Id == Id && x.IsDeleted != true)
                .AsNoTracking().FirstOrDefault();

            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

           var ItemDto = DbItem.Adapt<Dtos.FAQ>(Dtos.FAQ.SelectConfig());

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, ItemDto.ReferenceId, (int)Enums.Entities.FAQ, ItemDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.FAQEntity, ItemDto),
                   new OutputDictionary(OperationOutputKey.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.FAQ)));
        }

        public async Task<OperationOutput> SaveFAQ(Dtos.FAQ RequestdData)
        {
            Models.FAQ DbItem = new Models.FAQ();
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestdData.Id.HasValue)
            {
                DbItem = await _unitOfWork.FAQs.GetByIdAsync(RequestdData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestdData.Adapt(DbItem, RequestdData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.FAQs.Update(DbItem);
            }
            else
            {
                RequestdData.Adapt(DbItem, RequestdData.AddConfig(RequestOwner.Id));
                _unitOfWork.FAQs.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            return await GetFAQDetails(DbItem.Id);

        }

        public async Task<OperationOutput> FAQModelActions(Dtos.FAQ RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.FAQs.GetById(RequestedData.Id.Value);
            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            DbItem.IsDeleted = RequestedData.IsDeleted.HasValue ? RequestedData.IsDeleted.Value : DbItem.IsDeleted;

            DbItem.UpdatedBy = RequestOwner.Id;
            DbItem.UpdatedDate = DateTime.Now;

            _unitOfWork.FAQs.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

    }
}
