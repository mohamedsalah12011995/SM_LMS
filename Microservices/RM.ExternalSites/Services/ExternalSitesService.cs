
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RM.ExternalSites.UnitOfWorks;
using RM.Core.Services;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.ExternalSites.Dtos;
using static RM.ExternalSites.Dtos.OperationOutput;
using Mapster;
using RM.Core.Consts;
using RM.Models;

namespace RM.ExternalSites.Services
{
    public class ExternalSitesService:BaseService, IExternalSitesService
    {

        private readonly IUnitOfWork _unitOfWork;
        public ExternalSitesService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<OperationOutput> GetCategories(Dtos.ExternalSites RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var categs = _unitOfWork.ExternalSite.GetAll().Where(x => x.ParentId == null && x.ReferenceId == RequestedData.ReferenceId && x.IsDeleted == false).ToList();
            var categsDto = categs.Adapt<List<Dtos.ExternalSites>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
            new OutputDictionary(OperationOutputKeys.ExternalSitesEntity, categsDto));
        }
        public async Task<OperationOutput> GetExternalSites(Dtos.ExternalSites RequestedData)
        {

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            var ExternalSites = _unitOfWork.ExternalSite.GetAll().Where(x => x.ParentId == null && x.ReferenceId == RequestedData.ReferenceId && x.IsDeleted != true)
                .Where(x => RequestedData.Id.HasValue ? x.ParentId == RequestedData.Id : true)
                .Include(x => x.InverseParent).ToList();

            var ExternalSitesDto = ExternalSites.Adapt<List<Dtos.ExternalSites>>(Dtos.ExternalSites.SelectConfig( GetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.ExternalSites, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ExternalSitesEntity, ExternalSitesDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Articles)));

        }
        public async Task< OperationOutput> GetExternalSitesList(Dtos.ExternalSites RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            var ExternalSites = await _unitOfWork.ExternalSite.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending, 
                x => x.CreatedByNavigation, x => x.UpdatedByNavigation,x => x.Parent);

            var ExternalSitesDto = ExternalSites.Data.Adapt<List<Dtos.ExternalSites>>(Dtos.ExternalSites.SelectConfig( GetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.ExternalSites, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ExternalSitesEntity, ExternalSitesDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.Articles)),
                   new OutputDictionary(OperationOutputKeys.Pagination, ExternalSites.Pagination));

        }
        public async Task< OperationOutput> GetExternalSitesDetails(Dtos.ExternalSites RequestedData)
        {
            return await GetExternalSitesDetails(RequestedData.Id.Value);
        }
        public async Task< OperationOutput> GetExternalSitesDetails(int Id)
        {
            ExternalSite? DbItem = new ExternalSite();
            Dtos.ExternalSites Item = new Dtos.ExternalSites();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            DbItem = _unitOfWork.ExternalSite.GetAll()
                .Include(x => x.Parent)
                .Where(x => x.Id == Id && x.IsDeleted != true)
                .Where(x => (IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser) ? x.IsActive == true : true)
                .AsNoTracking().FirstOrDefault();

            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Item = DbItem.Adapt<Dtos.ExternalSites>(Dtos.ExternalSites.SelectConfig(GetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, (int)Enums.Entities.ExternalSites, Item.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ExternalSitesEntity, Item),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.ExternalSites)));

        }
        public async Task<OperationOutput> SaveExternalSite(Dtos.ExternalSites RequestedData)
        {

            Models.ExternalSite DbItem = new Models.ExternalSite();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.ImageBase64 != null)
                RequestedData.Image = !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.ImageBase64)
                    ? Images.SaveSingleImageOnServer(RequestedData.ImageBase64, null, ImagesSavePath, false) : null;

            if (RequestedData.Id.HasValue)
            {
                DbItem = _unitOfWork.ExternalSite.GetById(RequestedData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.ExternalSite.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
                _unitOfWork.ExternalSite.Add(DbItem);
            }


            await _unitOfWork.CompleteAsync();
            return await GetExternalSitesDetails(DbItem.Id);

        }
        public async Task<OperationOutput> ModelActions(Dtos.ExternalSites RequestedData)
        {
            ExternalSite DbItem;
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            DbItem = _unitOfWork.ExternalSite.GetById(RequestedData.Id.Value);
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

            _unitOfWork.ExternalSite.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
    }
}
