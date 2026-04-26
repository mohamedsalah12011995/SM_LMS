using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Officials.Dtos;
using RM.Officials.Integrations;
using RM.Officials.UnitOfWorks;

using static RM.Officials.Dtos.OperationOutput;

namespace RM.Officials.Services
{
    public class OfficialService : BaseService, IOfficialService
    {

        private readonly IUnitOfWork _unitOfWork;
        public OfficialService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetOfficialsList(OfficialDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            var officials = await _unitOfWork.Officials.FindAllByPagination(RequestedData.Filteration(),
                        RequestedData.Pagination, DefaultPaginationCount, x => x.OfficialOrder,
                        OrderBy.Descending, x => x.CreatedByNavigation, c => c.UpdatedByNavigation);

            var officialsDto = officials.Data.Adapt<List<OfficialDto>>(OfficialDto.SelectConfig(RequestUserRole, ThumbsGetPath, ImagesGetPath, DocumentsGetPath));


            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, RequestedData.ReferenceId, RequestedData.EntityId, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
          new OutputDictionary(OperationOutputKeys.OfficialsEntity, officialsDto),
          new OutputDictionary(OperationOutputKeys.EntityID, officialsDto.Any() ? Cryptography.AES.Encrypt(officialsDto[0].entityId) : string.Empty),
          new OutputDictionary(OperationOutputKeys.Pagination, officials.Pagination));

        }

        public async Task<OperationOutput> GetOfficialDetails(OfficialDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var official = await _unitOfWork.Officials.GetAll().AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == RequestedData.Id);

            var officialDto = official.Adapt<OfficialDto>(OfficialDto.SelectConfig(RequestUserRole, ThumbsGetPath,
                ImagesGetPath, DocumentsGetPath));


            if (RequestUserRole == Enums.UsersRoles.NormalUser)
                await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, officialDto.ReferenceId, officialDto.EntityId, officialDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                  new OutputDictionary(OperationOutputKeys.OfficialsEntity, officialDto),
                  new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt(officialDto.entityId)));
        }


        public async Task<OperationOutput> SaveOfficial(OfficialDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.OriginalPicBase64 != null)
                RequestedData.OriginalPic = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.OriginalPicBase64) ?
                    Images.SaveSingleImageOnServer(RequestedData.OriginalPicBase64, null, ImagesSavePath, true, 400, ThumbsSavePath) : null;

            if (RequestedData.CvUrlBase64 != null)
                RequestedData.CvUrl = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.CvUrlBase64) ?
                    Files.SaveBase64FileToServer(Guid.NewGuid().ToString() + ".pdf", RequestedData.CvUrlBase64, DocumentsSavePath) : null;


            Models.Official official = new();

            if (RequestedData.Id.HasValue)
            {
                official = _unitOfWork.Officials.GetById(RequestedData.Id.Value);

                if (official is null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(official, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.Officials.Update(official);
            }
            else
            {
                RequestedData.Adapt(official, RequestedData.AddConfig(RequestOwner.Id));
                _unitOfWork.Officials.Add(official);
            }

            await _unitOfWork.CompleteAsync();

            return await GetOfficialDetails(new OfficialDto { Id = official.Id });

        }

        public async Task<OperationOutput> SortOrder(List<OfficialDto> RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var Officials = await _unitOfWork.Officials.GetAll().Where(x => x.IsDeleted == false
             && RequestedData.Select(v => v.Id).Contains(x.Id)).AsNoTracking().ToListAsync();

            foreach (var item in Officials)
            {
                var officialItem = RequestedData.Where(v => v.Id == item.Id).FirstOrDefault();
                if (officialItem is not null)
                {
                    item.OfficialOrder = officialItem.OfficialOrder;
                    _unitOfWork.Officials.Update(item);
                }
            }
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
        public async Task<OperationOutput> ModelAction(OfficialDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            if (RequestedData.IsDeleted.HasValue)
            {
                await _unitOfWork.Officials.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                       sett => sett.SetProperty(x => x.IsDeleted, RequestedData.IsDeleted.Value)
                       .SetProperty(d => d.DeletedBy, RequestOwner.Id)
                       .SetProperty(y => y.DeletedDate, TransactionDate)
                       .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                       .SetProperty(y => y.UpdatedDate, TransactionDate));
            }
            if (RequestedData.IsActive.HasValue)
            {
                await _unitOfWork.Officials.ExecuteUpdateAsync(x => x.Id == RequestedData.Id,
                        sett => sett.SetProperty(x => x.IsActive, RequestedData.IsActive.Value)
                        .SetProperty(d => d.ActivatedBy, RequestOwner.Id)
                        .SetProperty(y => y.ActivatedDate, TransactionDate)
                        .SetProperty(y => y.UpdatedBy, RequestOwner.Id)
                        .SetProperty(y => y.UpdatedDate, TransactionDate));
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }

    }
}
