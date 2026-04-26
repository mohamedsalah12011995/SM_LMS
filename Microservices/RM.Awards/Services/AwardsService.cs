using RM.Core.Consts;
using Microsoft.EntityFrameworkCore;
using RM.Awards.Dtos;
using RM.Core.Helpers;
using static RM.Awards.Dtos.OperationOutput;
using RM.Awards.UnitOfWorks;
using RM.Models;
using Mapster;
using RM.Core.Services;
using RM.Core.Integrations;

namespace RM.Awards.Services
{
    public class AwardsService : BaseService, IAwardsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AwardsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetAwardsList(Dtos.Awards RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser)
                RequestedData.IsActive = true;

            var filteration = RequestedData.Filteration();

            var awards = await _unitOfWork.Awards.FindAllByPaginationAsync(filteration, RequestedData.Pagination, DefaultPaginationCount, x => x.AwardOrder ?? x.Id, OrderBy.Ascending,
                 x => x.CreatedByNavigation, x => x.UpdatedByNavigation);

            var awardsDto = awards.Data.Adapt<List<Dtos.Awards>>(Dtos.Awards.SelectConfig(ImagesGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.Awards, RequestedData.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.AwardsEntity, awardsDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Awards)),
                   new OutputDictionary(OperationOutputKeys.Pagination, awards.Pagination));
        }

        public async Task<OperationOutput> SaveAwards(Dtos.Awards RequestdData)
        {
            AmanahAward DbItem = new AmanahAward();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if(!Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestdData.IconUrlBase64))
                RequestdData.IconUrl= Images.SaveSingleImageOnServer(RequestdData.IconUrlBase64, 400, ImagesSavePath);

            if (RequestdData.Id.HasValue)
            {
                DbItem = await _unitOfWork.Awards.GetByIdAsync(RequestdData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestdData.Adapt(DbItem, RequestdData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Awards.Update(DbItem);
            }
            else
            {
                RequestdData.Adapt(DbItem, RequestdData.AddConfig(RequestOwner.Id));
                _unitOfWork.Awards.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();

            return await GetAwardsDetails(DbItem.Id);

        }


        public async Task<OperationOutput> GetAwardsDetails(Dtos.Awards RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            return await GetAwardsDetails(RequestedData.Id.Value);
        }
        public async Task<OperationOutput> GetAwardsDetails(int Id)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = _unitOfWork.Awards.GetAll()
                        .Where(x => x.Id == Id && x.IsDeleted != true)
                        .Where(x => (IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser) ? x.IsActive == true : true)
                        .AsNoTracking().FirstOrDefault();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<Dtos.Awards>(Dtos.Awards.SelectConfig(ImagesGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, (int)Enums.Entities.Awards, Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.AwardsEntity, Item),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Awards)));
        }
        public OperationOutput SortOrder(List<Dtos.Awards> RequestedData)
        {
            var DbItem = _unitOfWork.Awards.GetAll().Where(x => x.IsDeleted == false
             && RequestedData.Select(v => v.Id).Contains(x.Id)).ToList();
            foreach (var item in DbItem)
            {
                item.AwardOrder = RequestedData.Where(v => v.Id == item.Id).FirstOrDefault().AwardOrder;
                _unitOfWork.Awards.Update(item);
            }
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }
        public OperationOutput ModelActions(Dtos.Awards RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            AmanahAward DbItem = _unitOfWork.Awards.GetById(RequestedData.Id.Value);

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

            _unitOfWork.Awards.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }



    }
}
