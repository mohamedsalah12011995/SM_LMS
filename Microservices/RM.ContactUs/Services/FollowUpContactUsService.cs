using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.ContactUs.Dtos;
using RM.ContactUs.UnitOfWorks;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Services;
using RM.Models;
using static RM.ContactUs.Dtos.OperationOutput;

namespace RM.ContactUs.Services
{
    public class FollowUpContactUsService : BaseService, IFollowUpContactUsService
    {
        private readonly IUnitOfWork _unitOfWork;
        int HeadQuartiersRefernceId = 0;
        int DepartmentRefernceId = 0;
        public FollowUpContactUsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;
            HeadQuartiersRefernceId = int.Parse(_unitOfWork.Configuration.GetSection("AppSettings").GetSection("HeadQuartiers").Value!);
            DepartmentRefernceId = int.Parse(_unitOfWork.Configuration.GetSection("AppSettings").GetSection("Departments").Value!);
        }

        public async Task<OperationOutput> GetContactListForManager(Dtos.ContactUs RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var filteration = RequestedData.ManagerContactUsFilteration();

            var ContactUsList = await _unitOfWork.ContactUs.FindAllByPaginationAsync(filteration, RequestedData.Pagination,
                DefaultPaginationCount, x => x.Id, OrderBy.Descending, c => c.LastUserAction,
                c => c.LastUserAction.Reference, c => c.LastStatus, c => c.LastReferenceAction);

            var ContactUsListDto = ContactUsList.Data.Adapt<List<Dtos.ContactUs>>(Dtos.ContactUs.SelectFollowUpConfig(false));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ContactUsEntity, ContactUsListDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, ContactUsListDto.Count > 0 ? ContactUsListDto[0].EntityId! : string.Empty),
                   new OutputDictionary(OperationOutputKeys.Pagination, ContactUsList.Pagination!));
        }

        public OperationOutput AddAction(Dtos.Action action)
        {

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var userDbItem = _unitOfWork.User.GetById(RequestOwner.Id!.Value);
            var item = new Models.Actions();
            item.ContactId = action.ContactId!.Value;
            item.CreatedBy = userDbItem.Id;
            item.CreatedDate = TransactionDate;
            item.Note = action.Note;
            item.StatusId = action.StatusId;
            item.ToReferenceId = action.ToReferenceId;
            item.FromUserId = userDbItem.Id;
            _unitOfWork.Actions.Add(item);
            _unitOfWork.Complete();

            #region save action files 
            if (action.ActionFiles.Any())
            {
                SaveActionFiles(action, item);
            }
            #endregion

            action.Id = item.Id;
            action.FromUserId = item.FromUserId;
            action.CreatedDate = item.CreatedDate;
            action.LastActionisOfficer = (Enums.JobRole)userDbItem.JobRole! == Enums.JobRole.FollowUpContactUs;

            var complaint = _unitOfWork.ContactUs.GetAll().FirstOrDefault(c => c.Id == action.ContactId.Value);
            var feedback = _unitOfWork.Feedback.GetAll().Where(c => c.ContactUsId == complaint!.Id).OrderByDescending(c => c.Id).FirstOrDefault();
            if (complaint != null)
            {
                complaint.LastStatusDate = item.CreatedDate;
                complaint.LastStatusId = item.StatusId;
                complaint.LastActionId = item.Id;
                complaint.LastActionUser = item.FromUserId;
                complaint.LastActionReference = item.ToReferenceId;

                if (action.StatusId == (int)Enums.ContactStatus.Done || action.StatusId == (int)Enums.ContactStatus.Returned)
                    complaint.LastActionReference = userDbItem.ReferenceId;

                complaint.TransferFromManager = ((item.StatusId == (int)Enums.ContactStatus.UnderProcess
                    && (Enums.JobRole)userDbItem.JobRole == Enums.JobRole.ContactUsManagement))
                    || ((item.StatusId == (int)Enums.ContactStatus.Returned
                    && (Enums.JobRole)userDbItem.JobRole == Enums.JobRole.ContactUsManagement)) ? true : false;
            }

            if (complaint!.LastStatusId == (int)Enums.ContactStatus.Closed && feedback != null)
                feedback.IsClosed = true;

            _unitOfWork.Complete();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.Action, action));
        }

        private void SaveActionFiles(Dtos.Action action, Actions item)
        {

            foreach (var actionFile in action.ActionFiles)
            {
                var afile = new Models.ActionFiles();
                afile.ActionId = item.Id;
                afile.FileName = actionFile.FileName.Split('.')[0];
                afile.FileType = actionFile.FileType;

                if (!Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(actionFile.FileUrlBase64))
                {
                    if (!String.IsNullOrEmpty(actionFile.FileType) && actionFile.FileType.Contains("image"))
                        afile.FileUrl = Images.SaveSingleImageOnServer(actionFile.FileUrlBase64, null, ImagesSavePath, false);
                    else
                        afile.FileUrl = Files.SaveBase64FileToServer(Strings.GenerateGUID() + "." + actionFile.FileType, actionFile.FileUrlBase64, FilesSavePath);
                }
                item.ActionFiles.Add(afile);
            }
        }

        public async Task<OperationOutput> GetContactListForFollowUpUser(Dtos.ContactUs RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var filteration = RequestedData.FollowUpFilteration();

            var ContactUsList = await _unitOfWork.ContactUs.FindAllByPaginationAsync(filteration, RequestedData.Pagination,
                DefaultPaginationCount, x => x.Id, OrderBy.Descending,
                c => c.LastUserAction, c => c.LastUserAction.Reference,
                c => c.LastStatus, c => c.LastReferenceAction);

            var ContactUsListDto = ContactUsList.Data.Adapt<List<Dtos.ContactUs>>(Dtos.ContactUs.SelectFollowUpConfig(false));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ContactUsEntity, ContactUsListDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, ContactUsListDto.Count > 0 ? ContactUsListDto[0].EntityId! : string.Empty),
                   new OutputDictionary(OperationOutputKeys.Pagination, ContactUsList.Pagination!));
        }

        public OperationOutput GetFollowUpOfficerLookup()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var headQuartiers = _unitOfWork.References.GetAll(r => r.ParentId == HeadQuartiersRefernceId)
                .AsNoTracking().ToList().Adapt<List<LookupItem>>();


            var departments = _unitOfWork.References.GetAll(r => r.ParentId == DepartmentRefernceId)
               .AsNoTracking().ToList().Adapt<List<LookupItem>>();


            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.HeadQuartiers, headQuartiers),
                   new OutputDictionary(OperationOutputKeys.Departments, departments));

        }
        public async Task<OperationOutput> GetComplaintListForProccessorUser(Dtos.ContactUs RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var filteration = RequestedData.ProccessingFilteration();

            var ContactUsList = await _unitOfWork.ContactUs.FindAllByPaginationAsync(filteration, RequestedData.Pagination,
                DefaultPaginationCount, x => x.Id, OrderBy.Descending, c => c.LastUserAction, c => c.LastUserAction.Reference, c => c.LastStatus, c => c.LastReferenceAction);

            var ContactUsListDto = ContactUsList.Data.Adapt<List<Dtos.ContactUs>>(Dtos.ContactUs.SelectFollowUpConfig(false));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ContactUsEntity, ContactUsListDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, ContactUsListDto.Count > 0 ? ContactUsListDto[0].EntityId! : string.Empty),
                   new OutputDictionary(OperationOutputKeys.Pagination, ContactUsList.Pagination!));

        }

        public OperationOutput GetProccessorUserLookup()
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            List<LookupItem> childRefernces = null;
            User userDbItem = _unitOfWork.User.GetById(RequestOwner.Id!.Value);
            if (userDbItem != null)
            {
                childRefernces = _unitOfWork.References.GetAll(r => r.ParentId == userDbItem.ReferenceId)
                   .AsNoTracking().ToList()
                   .Adapt<List<LookupItem>>();
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.ChildReferences, childRefernces));
        }

        public OperationOutput GetContactActions(Dtos.ContactUs RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var actions = _unitOfWork.Actions.GetAll(c => c.ContactId == RequestedData.Id)
                .Include(c => c.Status)
                .Include(c => c.FromUser)
                .ThenInclude(c => c.Reference)
                .Include(c => c.ToReference)
                .Include(c => c.ActionFiles)
                .OrderBy(c => c.CreatedDate).ToList();

            var actionsDto = actions.Adapt<List<Dtos.Action>>(Dtos.Action.SelectConfig(ImagesGetPath, FilesGetPath, false));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.ContactActions, actionsDto));
        }

        private string GenerateFileUrl(Models.ActionFiles actionFiles)
        {
            if (actionFiles.FileType.Contains("image"))
                return $"{ImagesGetPath}/{actionFiles.FileUrl}";
            else if (actionFiles.FileType.Contains("pdf") || actionFiles.FileType.Contains("zip"))
                return $"{FilesGetPath}/{actionFiles.FileUrl}";
            else return string.Empty;
        }

        public async Task<OperationOutput> AddEvaluation(Dtos.Feedback RequestedData)
        {
            Models.Feedback feedback = new Models.Feedback();
            var contact = _unitOfWork.ContactUs.Find(x => x.Id == RequestedData.ContactUsId);
            if (contact != null)
            {
                feedback.ContactUsId = RequestedData.ContactUsId;
                feedback.IsPositive = RequestedData.IsPositive!.Value;
                feedback.Note = RequestedData.Note!;
                feedback.EvaluationDate = TransactionDate;
                feedback.IsClosed = RequestedData.IsPositive.Value ? true : false;

                contact.ProcessingTimesCount = contact.ProcessingTimesCount == null ? 1 : contact.ProcessingTimesCount + 1;
                _unitOfWork.Feedback.Add(feedback);
                await _unitOfWork.CompleteAsync();
            }

            RequestedData.Id = feedback.Id;
            RequestedData.EvaluationDate = feedback.EvaluationDate;

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.FeedbackEntity, RequestedData));

        }

        public async Task<OperationOutput> GetContactListForQualityAssurance(Dtos.ContactUs RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var filteration = RequestedData.QualityAssuranceFilteration();

            var ContactUsList = await _unitOfWork.ContactUs.FindAllByPaginationAsync(filteration, RequestedData.Pagination,
                DefaultPaginationCount, x => x.Id, OrderBy.Descending, c => c.LastUserAction, c => c.LastUserAction.Reference, c => c.LastStatus, c => c.LastReferenceAction, c => c.Feedbacks);

            var ContactUsListDto = ContactUsList.Data.Adapt<List<Dtos.ContactUs>>(Dtos.ContactUs.SelectFollowUpConfig(false));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ContactUsEntity, ContactUsListDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, ContactUsListDto.Count > 0 ? ContactUsListDto[0].EntityId! : string.Empty),
                   new OutputDictionary(OperationOutputKeys.Pagination, ContactUsList.Pagination!));

        }

        public OperationOutput GetContactFeedbacks(Dtos.ContactUs RequestedData)
        {
            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var feedbacks = _unitOfWork.Feedback.GetAll()
                .Where(c => c.ContactUsId == RequestedData.Id && !c.IsPositive)
                .OrderBy(c => c.EvaluationDate)
                .Adapt<List<Dtos.Feedback>>(Dtos.Feedback.SelectConfig(false));

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                     new OutputDictionary(OperationOutputKeys.ContactFeedbacks, feedbacks));
        }
    }
}
