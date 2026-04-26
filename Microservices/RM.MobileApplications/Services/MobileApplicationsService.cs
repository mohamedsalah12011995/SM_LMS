

using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using RM.MobileApplications.Dtos;
using RM.MobileApplications.UnitOfWorks;
using static RM.MobileApplications.Dtos.OperationOutput;

namespace RM.MobileApplications.Services
{
    public class MobileApplicationsService:BaseService, IMobileApplicationsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MobileApplicationsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            :base(httpContextAccessor,unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OperationOutput> GetMobileApplicationList(Dtos.MobileApplications RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            RequestedData.IsActive = RequestUserRole == Enums.UsersRoles.NormalUser ? true : RequestedData.IsActive;

            var result = _unitOfWork.MobileApplication.GetAll()
                .Where(RequestedData.Filteration())
                .Include(x => x.CreatedByNavigation)
                .Include(x => x.UpdatedByNavigation)
                .OrderByDescending(x => x.Id)
                .AsNoTracking().TakePaggination(RequestedData.Pagination, DefaultPaginationCount);

            var resultDto = result.Data.ToList().Adapt<List<Dtos.MobileApplications>>(Dtos.MobileApplications.SelectConfig(ImagesGetPath,DocumentsGetPath,ImagesSavePath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.MobileApplication, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.MobileApplicationsEntity, resultDto),
                   new OutputDictionary(OperationOutputKey.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.FAQ)),
                   new OutputDictionary(OperationOutputKey.Pagination, result.Pagination));
        }
        public async Task<OperationOutput> GetMobileApplicationDetails(Dtos.MobileApplications RequestedData)
        {
            return await GetMobileApplicationDetails(RequestedData.Id.Value);
        }
        public async Task<OperationOutput> GetMobileApplicationDetails(int Id)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var DbItem = await _unitOfWork.MobileApplication.GetAll()
                .Where(x => x.Id == Id && x.IsDeleted != true)
                .AsNoTracking().FirstOrDefaultAsync();

            if (DbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            DbItem.Attachments = await _unitOfWork.Attachment.GetAll().Where(x => x.ItemId == DbItem.Id && x.EntityId == (int) Enums.Entities.MobileApplication)
                .ToListAsync();
            DbItem.QuestionsAnswers = await _unitOfWork.QuestionsAnswer.GetAll().Where(x => x.ItemId == DbItem.Id && x.EntityId == (int)Enums.Entities.MobileApplication)
                .ToListAsync();

            var ItemDto = DbItem.Adapt<Dtos.MobileApplications>(Dtos.MobileApplications.SelectConfig(ImagesGetPath,DocumentsGetPath,ImagesSavePath));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, ItemDto.ReferenceId, (int)Enums.Entities.MobileApplication, ItemDto.Id, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKey.MobileApplicationsEntity, ItemDto));
        }

        public async Task<OperationOutput> SaveApplicationInformation(Dtos.MobileApplications RequestedData)
        {
            Models.MobileApplication DbItem = new Models.MobileApplication();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            UserManualsUpdate(RequestedData);

            if (RequestedData.Id.HasValue)
            {
                DbItem = await _unitOfWork.MobileApplication.GetAll().Where(x=>x.Id == RequestedData.Id.Value).FirstOrDefaultAsync();

                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                var Attachments = await _unitOfWork.Attachment.GetAll().Where(x => x.ItemId == DbItem.Id && x.EntityId == (int)Enums.Entities.MobileApplication)
                    .ToListAsync();
                var QuestionsAnswers = await _unitOfWork.QuestionsAnswer.GetAll().Where(x => x.ItemId == DbItem.Id && x.EntityId == (int)Enums.Entities.MobileApplication)
                    .ToListAsync();

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));
                AlbumImagesUpdate(RequestedData, Attachments);
                QuestionsAndAnswersUpdate(RequestedData, QuestionsAnswers);
                _unitOfWork.MobileApplication.Update(DbItem);
            }
            else
            {
                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id));
                AlbumImagesAdd(RequestedData, DbItem);
                QuestionsAndAnswersAdd(RequestedData, DbItem);
                _unitOfWork.MobileApplication.Add(DbItem);
            }

            await _unitOfWork.CompleteAsync();
            return await GetMobileApplicationDetails(DbItem.Id);
        }

        private void UserManualsUpdate(Dtos.MobileApplications RequestedData)
        {
                if(!Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.UserManualUrlArBase64))
                RequestedData.UserManualUrlAr = Files.SaveBase64FileToServer(Guid.NewGuid().ToString() + ".pdf", RequestedData.UserManualUrlArBase64, DocumentsSavePath);

                if(!Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.UserManualUrlEnBase64))
                RequestedData.UserManualUrlEn = Files.SaveBase64FileToServer(Guid.NewGuid().ToString() + ".pdf", RequestedData.UserManualUrlEnBase64, DocumentsSavePath);

                if(!Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.ImageUrlBase64))
                RequestedData.ImageUrl = Images.SaveSingleImageOnServer(RequestedData.ImageUrlBase64, null, ImagesSavePath, false);
        }

        private void QuestionsAndAnswersAdd(Dtos.MobileApplications RequestedData, Models.MobileApplication DbItem)
        {

            if (RequestedData.ListOfQuestions.Count > 0)
            {
                foreach (var Question in RequestedData.ListOfQuestions)
                {
                    var DbItemQuestionAnswer = new Models.QuestionsAnswer();
                    Question.Adapt(DbItemQuestionAnswer, Question.AddConfig(RequestOwner.Id));
                    DbItemQuestionAnswer.ItemId = DbItem.Id;
                    DbItem.QuestionsAnswers.Add(DbItemQuestionAnswer);
                }
            }

        }
        private void QuestionsAndAnswersUpdate(Dtos.MobileApplications RequestedData, List<Models.QuestionsAnswer> QuestionsAnswers)
        {
            var MustRemovedQuestions = QuestionsAnswers.Where(x => !RequestedData.ListOfQuestions.Select(v => v.Id.HasValue ? v.Id.Value : 0).Contains(x.Id)).ToList();

            var NewQuestions = RequestedData.ListOfQuestions.Where(x => !QuestionsAnswers.Select(v => v.Id).Contains(x.Id.HasValue ? x.Id.Value : 0)).ToList();

            var MustEditedQuestions = RequestedData.ListOfQuestions.Where(x => QuestionsAnswers.Select(v => v.Id).Contains(x.Id.HasValue ? x.Id.Value : 0)).ToList();

            if (MustRemovedQuestions.Count > 0)
            {
                foreach (var Question in MustRemovedQuestions)
                {
                    _unitOfWork.QuestionsAnswer.Delete(Question);
                }
            }
            if (MustEditedQuestions.Count > 0)
            {
                foreach (var Question in MustEditedQuestions)
                {
                    var DbItemQuestionAnswer = QuestionsAnswers.Where(x => x.Id == Question.Id).FirstOrDefault();

                    Question.Adapt(DbItemQuestionAnswer, Question.UpdateConfig(RequestOwner.Id));
                    _unitOfWork.QuestionsAnswer.Update(DbItemQuestionAnswer);
                }
            }

            if (NewQuestions.Count > 0)
            {
                foreach (var Question in NewQuestions)
                {
                    var DbItemQuestionAnswer = new Models.QuestionsAnswer();
                    Question.Adapt(DbItemQuestionAnswer, Question.AddConfig(RequestOwner.Id));
                    DbItemQuestionAnswer.ReferenceId = RequestedData.ReferenceId;
                    DbItemQuestionAnswer.ItemId = RequestedData.Id;
                    _unitOfWork.QuestionsAnswer.Add(DbItemQuestionAnswer);
                }
            }

        }

        private void AlbumImagesAdd(Dtos.MobileApplications RequestedData, Models.MobileApplication DbItem)
        {
            if (RequestedData.ListOfImage.Count > 0)
            {
                foreach (var Image in RequestedData.ListOfImage)
                {
                    var DbItemAttachment = new Models.Attachment();
                    Image.Adapt(DbItemAttachment, Image.AddConfig(RequestOwner.Id, ImagesSavePath, ThumbsSavePath));
                    DbItemAttachment.ReferenceId = RequestedData.ReferenceId;
                    DbItemAttachment.ItemId = DbItem.Id;    
                    DbItem.Attachments.Add(DbItemAttachment);
                }
            }
        }
        private void AlbumImagesUpdate(Dtos.MobileApplications RequestedData,List<Models.Attachment> Attachments)
        {
            var MustRemovedImages = Attachments.Where(x => !RequestedData.ListOfImage.Select(v => v.Id).Contains(x.Id)).ToList();
            var NewImages = RequestedData.ListOfImage.Where(x => !Attachments.Select(v => v.Id).Contains(x.Id.HasValue ? x.Id.Value : 0)).ToList();

            if (MustRemovedImages.Count > 0)
            {
                foreach (var Image in MustRemovedImages)
                {
                    _unitOfWork.Attachment.Delete(Image);
                }
            }

            if (NewImages.Count > 0)
            {
                foreach (var Image in NewImages)
                {
                    var DbItemAttachment = new Models.Attachment();
                    Image.Adapt(DbItemAttachment, Image.AddConfig(RequestOwner.Id, ImagesSavePath, ThumbsSavePath));
                    DbItemAttachment.ReferenceId = RequestedData.ReferenceId;
                    DbItemAttachment.ItemId = RequestedData.Id;
                    _unitOfWork.Attachment.Add(DbItemAttachment);
                }
            }
        }

        public async Task<OperationOutput> ModelAction(Dtos.MobileApplications RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if ((!RequestedData.IsActive.HasValue && !RequestedData.IsDeleted.HasValue) || !RequestedData.Id.HasValue)
                return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);

            var DbItem = _unitOfWork.MobileApplication.GetById(RequestedData.Id.Value);
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

            _unitOfWork.MobileApplication.Update(DbItem);
            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }
    }
}
