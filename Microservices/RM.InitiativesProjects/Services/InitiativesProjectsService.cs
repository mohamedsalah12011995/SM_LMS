

using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using RM.Models;
using RM.InitiativesProjects.Dtos;
using RM.InitiativesProjects.UnitOfWorks;
using static RM.InitiativesProjects.Dtos.OperationOutput;

namespace RM.InitiativesProjects.Services
{
    public class InitiativesProjectsService:BaseService, IInitiativesProjectsService
    {

        private readonly IUnitOfWork _unitOfWork;
        public InitiativesProjectsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetLookups()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var ItemBeneficiaries = _unitOfWork.Beneficiary.GetAll().Where(x => x.IsDeleted == false).AsNoTracking().ToList()
                .Adapt<List<Dtos.Beneficiaries>>();

            var ItemInitiativesProjectsTypes = _unitOfWork.InitiativesProjectsType.GetAll().Where(x => x.IsDeleted == false).AsNoTracking().ToList()
                .Adapt<List<Dtos.InitiativesProjectsType>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.InitiativesProjectsTypeEntity, ItemInitiativesProjectsTypes),
                   new OutputDictionary(OperationOutputKeys.BeneficiariesEntity, ItemBeneficiaries));
        }
        public async Task< OperationOutput > GetInitiativesProjectList(Dtos.InitiativesProjects RequestedData)
        {

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            var InitiativesProjects = await _unitOfWork.InitiativesProject.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending, x => x.InitiativesProjectsBeneficiaries);

            var InitiativesProjectDto = InitiativesProjects.Data.Adapt<List<Dtos.InitiativesProjects>>(Dtos.InitiativesProjects.SelectConfig(true));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.Articles, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.InitiativesProjectsEntity, InitiativesProjectDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.InitiativesProjects)),
                   new OutputDictionary(OperationOutputKeys.Pagination, InitiativesProjects.Pagination));

        }
        private async Task< OperationOutput> GetInitiativesProjectDetails(int? Id)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

           int CommentsCount = _unitOfWork.Comments.GetAll().Where(x => x.ItemId == Id).Count();

           var DbItem = _unitOfWork.InitiativesProject.GetAll()
                .Include(x => x.InitiativesProjectsBeneficiaries)
                .ThenInclude(x=>x.Beneficiary)
                .Where(x => x.Id == Id && x.IsDeleted != true)
                .Where(x => (IsPortal || RequestUserRole == Enums.UsersRoles.NormalUser) ? x.IsActive == true : true)
                .AsNoTracking().FirstOrDefault();

            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var Item = DbItem.Adapt<Dtos.InitiativesProjects>(Dtos.InitiativesProjects.SelectConfig(false,CommentsCount));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, (int)Enums.Entities.InitiativesProjects, Item.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.InitiativesProjectsEntity, Item),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.InitiativesProjects)));
        }
        public async Task< OperationOutput> GetInitiativesProjectDetails(Dtos.InitiativesProjects RequestedData)
        {
            return await GetInitiativesProjectDetails(RequestedData.Id);
        }

        public async Task<OperationOutput> SaveInitiativesProject(Dtos.InitiativesProjects RequestedData)
        {
            OperationOutput Result = new OperationOutput();
            Models.InitiativesProject DbItem = new Models.InitiativesProject();
            List<Dtos.Beneficiaries> MustRemovedBeneficiaries = new List<Dtos.Beneficiaries>();
            List<Dtos.Beneficiaries> NewBeneficiaries = new List<Dtos.Beneficiaries>();
            Models.InitiativesProjectsBeneficiary DbItemInitiativesProjectsBeneficiary;
            bool IsNew = true;
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.Id.HasValue)
            {
                IsNew = false;
                DbItem = _unitOfWork.InitiativesProject.GetById(RequestedData.Id.Value);
                DbItem.InitiativesProjectsBeneficiaries = _unitOfWork.InitiativesProject.GetAll().Where(x => x.Id == RequestedData.Id)
                    .SelectMany(x => x.InitiativesProjectsBeneficiaries).AsNoTracking().ToList().Adapt<List<InitiativesProjectsBeneficiary>>();
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));
            }
            else
            {
                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
                DbItem.EntityId = (int)Enums.Entities.InitiativesProjects;
            }



            #region BeneficiariesManagment
            MustRemovedBeneficiaries = DbItem.InitiativesProjectsBeneficiaries.Where(x => !RequestedData.ListOfBeneficiaries.Select(v => v.Id).Contains(x.Id))
                .ToList().Adapt<List<Beneficiaries>>();

            NewBeneficiaries = RequestedData.ListOfBeneficiaries.Where(x => !DbItem.InitiativesProjectsBeneficiaries.Select(v => v.Id).Contains(x.Id.HasValue ? x.Id.Value : 0))
                .ToList().Adapt<List<Beneficiaries>>();

            if (MustRemovedBeneficiaries.Count > 0)
            {
                foreach (var Item in MustRemovedBeneficiaries)
                {
                    var ItemToRemove = DbItem.InitiativesProjectsBeneficiaries.Where(x => x.Id == Item.Id).FirstOrDefault();
                    DbItem.InitiativesProjectsBeneficiaries.Remove(ItemToRemove);
                    _unitOfWork.InitiativesProjectsBeneficiary.Delete(ItemToRemove);
                }
            }
            if (NewBeneficiaries.Count > 0)
            {
                foreach (var Item in NewBeneficiaries)
                {
                    DbItemInitiativesProjectsBeneficiary = new Models.InitiativesProjectsBeneficiary();
                    DbItemInitiativesProjectsBeneficiary.BeneficiaryId = Item.Id;
                    DbItemInitiativesProjectsBeneficiary.InitiativesProjectId = RequestedData.Id;
                    DbItem.InitiativesProjectsBeneficiaries.Add(DbItemInitiativesProjectsBeneficiary);
                }
            }
            #endregion

            if (IsNew) _unitOfWork.InitiativesProject.Add(DbItem);
            else _unitOfWork.InitiativesProject.Update(DbItem);
            await _unitOfWork.CompleteAsync();
            return await GetInitiativesProjectDetails(DbItem.Id);
           
        }
        public async Task<OperationOutput> ModelActions(Dtos.InitiativesProjects RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);


            var DbItem = _unitOfWork.InitiativesProject.GetById(RequestedData.Id.Value);
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

            _unitOfWork.InitiativesProject.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
    }
}
