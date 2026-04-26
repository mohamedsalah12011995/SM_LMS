using Mapster;
using Microsoft.EntityFrameworkCore;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Integrations;
using RM.Core.Services;
using RM.Models;
using RM.Permits.Dtos;
using RM.Permits.PermitEnum;
using RM.Permits.UnitOfWorks;
using SixLabors.ImageSharp;
using static RM.Permits.Dtos.OperationOutput;


namespace RM.Permits.Services
{
    public class PermitService : BaseService, IPermitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private int genderLookupId;
        private int maritalStatusLookupId;
        private string yaqeenPersonInfoUrl;
        private string yaqeenIdTypeLookupsUrl;


        public PermitService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor, unitOfWork.Configuration)
        {

            _unitOfWork = unitOfWork;
            setPropertiesFromConfiguration();
        }

        #region HELPER METHOD >> CONSTRACTOR 

        void setPropertiesFromConfiguration()
        {
            genderLookupId = Convert.ToInt32(_unitOfWork.Configuration.GetSection("AppSettings").GetSection("GenderLookupId").Value);
            maritalStatusLookupId = Convert.ToInt32(_unitOfWork.Configuration.GetSection("AppSettings").GetSection("MaritalStatusLookupId").Value);
            yaqeenPersonInfoUrl = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("YaqeenPersonInfoUrl").Value;
            yaqeenIdTypeLookupsUrl = _unitOfWork.Configuration.GetSection("AppSettings").GetSection("YaqeenIdTypeLookupsUrl").Value;
        }

        #endregion


        #region CPANEL PERMIT REQUESTS

        public OperationOutput GetPermitRequestCPLookups()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var permitRequestStatuses = new List<PermitRequestStatuses>
            {
                new PermitRequestStatuses {Id=(int)PermitEnums.PermitRequestStatus.New,NameAr=" التصاريح الجديدة",NameEn="New Permit Requests"},
                new PermitRequestStatuses {Id=(int)PermitEnums.PermitRequestStatus.Verified,NameAr="التصاريح المقبولة",NameEn="Acceptance Permit Requests"},
                new PermitRequestStatuses {Id=(int)PermitEnums.PermitRequestStatus.Canceld,NameAr=" التصاريح الغير مقبولة",NameEn="Canceled Permit Requests"},
                new PermitRequestStatuses {Id=(int)PermitEnums.PermitRequestStatus.Rejected,NameAr=" التصاريح المرفوضة",NameEn="Rejected Permit Requests"},
            };

            var genderMajorLookup = _unitOfWork.MajorLookupsType.FindAll(c => c.NameEn.Trim() == LookupsType.Gender
                                     , m => m.MajorLookups).Select(x => x.MajorLookups).ToList();

            var genderMajorLookupDto = genderMajorLookup.Adapt<List<MajorLookupsDto>>();


            var maritalStatus = _unitOfWork.MajorLookupsType.FindAll(c => c.NameEn.Trim() == LookupsType.MaritalStatus

             , m => m.MajorLookups).Select(x => x.MajorLookups).ToList();

            var maritalStatusDto = maritalStatus.Adapt<List<MajorLookupsDto>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
             new OutputDictionary(OperationOutputKeys.PermitRequestStatuses, permitRequestStatuses),
             new OutputDictionary(OperationOutputKeys.GenderList, genderMajorLookupDto),
             new OutputDictionary(OperationOutputKeys.MaritalStatus, maritalStatusDto));
        }


        public async Task<OperationOutput> GetPermitRequestsList(PermitRequest RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var userDbItem = await _unitOfWork.User.GetByIdAsync(RequestOwner.Id.Value);
            var userProjects = _unitOfWork.ProjectsUsers.FindAll(c => c.UserId == RequestOwner.Id.Value).ToList();

            if (!userProjects.Any())
                return GetOperationOutput(header: Enums.ServiceMessages.MustConnectUserToProject);

            if (CheckUserPermission(userDbItem))
            {

                var filteration = RequestedData.Filteration(userDbItem);

                var permitQuery = _unitOfWork.PermitsRequest.GetAll()
                 .Include(c => c.CreatedByNavigation)
                 .Include(c => c.UpdatedByNavigation)
                 .Include(c => c.Project)
                 .Include(c => c.PermitActions)
                 .ThenInclude(c => c.CreatedByNavigation)
                 .Where(filteration)
                 .Where(p => userProjects.Select(c => c.ProjectId).Contains(p.ProjectId.Value))
                 .Where(p => p.LastUserActionJobRole.Value != (int)userDbItem.JobRole ?
                             p.PermitState == (int)PermitEnums.PermitRequestStatus.New
                          || p.PermitState == (int)PermitEnums.PermitRequestStatus.Verified : true)
                 .OrderByDescending(x => x.CreatedDate);

                var permits = _unitOfWork.PermitsRequest.GetAllWithPaggination(permitQuery, RequestedData.Pagination, DefaultPaginationCount);
                var permitsDto = permits.Data.Adapt<List<PermitRequest>>(PermitRequest.SelectConfig());

                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                      new OutputDictionary(OperationOutputKeys.PermitsEntity, permitsDto),
                      new OutputDictionary(OperationOutputKeys.Pagination, permits.Pagination));
            }
            else return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


        }

        public async Task<OperationOutput> GetPermitRequestDetails(PermitRequestGetByID RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var userDbItem = await _unitOfWork.User.GetByIdAsync(RequestOwner.Id.Value);

            if (CheckUserPermission(userDbItem))
            {
                var permitRequest = await _unitOfWork.PermitsRequest.GetAll()
                    .Include(q => q.DeliverReference)
                    .Include(u => u.CreatedByNavigation)
                    .Include(c => c.PermitWorkSites)
                    .ThenInclude(x => x.WorksiteNavigation)
                    .Include(c => c.PermitActions)
                    .ThenInclude(p => p.UpdatedByNavigation).FirstOrDefaultAsync(x => x.Id == RequestedData._id);


                if (permitRequest == null)
                    return GetOperationOutput(header: Enums.ServiceMessages.NoDataReturned);

                var permitDto = permitRequest.Adapt<PermitRequest>(PermitRequest.SelectConfigForDetails(IdentityPhotoGetPath, PersonalPhotoGetPath, DocumentsGetPath));
                ActionState actionState = new();


                actionState.Verified = (permitRequest.PermitState == (int)PermitEnums.PermitRequestStatus.New || permitRequest.PermitState == (int)PermitEnums.PermitRequestStatus.Verified) && !permitRequest.PermitActions.Any(c => c.PermitRequestId == permitDto.Id && c.CreatedBy.Value == userDbItem.Id) && (userDbItem.JobRole == (int)Enums.JobRole.PermitSecurityAuditOfficer || userDbItem.JobRole == (int)Enums.JobRole.PermitProjectManager || userDbItem.JobRole == (int)Enums.JobRole.PermitApprovalOfficer) ? true : false;
                actionState.Rejected = (permitRequest.PermitState == (int)PermitEnums.PermitRequestStatus.New || permitRequest.PermitState == (int)PermitEnums.PermitRequestStatus.Verified) && !permitRequest.PermitActions.Any(c => c.PermitRequestId == permitDto.Id && c.CreatedBy.Value == userDbItem.Id) && (userDbItem.JobRole == (int)Enums.JobRole.PermitSecurityAuditOfficer || userDbItem.JobRole == (int)Enums.JobRole.PermitProjectManager || userDbItem.JobRole == (int)Enums.JobRole.PermitApprovalOfficer) ? true : false;

                actionState.Canceld = permitRequest.PermitState == (int)PermitEnums.PermitRequestStatus.Verified && (userDbItem.JobRole == (int)Enums.JobRole.PermitProjectManager || userDbItem.JobRole == (int)Enums.JobRole.PermitApprovalOfficer) ? true : false;
                actionState.CanPrinted = permitRequest.PermitState == (int)PermitEnums.PermitRequestStatus.Verified && userDbItem.JobRole == (int)Enums.JobRole.PermitPrintingOfficer ? true : false;

                return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.PermitsEntity, permitDto),
                    new OutputDictionary(OperationOutputKeys.ActionState, actionState));

            }

            else return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

        }


        #region HELPER METHOD >> GetPermitRequestDetails
        bool CheckUserPermission(Models.User userDbItem)
        {
            return (Enums.JobRole)userDbItem.JobRole == Enums.JobRole.PermitSecurityAuditOfficer
                 || (Enums.JobRole)userDbItem.JobRole == Enums.JobRole.PermitPrintingOfficer
                 || (Enums.JobRole)userDbItem.JobRole == Enums.JobRole.OrganizationEmployee
                 || (Enums.JobRole)userDbItem.JobRole == Enums.JobRole.PermitProjectManager
                 || (Enums.JobRole)userDbItem.JobRole == Enums.JobRole.PermitApprovalOfficer;
        }

        #endregion


        public async Task<OperationOutput> ChangeActionPermitRequest(PermitAddAction RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);

            if (userDbItem.ReferenceId == RequestedData.DeliverReferenceId)
            {

                if (RequestedData.Id.HasValue)
                {
                    var permitRequest = _unitOfWork.PermitsRequest.Find(x => x.Id == RequestedData.Id);

                    var projectSteps = _unitOfWork.FlowStepperProjects.FindAll(c => c.ProjectId == permitRequest.ProjectId).OrderBy(c => c.OrderStep).ToList();

                    if (permitRequest is not null && RequestedData.PermitState != null
                        && Enum.IsDefined(typeof(PermitEnums.PermitRequestStatus), RequestedData.PermitState))
                    {
                        AddPermitAction(RequestedData, userDbItem);

                        if (permitRequest.PermitState != (int)PermitEnums.PermitRequestStatus.New)
                            permitRequest.CurrentStep = permitRequest.NextStep;

                        permitRequest.PermitState = RequestedData.PermitState;
                        permitRequest.Notes = RequestedData.Notes;
                        permitRequest.LastUserActionJobRole = userDbItem.JobRole;

                        var nextStep = projectSteps.SkipWhile(p => p.StepId != permitRequest.NextStep).Skip(1).FirstOrDefault();
                        if (nextStep != null)
                            permitRequest.NextStep = nextStep.StepId;

                        if (permitRequest.PermitState == (int)PermitEnums.PermitRequestStatus.Rejected)
                            permitRequest.IsActive = false;

                        _unitOfWork.PermitsRequest.Update(permitRequest);

                        var result = _unitOfWork.Complete();

                        if (result > 0)
                            return await GetPermitRequestDetails(new PermitRequestGetByID { _id = RequestedData.Id });
                        else return GetOperationOutput(header: Enums.ServiceMessages.TransactionErorr);

                    }
                }
            }

            return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

        }

        #region HELPER METHOD ChangeActionPermitRequest
        private void AddPermitAction(PermitAddAction permit, Models.User user)
        {
            var permitAction = new Models.PermitAction();
            permitAction.PermitRequestId = permit.Id;
            permitAction.CreatedBy = user.Id;
            permitAction.CreatedDate = TransactionDate;
            permitAction.UpdatedBy = user.Id;
            permitAction.UpdatedDate = TransactionDate;
            permitAction.Notes = permit.Notes;
            permitAction.Status = permit.PermitState;
            permitAction.StepId = permit.CurrentStep;

            _unitOfWork.PermitActions.Add(permitAction);

        }

        #endregion


        #endregion

        #region   SITE PERMIT REQUESTS

        public async Task<OperationOutput> GetIntegrationData(UserInformation userInfo)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);

            if ((Enums.JobRole)userDbItem.JobRole != Enums.JobRole.OrganizationEmployee)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


            var genderData = _unitOfWork.MajorLookup.FindAll(c => c.TypeId == genderLookupId).ToList();
            var genderDataDto = genderData.Adapt<List<Dtos.MajorLookupsDto>>();


            var maritalStatusData = _unitOfWork.MajorLookup.FindAll(c => c.TypeId == maritalStatusLookupId).ToList();
            var maritalStatusDataDto = maritalStatusData.Adapt<List<Dtos.MajorLookupsDto>>();


            var personData = await InvokeService<IntegrationData>.Invoke(yaqeenPersonInfoUrl, userInfo);

            if (personData != null && personData.Body != null && personData.Body.UserInformation != null)
            {
                personData.Body.UserInformation._genderId = personData.Body.UserInformation.gender == "M" ? genderDataDto.First(x => x.NameEn == "Male").Id :
                 genderDataDto.First(x => x.NameEn == "Female").Id;

                SetPersonLifeStatus(maritalStatusDataDto, personData.Body.UserInformation);

                personData.Body.UserInformation.isForigin = userInfo.idTypeCode == "R" ? true : false;
            }
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                      new OutputDictionary(OperationOutputKeys.PermitsEntity, personData));

        }

        #region   HELPER METHOD >>  GetIntegrationData
        private static void SetPersonLifeStatus(List<MajorLookupsDto> maritalStatusData, UserInformation personData)
        {
            switch (personData.lifeStatus)
            {
                case "S":
                    personData._lifeCode = maritalStatusData.First(x => x.NameEn == "Single").Id;
                    break;
                case "M":
                    personData._lifeCode = maritalStatusData.First(x => x.NameEn == "Married").Id;
                    break;
                case "L":
                    personData._lifeCode = maritalStatusData.First(x => x.NameEn == "Married and dependent").Id;
                    break;
                case "D":
                    personData._lifeCode = maritalStatusData.First(x => x.NameEn == "Divorced").Id;

                    break;
                case "W":
                    personData._lifeCode = maritalStatusData.First(x => x.NameEn == "Widower").Id;

                    break;

                default:
                    break;
            }
        }

        #endregion

        public async Task<OperationOutput> GetPermitRequestLookups(PermitRequestLookup RequestdData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var idTypeLookups = await InvokeService<IdTypeLookups>.Invoke(yaqeenIdTypeLookupsUrl);

            var workLocations = _unitOfWork.MajorLookupsType.FindAll(c => c.NameEn.Trim() == LookupsType.WorkSites

                , m => m.MajorLookups).Select(x => x.MajorLookups.Where(x => x.ReferenceId == RequestdData.ReferenceId).ToList()).FirstOrDefault();

            var workLocationsDto = workLocations.Adapt<List<Dtos.MajorLookupsDto>>();

            var referncesleaderships = _unitOfWork.References.GetAll()
                .Where(c => c.ParentId == RequestdData.LeadersReferenceId).ToList();

            var referncesleadershipsDto = referncesleaderships.Adapt<List<Dtos.MajorLookupsDto>>();

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                    new OutputDictionary(OperationOutputKeys.WorkSites, workLocationsDto),
                    new OutputDictionary(OperationOutputKeys.Refernces, referncesleadershipsDto),
                    new OutputDictionary(OperationOutputKeys.IdTypeLookups, idTypeLookups));

        }


        public async Task<OperationOutput> SavePermitRequest(PermitRequest RequestdData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Models.PermitsRequest permitRequest = new();

            var permitProjectFlowSteps = _unitOfWork.FlowStepperProjects.GetAll()
                .Where(f => f.ProjectId == RequestdData.ProjectId).OrderBy(c => c.OrderStep).FirstOrDefault();


            var userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);

            if ((Enums.JobRole)userDbItem.JobRole != Enums.JobRole.OrganizationEmployee)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            if (RequestdData.PermitType.Value == (int)PermitEnums.PermitTypes.Personal)
            {
                if (Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestdData.IdentityPhotoBase64)
                || Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(RequestdData.PersonalPhotoBase64))

                    return GetOperationOutput(header: Enums.ServiceMessages.RequiredFiled);
            }

            if (!string.IsNullOrEmpty(RequestdData.IdentityPhotoBase64) || !string.IsNullOrEmpty(RequestdData.PersonalPhotoBase64))
            {
                if (Files.GetBase64FileSizeMb(RequestdData.IdentityPhotoBase64) > FileSizeMb ||
                Files.GetBase64FileSizeMb(RequestdData.PersonalPhotoBase64) > FileSizeMb)

                    return GetOperationOutput(header: Enums.ServiceMessages.FileSizeError);
            }

            if (!string.IsNullOrEmpty(RequestdData.Attachment1Base64) || !string.IsNullOrEmpty(RequestdData.Attachment2Base64))
            {
                if (Files.GetBase64FileSizeMb(RequestdData.Attachment1Base64) > FileSizeMb ||
                Files.GetBase64FileSizeMb(RequestdData.Attachment2Base64) > FileSizeMb)

                    return GetOperationOutput(header: Enums.ServiceMessages.FileSizeError);

            }
            if (RequestdData.PermitType.Value == (int)PermitEnums.PermitTypes.Personal)
            {
                RequestdData.Adapt(permitRequest, RequestdData.AddPersonalPermitConfig(RequestOwner.Id.Value,
                    userDbItem.JobRole.Value, permitProjectFlowSteps, IdentityPhotoSavePath, PersonalPhotoSavePath,
                    DocumentsSavePath));

                AddPermitWorkSites(RequestdData, ref permitRequest);

                _unitOfWork.PermitsRequest.Add(permitRequest);
            }

            if (RequestdData.PermitType.Value == (int)PermitEnums.PermitTypes.Car)
            {
                RequestdData.Adapt(permitRequest, RequestdData.AddCarPermitConfig(RequestOwner.Id.Value,
                    userDbItem.JobRole.Value, permitProjectFlowSteps));

                AddPermitWorkSites(RequestdData, ref permitRequest);

                _unitOfWork.PermitsRequest.Add(permitRequest);
            }


            #region Save Permit Action

            SavePermitAction(RequestdData, permitRequest);

            #endregion


            _unitOfWork.Complete();

            RequestdData.Id = permitRequest.Id;

            await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, RequestdData.ReferenceId, RequestdData.EntityId, null, Enums.InteractionStatisticsType.ViewsCount, RequestdData.ItemUrl);

            return await GetPermitRequestDetails(new PermitRequestGetByID { _id = RequestdData.Id });
        }

        #region HELPER METHODS >> SavePermitRequest

        private static void SavePermitAction(PermitRequest RequestdData, PermitsRequest permitRequest)
        {
            if (!RequestdData.Id.HasValue)
            {
                var action = new Models.PermitAction();
                action.PermitRequestId = permitRequest.Id;
                action.CreatedBy = permitRequest.CreatedBy;
                action.CreatedDate = permitRequest.CreatedDate;
                action.UpdatedBy = permitRequest.CreatedBy;
                action.UpdatedDate = permitRequest.CreatedDate;
                action.StepId = permitRequest.CurrentStep;
                action.Status = permitRequest.PermitState;
                action.IsPrinted = null;
                permitRequest.PermitActions.Add(action);
            }
        }

        private static void AddPermitWorkSites(PermitRequest RequestdData, ref PermitsRequest permitRequest)
        {
            foreach (var worksite in RequestdData.ListOfWorkSites)
            {
                permitRequest.PermitWorkSites.Add(new Models.PermitWorkSite()
                {
                    PermitId = permitRequest.Id,
                    WorksiteId = worksite.Id,
                });
            }
        }


        #endregion


        public async Task<OperationOutput> QueryPersonalPermitRequests(QueryPersonPermitRequests RequestdData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);

            if ((Enums.JobRole)userDbItem.JobRole != Enums.JobRole.OrganizationEmployee)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


            var permits = _unitOfWork.PermitsRequest.GetAll()
         .Include(c => c.Project)
         .Where(r => !string.IsNullOrEmpty(RequestdData.IdentityNumber) ?
              r.IdentityNumber == RequestdData.IdentityNumber
              : r.CreatedBy == RequestOwner.Id)
          .Where(r => r.IsDeleted == false && r.PermitType == (int)PermitEnums.PermitTypes.Personal)

       .Where(r => !string.IsNullOrEmpty(RequestdData.Code) ? r.Code == RequestdData.Code : true).ToList();
            var permitsDto = permits.Adapt<List<PermitRequest>>(PermitRequest.SelectConfig());

            await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, RequestdData.ReferenceId, RequestdData.EntityId, null, Enums.InteractionStatisticsType.ViewsCount, RequestdData.ItemUrl);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                  new OutputDictionary(OperationOutputKeys.PermitsEntity, permitsDto));

        }

        public async Task<OperationOutput> QueryCarPermitRequests(QueryCarPermitRequests RequestdData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);


            var userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);
            if ((Enums.JobRole)userDbItem.JobRole != Enums.JobRole.OrganizationEmployee)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


            var permits = _unitOfWork.PermitsRequest.GetAll()
               .Include(c => c.Project)
               .Include(u => u.CreatedByNavigation)
               .ThenInclude(p => p.Reference)
               .Where(c => string.IsNullOrEmpty(RequestdData.CarLitters) && string.IsNullOrEmpty(RequestdData.CarNumbers) ?
                      c.CreatedBy == RequestOwner.Id && c.PermitType == (int)PermitEnums.PermitTypes.Car :
                c.CarLitters.ToLower().Trim() == RequestdData.CarLitters.ToLower().Trim() && c.CarNumbers.Trim() == RequestdData.CarNumbers.Trim())
               .ToList().Adapt<List<PermitRequest>>(PermitRequest.SelectConfig());

            await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, RequestdData.ReferenceId, RequestdData.EntityId, null, Enums.InteractionStatisticsType.ViewsCount, RequestdData.ItemUrl);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
                new OutputDictionary(OperationOutputKeys.PermitsEntity, permits));

        }

        public OperationOutput RequestToPrintPermit(PermitRequestGetByID RequestdData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);
            if ((Enums.JobRole)userDbItem.JobRole != Enums.JobRole.OrganizationEmployee)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

            var permit = _unitOfWork.PermitsRequest.Find
                   (x => x.Id == RequestdData._id, c => c.PermitActions);

            if (permit != null && permit.PermitState == (int)PermitEnums.PermitRequestStatus.Verified)
            {
                var count = permit.PermitActions != null ? permit.PermitActions.Where(x => x.IsPrinted == true).Count() : 0;
                var limitPrint = Convert.ToInt32(_unitOfWork.Configuration.GetSection("AppSettings").GetSection("LimitPrintPermits").Value);

                if (count == limitPrint)
                    return GetOperationOutput(header: Enums.ServiceMessages.NotAllowedPrintMessage);

                var permitNotPrintedCount = permit.PermitActions.Count(x => x.IsPrinted == false);

                if (permitNotPrintedCount == 0 && permit.PermitEndDate.Value.Date >= TransactionDate.Date)

                    SavePermitAction(permit);

                else return GetOperationOutput(header: Enums.ServiceMessages.NotAllowedPrintMessage);
            }

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);
        }

        #region HELPER METHOD >> RequestToPrintPermit
        private void SavePermitAction(PermitsRequest item)
        {
            var printRequest = new Models.PermitAction();
            printRequest.IsPrinted = false;
            printRequest.CreatedBy = item.CreatedBy;
            printRequest.UpdatedBy = RequestOwner.Id;
            printRequest.CreatedDate = item.CreatedDate;
            printRequest.UpdatedDate = TransactionDate;
            printRequest.PermitRequestId = item.Id;
            item.CurrentStep = item.NextStep;
            _unitOfWork.PermitActions.Add(printRequest);
            _unitOfWork.Complete();
        }

        #endregion

        public async Task<OperationOutput> GetCompanyInfo()
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            Dtos.CompanyRefernce companyInfoDto = null;

            var userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);

            if ((Enums.JobRole)userDbItem.JobRole != Enums.JobRole.OrganizationEmployee)
                return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);


            var companyInfo = await _unitOfWork.User.GetAll()
                  .Include(c => c.Reference)
                  .ThenInclude(c => c.ReferenceContents).AsNoTracking()
                  .FirstOrDefaultAsync(x => x.Id == RequestOwner.Id.Value);

            if (companyInfo is not null)
            {
                companyInfoDto = new Dtos.CompanyRefernce
                {
                    Id = companyInfo.Id,
                    Address = companyInfo.Reference.ReferenceContents.Any() ? companyInfo.Reference.ReferenceContents.First().Address : null,
                    AddressEn = companyInfo.Reference.ReferenceContents.Any() ? companyInfo.Reference.ReferenceContents.First().AddressEn : null,
                    ReferenceId = companyInfo.ReferenceId,
                    Email = companyInfo.Reference.ReferenceContents.Any() ? companyInfo.Reference.ReferenceContents.First().Email : null,
                    Phone = companyInfo.Reference.ReferenceContents.Any() ? companyInfo.Reference.ReferenceContents.First().Phone : null,
                    EntityId = companyInfo.Reference.ReferenceContents.Any() ? companyInfo.Reference.ReferenceContents.First().EntityId : null,
                    Region = companyInfo.Reference.ReferenceContents.Any() ? companyInfo.Reference.ReferenceContents.First().Region : null,
                    Mobile = companyInfo.Reference.ReferenceContents.Any() ? companyInfo.Reference.ReferenceContents.First().Mobile : null,
                    Fax = companyInfo.Reference.ReferenceContents.Any() ? companyInfo.Reference.ReferenceContents.First().Fax : null,
                    Mailbox = companyInfo.Reference.ReferenceContents.Any() ? companyInfo.Reference.ReferenceContents.First().Mailbox : null,
                    RegistrationNo = companyInfo.Reference.ReferenceContents.Any() ? companyInfo.Reference.ReferenceContents.First().RegistrationNo : null,
                    ManagerName = companyInfo.Reference.ReferenceContents.Any() ? companyInfo.Reference.ReferenceContents.First().ManagerName : null,
                    EndDateRegistrationNo = companyInfo.Reference.ReferenceContents.Any() ? companyInfo.Reference.ReferenceContents.First().EndDateRegistrationNo : null,
                    CompanyUserName = companyInfo.Name,
                    UserName = companyInfo.UserName,
                    Password = companyInfo.Password,
                    CompanyName = companyInfo.Reference != null ? companyInfo.Reference.NameAr : null
                };
            }

            await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, companyInfoDto.ReferenceId, (int)Enums.Entities.CompanyInfo, null, Enums.InteractionStatisticsType.ViewsCount, null);

            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess,
               new OutputDictionary(OperationOutputKeys.CompanyInfoEntity, companyInfoDto));

        }


        #endregion

        #region CPANEL PRINT PERMIT REQUESTS 


        public async Task<OperationOutput> PrintPermit(PrintPermitDto RequestedData)
        {
            if (RequestOwner is null)
                return GetOperationOutput(header: Enums.ServiceMessages.NoTokenRequested);

            var userDbItem = _unitOfWork.User.GetById(RequestOwner.Id.Value);

            if ((Enums.JobRole)userDbItem.JobRole == Enums.JobRole.PermitPrintingOfficer
                  && userDbItem.ReferenceId == RequestedData.DeliverReferenceId
                  && RequestedData.LastPrintRequestId != null)
            {

                await _unitOfWork.PermitActions.ExecuteUpdateAsync(x => x.Id == RequestedData.LastPrintRequestId,
                    sett => sett.SetProperty(x => x.UpdatedBy, RequestOwner.Id)
                    .SetProperty(d => d.IsPrinted, true)
                    .SetProperty(y => y.UpdatedDate, TransactionDate));

                await _unitOfWork.PermitsRequest.ExecuteUpdateAsync(x => x.Id == RequestedData.Id.Value,
                  sett => sett.SetProperty(x => x.LastUserActionJobRole, userDbItem.JobRole));

                return await GetPermitRequestDetails(new PermitRequestGetByID { _id = RequestedData.Id });

            }
            else return GetOperationOutput(header: Enums.ServiceMessages.NoPermission);

        }


        public async Task<OperationOutput> SaveInteractionStatistics(InteractionStatisticsDto requestData)
        {
            await InteractionStatistics.SaveInteractionStatistics(StatisticsServiceUrl, Token, requestData.ReferenceId, (int)Enums.Entities.EntrancePermits, null, Enums.InteractionStatisticsType.ViewsCount, requestData.ItemUrl);
            return GetOperationOutput(header: Enums.ServiceMessages.TransactionSuccess);

        }



        #endregion




    }
}
