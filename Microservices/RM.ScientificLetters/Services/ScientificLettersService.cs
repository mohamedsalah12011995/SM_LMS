
using RM.ScientificLetters.Dtos;
using RM.ScientificLetters.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using RM.Core.Services;
using static RM.ScientificLetters.Dtos.OperationOutput;
using RM.Core.Helpers;
using RM.Core.Integrations;
using Mapster;
using RM.Core.Consts;

namespace RM.ScientificLetters.Services
{
    public class ScientificLettersService:BaseService,IScientificLettersService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ScientificLettersService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor,unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetScientificLettersLookups()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var qualifications = _unitOfWork.MajorLookups.GetAll().Where(c => c.TypeId == (int)Enums.MajorLookupsTypes.ScientificLettersDegree)
                .AsNoTracking().ToList().Adapt<List<Dtos.Qualification>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                        new OutputDictionary(OperationOutputKeys.Qualification, qualifications));
        }
        public async Task<OperationOutput> GetScientificLettersList(Dtos.ScientificLetters RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var result = await _unitOfWork.ScientificLetters.FindAllByPaginationAsync(RequestedData.Filteration(), RequestedData.Pagination, DefaultPaginationCount, x => x.Id, OrderBy.Descending,
                c => c.CreatedByNavigation, u => u.UpdatedByNavigation, a => a.ActivatedByNavigation, d => d.DeletedByNavigation, Q => Q.Degree);

            var resultDto = result.Data.Adapt<List<Dtos.ScientificLetters>>(Dtos.ScientificLetters.SelectConfig(FilesGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.ScientificLetters, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ScientificLettersEntity, resultDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.ScientificLetters)),
                   new OutputDictionary(OperationOutputKeys.Pagination, result.Pagination));
        }
        public async Task<OperationOutput> GetScientificLettersDetails(Dtos.ScientificLetters RequestedData)
        {
            return await GetScientificLetterDetails(RequestedData.Id.Value);
        }
        public async Task<OperationOutput> GetScientificLetterDetails(int Id)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var Item = _unitOfWork.ScientificLetters.GetAll()
                .Include(x=>x.Degree).Where(x => x.Id == Id && x.IsDeleted != true)
                .AsNoTracking().FirstOrDefault();
            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            var ItemDto = Item.Adapt<Dtos.ScientificLetters>(Dtos.ScientificLetters.SelectConfig(FilesGetPath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, (int)Enums.Entities.ScientificLetters, Item.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ScientificLettersEntity, ItemDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.ScientificLetters)));
        }
 
        public async Task<OperationOutput> SaveScientificLetters(Dtos.ScientificLetters RequestedData)
        {
            Models.ScientificLetters DbItem = new Models.ScientificLetters();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.FileBase64))
            {
                string FileName = Strings.GenerateGUID() + "." + RequestedData.FileType;
                Files.SaveBase64FileToServer(FileName, RequestedData.FileBase64, DocumentsSavePath);
                RequestedData.FileName = FileName;
            }

            if (RequestedData.Id.HasValue)
            {
                DbItem = _unitOfWork.ScientificLetters.GetById(RequestedData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.ScientificLetters.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
                _unitOfWork.ScientificLetters.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            return await GetScientificLetterDetails(DbItem.Id);
        }

        public async Task<OperationOutput> ModelActions(Dtos.ScientificLetters RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.ScientificLetters.GetById(RequestedData.Id.Value);
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

            _unitOfWork.ScientificLetters.Update(DbItem);
            await _unitOfWork.CompleteAsync();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }
    }
}
