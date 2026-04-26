using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.ContactUs.Dtos;
using RM.ContactUs.UnitOfWorks;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using RM.Models;
using static RM.ContactUs.Dtos.OperationOutput;

namespace RM.ContactUs.Services
{
    public class ContactUsService : BaseService, IContactUsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private string Amana940_UserName;
        private string Amana940_Password;
        private string Amana940Complain_UserName;
        private string Amana940Complain_Password;
        int HeadQuartiersRefernceId = 0;
        int DepartmentRefernceId = 0;
        int AutomatedUserId = 0;

        public ContactUsService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {
            _unitOfWork = unitOfWork;

            Amana940_UserName = _unitOfWork.Configuration.GetSection("Amana940Credentials").GetSection("UserName").Value;
            Amana940_Password = _unitOfWork.Configuration.GetSection("Amana940Credentials").GetSection("Password").Value;
            Amana940Complain_UserName = _unitOfWork.Configuration.GetSection("Amana940Credentials").GetSection("Amana940Complain_UserName").Value;
            Amana940Complain_Password = _unitOfWork.Configuration.GetSection("Amana940Credentials").GetSection("Amana940Complain_Password").Value;

            HeadQuartiersRefernceId = int.Parse(_unitOfWork.Configuration.GetSection("AppSettings").GetSection("HeadQuartiers").Value);
            DepartmentRefernceId = int.Parse(_unitOfWork.Configuration.GetSection("AppSettings").GetSection("Departments").Value);
            AutomatedUserId = int.Parse(_unitOfWork.Configuration.GetSection("AppSettings").GetSection("AutomatedUserId").Value);
        }


        public async Task<OperationOutput> GetContactUsList(Dtos.ContactUs RequestedData)
        {

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var filteration = RequestedData.ContactUsFilteration();

            var ContactUsList = await _unitOfWork.ContactUs
                .FindAllByPaginationAsync(filteration, RequestedData.Pagination,
                DefaultPaginationCount, x => x.Id, OrderBy.Descending, c => c.CreatedByNavigation,
                c => c.LastUserAction, criteria => criteria.LastUserAction.Reference, criteria => criteria.Actions, c => c.LastStatus, c => c.LastStatus.MajorStatus);

            var ContactUsListDto = ContactUsList.Data.Adapt<List<Dtos.ContactUs>>(Dtos.ContactUs.SelectContactUsConfig(VideosGetPath, false));

            if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, RequestedData.ReferenceId, (int)Enums.Entities.ContactUs, null, Enums.InteractionStatisticsType.ViewsCount);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ContactUsEntity, ContactUsListDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.ContactUs)),
                   new OutputDictionary(OperationOutputKeys.Pagination, ContactUsList.Pagination));

        }
        public async Task<OperationOutput> GetContactUsDetails(Dtos.ContactUs RequestedData)
        {
            Integrations.ContactMayor contactMayor = new();
            Integrations.RM940Complain contact940 = new(Amana940Complain_UserName, Amana940Complain_Password);
            Dtos.ContactUs Item = new Dtos.ContactUs();

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            switch (RequestedData.EntityId)
            {
                case (int)Enums.Entities.ContactUs_Mayor:
                    {
                        Item = contactMayor.GetMayorComplain(RequestedData);
                        if (Item == null)
                            return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);
                        if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, (int)Enums.Entities.ContactUs_Mayor, Item.Id, Enums.InteractionStatisticsType.ViewsCount);
                        break;
                    }
                case (int)Enums.Entities.ContactUs_940:
                    {
                        Item = await contact940.GetComplain(RequestedData);
                        if (Item == null)
                            return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);
                        if (RequestUserRole == Enums.UsersRoles.NormalUser) await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, this.Token, Item.ReferenceId, (int)Enums.Entities.ContactUs_940, Item.Id, Enums.InteractionStatisticsType.ViewsCount);
                        break;
                    }

                default:
                    break;
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ContactUsEntity, Item),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.ContactUs)));
        }
        public OperationOutput CpGetContactUsDetails(Dtos.ContactUs RequestedData)
        {
            Dtos.ContactUs ItemDto = new Dtos.ContactUs();
            User userDbItem = null;
            ActionStatus actionStatus = new ActionStatus();
            if (RequestOwner != null && RequestOwner.Id != null)
                userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);

            var Item = _unitOfWork.ContactUs
                .GetAll()
                .Include(c => c.CreatedByNavigation)
                .Include(c => c.Actions)
                .Include(c => c.LastStatus)
                .ThenInclude(c => c.MajorStatus)
                .Include(x => x.LastUserAction)
                .ThenInclude(x => x.Reference)
                .Where(x => x.Id == RequestedData.Id)
                .AsNoTracking().FirstOrDefault();

            if (Item == null || userDbItem == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Item.Adapt(ItemDto, Dtos.ContactUs.SelectContactUsConfig(GetPath, false));

            #region Set Action Status

            if ((Enums.JobRole)userDbItem.JobRole == Enums.JobRole.ContactUsManagement)
            {
                User user = null;
                if (ItemDto.LastActionUser != null)
                    user = _unitOfWork.User.GetById(ItemDto.LastActionUser.Value);

                if (ItemDto.LastStatusId == (int)Enums.ContactStatus.New)
                    actionStatus = new ActionStatus { TransferToFollowUpOfficer = true, Reject = true, Processed = false, Return = false, TransferTo = false, TransferToDepartment = false, TransferToRegion = false, Closed = false, Replay = true };

                else if (user != null && (ItemDto.LastStatusId == (int)Enums.ContactStatus.Returned || ItemDto.LastStatusId == (int)Enums.ContactStatus.Rejected) && user.JobRole == (int)Enums.JobRole.FollowUpContactUs)
                    actionStatus = new ActionStatus { TransferToFollowUpOfficer = true, Reject = true, Processed = false, Return = false, TransferTo = false, TransferToDepartment = false, TransferToRegion = false, Closed = false, Replay = true };

                else
                    actionStatus = new ActionStatus { TransferToFollowUpOfficer = false, Reject = false, Processed = false, Return = false, TransferTo = false, TransferToDepartment = false, TransferToRegion = false, Closed = false, Replay = true };
            }

            else if ((Enums.JobRole)userDbItem.JobRole == Enums.JobRole.FollowUpContactUs)
            {
                if (ItemDto.LastStatusId == (int)Enums.ContactStatus.Rejected && ItemDto.RejectedByOfficer == true)
                    actionStatus = new ActionStatus { TransferToFollowUpOfficer = false, Reject = false, Processed = false, Return = false, TransferTo = false, TransferToDepartment = false, TransferToRegion = false, Closed = true, Replay = false };


                else if ((ItemDto.LastStatusId == (int)Enums.ContactStatus.Returned && ItemDto.LastUserAction.JobRole == (int)Enums.JobRole.ContactUsManagement)
                && (ItemDto.ParentLastReference != HeadQuartiersRefernceId && ItemDto.ParentLastReference != DepartmentRefernceId))
                    actionStatus = new ActionStatus { TransferToFollowUpOfficer = false, Reject = true, Processed = true, Return = true, TransferTo = false, TransferToDepartment = true, TransferToRegion = true, Closed = true, Replay = false };


                else if ((ItemDto.LastStatusId == (int)Enums.ContactStatus.Done || ItemDto.LastStatusId == (int)Enums.ContactStatus.Returned)
                 && (ItemDto.ParentLastReference != HeadQuartiersRefernceId && ItemDto.ParentLastReference != DepartmentRefernceId))
                    actionStatus = new ActionStatus { TransferToFollowUpOfficer = false, Reject = false, Processed = false, Return = false, TransferTo = false, TransferToDepartment = false, TransferToRegion = false, Closed = false, Replay = false };

                else if ((ItemDto.LastStatusId == (int)Enums.ContactStatus.Done || ItemDto.LastStatusId == (int)Enums.ContactStatus.Returned)
                      && (ItemDto.ParentLastReference == HeadQuartiersRefernceId || ItemDto.ParentLastReference == DepartmentRefernceId))
                    actionStatus = new ActionStatus { TransferToFollowUpOfficer = false, Reject = true, Processed = false, Return = true, TransferTo = false, TransferToDepartment = true, TransferToRegion = true, Closed = true, Replay = true };

                else if (ItemDto.LastStatusId != (int)Enums.ContactStatus.Closed && ItemDto.LastStatusId != (int)Enums.ContactStatus.FinalRejected && ItemDto.LastStatusId != (int)Enums.ContactStatus.TransferTo)
                    actionStatus = new ActionStatus { TransferToFollowUpOfficer = false, Reject = true, Processed = false, Return = true, TransferTo = false, TransferToDepartment = true, TransferToRegion = true, Closed = true, Replay = true };

                else
                    actionStatus = new ActionStatus { TransferToFollowUpOfficer = false, Reject = false, Processed = false, Return = false, TransferTo = false, TransferToDepartment = false, TransferToRegion = false, Closed = false, Replay = false };
            }
            else if ((Enums.JobRole)userDbItem.JobRole == Enums.JobRole.ContactUsProcessingleaders ||
                (Enums.JobRole)userDbItem.JobRole == Enums.JobRole.ContactUsProcessingDepartments)
            {
                var childRefernce = _unitOfWork.References.GetAll().FirstOrDefault(r => r.ParentId == userDbItem.ReferenceId);
                bool isHasChildRefernces = childRefernce != null ? true : false;

                if (Item.LastStatusId == (int)Enums.ContactStatus.TransferTo && Item.LastActionReference != userDbItem.ReferenceId)
                    actionStatus = new ActionStatus { TransferToFollowUpOfficer = false, Reject = false, Processed = false, Return = false, TransferTo = false, TransferToDepartment = false, TransferToRegion = false, Closed = false };
                else if ((Item.LastStatusId == (int)Enums.ContactStatus.Done || Item.LastStatusId == (int)Enums.ContactStatus.Returned) && Item.LastActionReference == userDbItem.ReferenceId)
                    actionStatus = new ActionStatus { TransferToFollowUpOfficer = false, Reject = false, Processed = false, Return = false, TransferTo = false, TransferToDepartment = false, TransferToRegion = false, Closed = false };
                else actionStatus = new ActionStatus { TransferToFollowUpOfficer = false, Reject = false, Processed = true, Return = true, TransferTo = isHasChildRefernces, TransferToDepartment = false, TransferToRegion = false, Closed = false };
            }
            else if ((Enums.JobRole)userDbItem.JobRole == Enums.JobRole.ContactUsQualityAssurance)
                actionStatus = new ActionStatus { Reopen = true };
            else actionStatus = new ActionStatus();

            #endregion

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ContactUsEntity, ItemDto),
                   new OutputDictionary(OperationOutputKeys.EntityID, Cryptography.AES.Encrypt((int)Enums.Entities.ContactUs)),
                   new OutputDictionary(OperationOutputKeys.ActionStatus, actionStatus));
        }

        #region 940Handlers
        public async Task<OperationOutput> GetAmana940Categories(Dtos.Amana940Category RequestedData)
        {
            Integrations.RM940 rM940 = new(Amana940_UserName, Amana940_Password);
            List<Dtos.Amana940Category> Item;

            if (RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (RequestedData.ID != null)
                Item = await rM940.GetCategoriesListAsync(RequestedData.ID.ToString());
            else
                Item = await rM940.GetCategoriesListAsync();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.CategoryEntity, Item));
        }

        #endregion

        public OperationOutput SaveContactUs(Dtos.ContactUs RequestedData)
        {
            ContactU DbItem = new ContactU();
            User userDbItem = null;

            if (RequestOwner != null && RequestOwner.Id != null)
                userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);

            if (UseCapcha)
                if (string.IsNullOrEmpty(RequestedData.Capcha) || !GoogleCapcha.CheckCapchaSession(CapchaSecret, RequestedData.Capcha))
                    return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            if (userDbItem == null && RequestOwner == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            if (!Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestedData.FileUrlBase64))
            {
                if (RequestedData.EntityId == (int)Enums.Entities.ContactUs_Deaf)
                    RequestedData.FileUrl = Files.SaveBase64FileToServer(Strings.GenerateGUID() + ".mp4", RequestedData.FileUrlBase64, VideosSavePath);
                else
                    if (RequestedData.IsImageAttached.HasValue)
                    RequestedData.FileUrl = Images.SaveSingleImageOnServer(RequestedData.FileUrlBase64, null, ImagesSavePath, false);
                else
                    RequestedData.FileUrl = Files.SaveBase64FileToServer(Strings.GenerateGUID() + ".pdf", RequestedData.FileUrlBase64, FilesSavePath);
            }

            if (RequestedData.Id.HasValue)
            {
                DbItem = _unitOfWork.ContactUs.GetById(RequestedData.Id.Value);
                if (DbItem == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                RequestedData.Adapt(DbItem, RequestedData.UpdateConfig(RequestOwner.Id));
                _unitOfWork.ContactUs.Update(DbItem);
            }
            else
            {
                var Code = DateTime.Now.ToString("yyMMddhhmm") + Strings.RandomDigits(4).ToString();
                RequestedData.Adapt(DbItem, RequestedData.AddConfig(RequestOwner.Id, Code));
                Actions action = new Actions();
                action.ContactId = DbItem.Id;
                action.CreatedBy = DbItem.CreatedBy;
                action.CreatedDate = TransactionDate;
                action.StatusId = (int)Enums.ContactStatus.New;
                action.FromUserId = DbItem.CreatedBy;
                DbItem.Actions.Add(action);
                _unitOfWork.ContactUs.Add(DbItem);
                _unitOfWork.Complete();

                UpdateContactAfterAction(RequestedData, ref DbItem, action);
                if (RequestedData.RegionReferenceId != null)
                {
                    SaveActionInCaseRegionReferenceIsExist(RequestedData, ref DbItem, action);
                    UpdateContactUsForRegionReferenceAction(RequestedData, ref DbItem, action);
                }
            }
            _unitOfWork.Complete();

            RequestedData.Id = DbItem.Id;
            RequestedData.Code = DbItem.Code;
            if (userDbItem != null && userDbItem.JobRole != null && (Enums.JobRole)userDbItem.JobRole == Enums.JobRole.FollowUpContactUs)
                return CpGetContactUsDetails(RequestedData);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.ContactUsEntity, RequestedData));
        }


        private void SaveActionInCaseRegionReferenceIsExist(Dtos.ContactUs RequestedData, ref ContactU DbItem, Actions action)
        {
            action = new Actions();
            action.ContactId = DbItem.Id;
            action.CreatedBy = DbItem.CreatedBy;
            action.CreatedDate = TransactionDate;
            action.StatusId = (int)Enums.ContactStatus.TransferTo;
            action.FromUserId = AutomatedUserId;
            action.ToReferenceId = RequestedData.RegionReferenceId;
            DbItem.Actions.Add(action);
        }

        private void UpdateContactUsForRegionReferenceAction(Dtos.ContactUs RequestedData, ref ContactU DbItem, Actions action)
        {
            DbItem.FirstStatusId = (int)Enums.ContactStatus.TransferTo;
            DbItem.LastStatusDate = TransactionDate;
            DbItem.LastStatusId = (int)Enums.ContactStatus.TransferTo;
            DbItem.FirstActionId = action.Id;
            DbItem.LastActionId = action.Id;
            DbItem.LastActionUser = AutomatedUserId;
            DbItem.LastActionReference = RequestedData.RegionReferenceId;
        }

        private void UpdateContactAfterAction(Dtos.ContactUs RequestedData, ref ContactU DbItem, Actions action)
        {
            DbItem.FirstStatusId = (int)Enums.ContactStatus.New;
            DbItem.LastStatusDate = TransactionDate;
            DbItem.LastStatusId = (int)Enums.ContactStatus.New;
            DbItem.FirstActionId = action.Id;
            DbItem.LastActionId = action.Id;
            DbItem.LastActionUser = DbItem.CreatedBy;
        }

        #region inquire about complaint , suggestion and inquiry from portal site
        public OperationOutput GetContactDetails(Dtos.ContactUs RequestedData)
        {
            Dtos.ContactUs ItemDto = new Dtos.ContactUs();
            bool viewEvaluation = false;

            if (string.IsNullOrEmpty(RequestedData.Code) && string.IsNullOrEmpty(RequestedData.UserId))
                return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

            var Item = _unitOfWork.ContactUs
                        .GetAll()
                        .Where(x => x.EntityId == RequestedData.EntityId && x.IsDeleted == false && (x.Code == RequestedData.Code || x.UserId == RequestedData.UserId))
                        .Include(c => c.CreatedByNavigation)
                        .Include(c => c.LastUserAction)
                        .ThenInclude(c => c.Reference)
                        .Include(c => c.Actions)
                        .Include(c => c.LastStatus)
                        .ThenInclude(c => c.MajorStatus)
                        .Include(c => c.Feedbacks)
                        .AsNoTracking().FirstOrDefault();

            if (Item == null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

            Item.Adapt(ItemDto, Dtos.ContactUs.SelectContactUsConfig(VideosGetPath, true));

            if (Item.LastStatusId == (int)Enums.ContactStatus.Closed)
            {
                var lastFeedback = _unitOfWork.Feedback.GetAll()
                .Where(x => x.ContactUsId == Item.Id)
                .OrderByDescending(c => c.Id).FirstOrDefault();
                if (lastFeedback == null)
                    viewEvaluation = true;
                else if (lastFeedback.IsClosed && lastFeedback.IsPositive)
                    viewEvaluation = false;
                else if (lastFeedback.IsClosed && !lastFeedback.IsPositive)
                    viewEvaluation = true;
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                   new OutputDictionary(OperationOutputKeys.ContactUsEntity, ItemDto),
                   new OutputDictionary(OperationOutputKeys.ViewEvaluation, viewEvaluation));
        }
        #endregion

        public OperationOutput GetRegionRefernces()
        {
            var regions = _unitOfWork.References.GetAll().Where(c => c.IsDeleted == false && c.ParentId == HeadQuartiersRefernceId).AsNoTracking().ToList();
            var regionsDto = regions.Adapt<List<LookupItem>>();
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.Regions, regions));
        }

    }
}
