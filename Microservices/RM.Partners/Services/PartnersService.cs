
using RM.Partners.Dtos;
using RM.Partners.UnitOfWorks;
using static RM.Partners.Dtos.OperationOutput;
using RM.Core.Services;
using RM.Core.Helpers;
using RM.Core.Consts;
using RM.Core.Integrations;
using Mapster;
using Microsoft.EntityFrameworkCore;


namespace RM.Partners.Services
{
    public class PartnersService:BaseService, IPartnersService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PartnersService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor,unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationOutput> GetPartners(Dtos.Partner RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser)
                RequestedData.IsActive = true;

            var result = await _unitOfWork.Partners.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending, x => x.CreatedByNavigation, x => x.UpdatedByNavigation);

            var resultDto = result.Data.Adapt<List<Dtos.Partner>>(Dtos.Partner.SelectConfig(ImagesGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, RequestedData.EntityId, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.PartnersEntity, resultDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.FAQ)),
                   new OutputDictionary(OperationOutputKeys.Pagination, result.Pagination));
        }
        public async Task<OperationOutput> GetPartnerDetails(Dtos.Partner RequestedData)
        {
            return await GetPartnerDetails(RequestedData.Id.Value);
        }
        public async Task<OperationOutput> GetPartnerDetails(int Id)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = _unitOfWork.Partners.GetAll()
                .Where(x => x.Id == Id && x.IsDeleted != true)
                .AsNoTracking().FirstOrDefault();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<Dtos.Partner>(Dtos.Partner.SelectConfig(ImagesGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, Item.EntityId, Item.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.PartnersEntity, ItemDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Partners)));
        }

        public async Task<OperationOutput> SavePartners(Dtos.Partner RequestdData)
        {
            Models.Partner DbItem = new Models.Partner();
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestdData.IconUrl = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestdData.IconUrlBase64)
                ? Images.SaveSingleImageOnServer(RequestdData.IconUrlBase64, 400, ImagesSavePath) : DbItem.IconUrl;

            if (RequestdData.Id.HasValue)
            {
                DbItem = _unitOfWork.Partners.GetById(RequestdData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestdData.Adapt(DbItem, RequestdData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Partners.Update(DbItem);
            }
            else
            {
                RequestdData.Adapt(DbItem, RequestdData.AddConfig(RequestOwner.Id));
                _unitOfWork.Partners.Add(DbItem);
            }

           await _unitOfWork.CompleteAsync();
           return await GetPartnerDetails(DbItem.Id);
        }
        public async Task<OperationOutput> ModelAction(Dtos.Partner RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.Partners.GetById(RequestedData.Id.Value);
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

            _unitOfWork.Partners.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
    }
}
