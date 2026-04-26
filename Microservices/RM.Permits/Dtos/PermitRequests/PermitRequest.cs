using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Permits.Interfaces;
using RM.Permits.PermitEnum;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class PermitRequest : BaseDto<PermitRequest, Models.PermitsRequest>, IFilterationPermits<Models.PermitsRequest>
    {
        public PermitRequest()
        {
            PermitActions = new List<PermitAction>();
            PrintRequests = new List<PermitAction>();
        }
        [JsonIgnore]
        public int? Id { get; set; }

        [JsonIgnore]
        public int? _lastPrintRequestId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? LeadersReferenceId { get; set; }

        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? DeliverReferenceId { get; set; }

        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }

        [JsonIgnore]
        public int? CurrentStep { get; set; }
        public string currentStep { set { CurrentStep = Accessor.Set(value); } get { return Accessor.Get(CurrentStep); } }

        [JsonIgnore]
        public int? NextStep { get; set; }
        public string nextStep { set { NextStep = Accessor.Set(value); } get { return Accessor.Get(NextStep); } }

        [JsonIgnore]
        public int? ProjectId { get; set; }
        public string projectId { set { ProjectId = Accessor.Set(value); } get { return Accessor.Get(ProjectId); } }

        [JsonIgnore]
        public int? LastUserActionJobRole { get; set; }
        public string lastUserActionJobRole { set { LastUserActionJobRole = Accessor.Set(value); } get { return Accessor.Get(LastUserActionJobRole); } }



        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string leadersReferenceId { set { LeadersReferenceId = Accessor.Set(value); } get { return Accessor.Get(LeadersReferenceId); } }
        public string LastPrintRequestId { set { _lastPrintRequestId = Accessor.Set(value); } get { return Accessor.Get(_lastPrintRequestId); } }

        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string deliverReferenceId { set { DeliverReferenceId = Accessor.Set(value); } get { return Accessor.Get(DeliverReferenceId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get(DeletedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get(ActivatedBy); } }
        public string Nationality { get; set; }
        public string JobName { get; set; }
        public string DateOfBirth { get; set; }
        public string DateOfBirthH { get; set; }
        public string PassportNumber { get; set; }
        public string passportExpiryDate { get; set; }
        public string IdentityExpiryDate { get; set; }
        public bool? IsForigin { get; set; }
        public bool? IsCommitted { get; set; }
        public string IsCommittedString { get; set; }
        public string IdentityType { get; set; }
        public string CompanyUserName { get; set; }
        public string CarBoard { get; set; }
        public string UserCreated { get; set; }
        public string ItemUrl { get; set; }


        public string IdentityNumber { get; set; }
        public string IdentitySource { get; set; }
        public DateTime? PermitStartDate { get; set; }
        public DateTime? PermitEndDate { get; set; }
        public int? PermitDays { get; set; }
        public string IdentityPhoto { get; set; }
        public string PersonalPhoto { get; set; }
        public string Attachment1 { get; set; }
        public string Attachment2 { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }

        public string FullName { get; set; }
        public string Notes { get; set; }
        public string DeliverReferenceString { get; set; }

        [JsonIgnore]
        public int? _genderId { get; set; }
        public string GenderId { set { _genderId = Accessor.Set(value); } get { return Accessor.Get(_genderId); } }
        public string Gender { get; set; }

        [JsonIgnore]
        public int? NationalityId { get; set; }
        public string nationalityId { set { NationalityId = Accessor.Set(value); } get { return Accessor.Get(NationalityId); } }



        [JsonIgnore]
        public int? _lifeCode { get; set; }
        public string LifeCode { set { _lifeCode = Accessor.Set(value); } get { return Accessor.Get(_lifeCode); } }

        public string LifeStateString { get; set; }
        public int LifeStatusCode { get; set; }

        public string IdentityPhotoBase64 { get; set; }
        public string PersonalPhotoBase64 { get; set; }
        public string Attachment1Base64 { get; set; }
        public string Attachment2Base64 { get; set; }

        public string CarModel { get; set; }
        public string CarLitters { get; set; }
        public string CarNumbers { get; set; }
        public string CarColor { get; set; }
        public int? PermitType { get; set; }
        public string PermitTypeString { get; set; }
        public string Code { get; set; }

        public string StatusString { get; set; }
        public string StatusStringEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public DateTime? ActivatedDate { get; set; }
        public bool? IsRequiredForPrinting { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string CarType { get; set; }

        public string PermitStartDateString { get; set; }
        public string PermitEndDateString { get; set; }
        public int? PermitState { get; set; }
        public string PermitStateString { get; set; }
        public string PermitStateStringEn { get; set; }
        public string DateOfBirthString { get; set; }
        public string ExpiryDateString { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string PersonCreatedBy { get; set; }
        [JsonIgnore]
        public bool? IsPrintQueryList { get; set; }
        public int PrintCount { get; set; }
        public bool? IsExpiredFilter { get; set; }
        public string ExpiredStringAr { get; set; }
        public string ExpiredStringEn { get; set; }

        public List<PermitAction> PermitActions { get; set; }
        public List<PermitAction> PrintRequests { get; set; }

        public List<PermitWorksite> ListOfWorkSites { get; set; } = new List<PermitWorksite>();
        public ApplicationOperation.Pagination Pagination { get; set; }
        public ActionState ActionState { get; set; }
        public string LastUserActionName { get; set; }
        public bool AllowPrint { get; set; }
        public string ProjectNameAr { get; set; }
        public string ProjectNameEn { get; set; }



        public ExpressionStarter<Models.PermitsRequest> Filteration(Models.User user)
        {
            var filter = PredicateBuilder.New<Models.PermitsRequest>(true);
            var _jobRole = (Enums.JobRole)user.JobRole;

            if (_jobRole == Enums.JobRole.PermitProjectManager)
                filter.And(u => u.NextStep == (int)PermitEnums.FlowStepper.ProjectManagerApproval
                || u.PermitActions.Any(c => c.PermitRequestId == u.Id
                && c.CreatedBy.Value == user.Id));

            if (_jobRole == Enums.JobRole.PermitSecurityAuditOfficer)
                filter.And(u => u.NextStep == (int)PermitEnums.FlowStepper.PermitVerification
                || u.PermitActions.Any(c => c.PermitRequestId == u.Id
                && c.CreatedBy.Value == user.Id));

            if (_jobRole == Enums.JobRole.PermitApprovalOfficer)
                filter.And(u => u.NextStep == (int)PermitEnums.FlowStepper.VerificationAndApproval
                || u.PermitActions.Any(c => c.PermitRequestId == u.Id
                && c.CreatedBy.Value == user.Id));

            if (_jobRole == Enums.JobRole.PermitPrintingOfficer)
                filter.And(u => u.NextStep == (int)PermitEnums.FlowStepper.Print &&
               u.PermitState == (int)PermitEnums.PermitRequestStatus.Verified && u.PermitActions.OrderByDescending(x => x.Id).FirstOrDefault().IsPrinted == false);

            if (IsExpiredFilter == false)
                filter.And(u => u.PermitEndDate.Value.Date < DateTime.Now.Date);

            if (IsExpiredFilter == true)
                filter.And(u => u.PermitEndDate.Value.Date >= DateTime.Now.Date);

            if (PermitType != null)
                filter.And(u => u.PermitType == PermitType);


            if (PermitState != null)
                filter.And(u => u.PermitState == PermitState);

            if (PermitStartDate != null)
                filter.And(u => u.PermitStartDate.Value.Date == PermitStartDate.Value.Date);

            if (PermitEndDate != null)
                filter.And(u => u.PermitEndDate.Value.Date == PermitEndDate.Value.Date);

            if (!string.IsNullOrEmpty(Code))
                filter.And(u => u.Code.Contains(Code));

            if (!string.IsNullOrEmpty(IdentityNumber))
                filter.And(u => u.IdentityNumber.Contains(IdentityNumber));

            if (!string.IsNullOrEmpty(JobName))
                filter.And(u => u.JobName.Contains(JobName));

            if (!string.IsNullOrEmpty(Nationality))
                filter.And(u => u.Nationality.Contains(Nationality));

            if (!string.IsNullOrEmpty(CarLitters))
                filter.And(u => u.CarLitters.Contains(CarLitters));


            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);



            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }


        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.PermitsRequest, PermitRequest>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)

                .Map(dest => dest.PermitTypeString, src => src.PermitType == (int)PermitEnums.PermitTypes.Personal ? "تصريح شخصى" : "تصريح سيارة")
                .Map(dest => dest.PermitStartDateString, src => src.PermitStartDate.HasValue ? src.PermitStartDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PermitEndDateString, src => src.PermitEndDate.HasValue ? src.PermitEndDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.Gender, src => src.Gender != null ? src.Gender == (int)Enums.Gender.Male ? "ذكر" : "انثى" : null)
                .Map(dest => dest.DateOfBirthString, src => src.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd"))
                .Map(dest => dest.IdentityType, src => src.IsForigin == true ? "مقيم" : "مواطن")

                .Map(dest => dest.PermitStateString, src => src.PermitState == (int)PermitEnums.PermitRequestStatus.New ? "تصريح جديد"
                     : src.PermitState == (int)PermitEnums.PermitRequestStatus.Verified ? "تصريح مقبول" :
                     src.PermitState == (int)PermitEnums.PermitRequestStatus.Canceld ? " تصريح غير مقبول" :
                     src.PermitState == (int)PermitEnums.PermitRequestStatus.Rejected ? " تصريح مرفوض" : "")

                .Map(dest => dest.PermitStateStringEn, src => src.PermitState == (int)PermitEnums.PermitRequestStatus.New ? "New Permit"
                     : src.PermitState == (int)PermitEnums.PermitRequestStatus.Verified ? "Acceptable " :
                     src.PermitState == (int)PermitEnums.PermitRequestStatus.Canceld ? "Unacceptable" :
                     src.PermitState == (int)PermitEnums.PermitRequestStatus.Rejected ? "Rejected" : "")

                 .Map(dest => dest.StatusString, src => src.IsActive == true ? "مقبول" : "مرفوض")
                 .Map(dest => dest.StatusStringEn, src => src.IsActive == true ? "Acceptable" : "Rejected")
                 .Map(dest => dest.ExpiredStringAr, src => src.PermitEndDate.GetValueOrDefault().Date >= DateTime.Now.Date ? "سارى" : "منتهى")

                 .Map(dest => dest.ExpiredStringEn, src => src.PermitEndDate.GetValueOrDefault().Date >= DateTime.Now.Date ? "Activated" : "Expired")
                 .Map(dest => dest.LastUserActionName, src => src.PermitActions.Any() ? src.PermitActions.Where(c => c.IsPrinted == null).OrderByDescending(c => c.Id).First().CreatedByNavigation.Name : string.Empty)
                 .Map(dest => dest.ProjectNameAr, src => src.Project != null ? src.Project.NameAr : string.Empty)
                 .Map(dest => dest.ProjectNameEn, src => src.Project != null ? src.Project.NameEn : string.Empty)
                 .Map(dest => dest.AllowPrint, src => src.NextStep == (int)PermitEnums.FlowStepper.Print ? true : false)
                 .Map(dest => dest.CarBoard, src => $"{src.CarLitters}  {src.CarNumbers}")

                 .Map(dest => dest.UserCreated, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : null)
                 .Map(dest => dest.CompanyUserName, src => src.CreatedByNavigation != null && src.CreatedByNavigation.Reference != null ? src.CreatedByNavigation.Reference.NameAr : null)



                 .Config;
        }

        public static TypeAdapterConfig SelectConfigForDetails(string IdentityPhotoGetPath, string PersonalPhotoGetPath, string DocumentsGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.PermitsRequest, PermitRequest>()
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                   .Map(dest => dest._genderId, src => src.Gender)
                 .Map(dest => dest._lifeCode, src => src.LifeStatusCode)

                  .Map(dest => dest.PermitStartDateString, src => src.PermitStartDate.HasValue ? src.PermitStartDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                   .Map(dest => dest.PermitEndDateString, src => src.PermitEndDate.HasValue ? src.PermitEndDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                   .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)

                .Map(dest => dest.PermitTypeString, src => src.PermitType == (int)PermitEnums.PermitTypes.Personal ? "تصريح شخصى" : "تصريح سيارة")

                .Map(dest => dest.IdentityType, src => src.IsForigin == true ? "مقيم" : "مواطن")
                 .Map(dest => dest.IdentityExpiryDate, src => src.ExpiryDate)

                .Map(dest => dest.PermitStateString, src => src.PermitState == (int)PermitEnums.PermitRequestStatus.New ? "تصريح جديد"
                     : src.PermitState == (int)PermitEnums.PermitRequestStatus.Verified ? "تصريح مقبول" :
                     src.PermitState == (int)PermitEnums.PermitRequestStatus.Canceld ? " تصريح غير مقبول" :
                     src.PermitState == (int)PermitEnums.PermitRequestStatus.Rejected ? " تصريح مرفوض" : "")

                .Map(dest => dest.PermitStateStringEn, src => src.PermitState == (int)PermitEnums.PermitRequestStatus.New ? "New Permit"
                     : src.PermitState == (int)PermitEnums.PermitRequestStatus.Verified ? "Acceptable " :
                     src.PermitState == (int)PermitEnums.PermitRequestStatus.Canceld ? "Unacceptable" :
                     src.PermitState == (int)PermitEnums.PermitRequestStatus.Rejected ? "Rejected" : "")

                 .Map(dest => dest.DeliverReferenceString, src => src.DeliverReference != null ? src.DeliverReference.NameAr : string.Empty)
                 .Map(dest => dest.DateOfBirth, src => src.DateOfBirth.GetValueOrDefault().ToString("yyyy-MM-dd"))
                 .Map(dest => dest.IsCommittedString, src => src.IsCommitted == true ? "يلتزم" : "لا يلتزم")

                 .Map(dest => dest.IdentityPhoto, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.IdentityPhoto) ? $"{IdentityPhotoGetPath}/{src.IdentityPhoto}" : string.Empty)
                 .Map(dest => dest.PersonalPhoto, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.PersonalPhoto) ? $"{PersonalPhotoGetPath}/{src.PersonalPhoto} " : string.Empty)
                 .Map(dest => dest.Attachment1, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.Attachment1) ? $"{DocumentsGetPath}/ {src.Attachment1}" : string.Empty)
                 .Map(dest => dest.Attachment2, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.Attachment2) ? $"{DocumentsGetPath}/ {src.Attachment2}" : string.Empty)
                 .Map(dest => dest._lastPrintRequestId, src => src.PermitActions.OrderByDescending(x => x.Id).FirstOrDefault(x => x.IsPrinted == false) != null ? src.PermitActions.OrderByDescending(x => x.Id).FirstOrDefault(x => x.IsPrinted == false).Id : 0)

                 .Map(dest => dest.PrintRequests, src => src.PermitActions.Any() ? src.PermitActions.Where(x => x.IsPrinted == true).OrderBy(c => c.Id).Adapt<List<Dtos.PermitAction>>(Dtos.PermitAction.SelectConfig()) : new List<Dtos.PermitAction>())
                 .Map(dest => dest.PermitActions, src => src.PermitActions.Any() ? src.PermitActions
                    .Where(x => x.IsPrinted == null).OrderBy(c => c.Id)
                    .Adapt<List<Dtos.PermitAction>>(Dtos.PermitAction.SelectConfig())
                    : new List<Dtos.PermitAction>())

                 .Map(dest => dest.ListOfWorkSites, src => src.PermitWorkSites.Any() ? src.PermitWorkSites.Adapt<List<Dtos.PermitWorksite>>(Dtos.PermitWorksite.SelectConfig()) : new List<Dtos.PermitWorksite>())

                 .Map(dest => dest.PrintCount, src => src.PermitActions != null ?
                    src.PermitActions.Where(x => x.IsPrinted == true).Count() : 0)

                 .Config;
        }

        public TypeAdapterConfig AddPersonalPermitConfig(int userId, int userJobRole, Models.FlowStepperProjects permitProjectFlowSteps, string IdentityPhotoSavePath, string PersonalPhotoSavePath, string DocumentsSavePath)
        {
            return new TypeAdapterConfig()
                .NewConfig<PermitRequest, Models.PermitsRequest>().IgnoreNullValues(true)
                .Map(dest => dest.Code, src => $"{DateTime.Now.ToString("yyMMddhhmm")}{Strings.RandomDigits(4).ToString()}")
                .Map(dest => dest.Gender, src => src._genderId.HasValue ? src._genderId.Value : 0)
                .Map(dest => dest.LifeStatusCode, src => src._lifeCode.HasValue ? src._lifeCode.Value : 0)
                .Map(dest => dest.DateOfBirth, src => Dates.ChangeDateFormat(src.DateOfBirth))
                .Map(dest => dest.ExpiryDate, src => src.ExpiryDateString)

                .Map(dest => dest.IdentityPhoto, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.IdentityPhotoBase64) ?
                  Images.SaveSingleImageOnServer(src.IdentityPhotoBase64, null, IdentityPhotoSavePath, false, null, null) : null)

               .Map(dest => dest.PersonalPhoto, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.PersonalPhotoBase64) ?
                Images.SaveSingleImageOnServer(src.PersonalPhotoBase64, null, PersonalPhotoSavePath, false, null, null) : null)

                .Map(dest => dest.Attachment1, src => !string.IsNullOrEmpty(src.Attachment1Base64) ?
                 Files.SaveBase64FileToServer(Strings.GenerateGUID() + ".pdf", src.Attachment1Base64, DocumentsSavePath)
                : null)

                .Map(dest => dest.Attachment2, src => !string.IsNullOrEmpty(src.Attachment2Base64) ?
                 Files.SaveBase64FileToServer(Strings.GenerateGUID() + ".pdf", src.Attachment2Base64, DocumentsSavePath)
                : null)
                  .Map(dest => dest.PermitEndDate, src => src.PermitStartDate.Value.AddDays(src.PermitDays.Value))

                .Map(dest => dest.PermitType, src => src.PermitType.Value)
                .Map(dest => dest.PermitState, src => (int)PermitEnums.PermitRequestStatus.New)

                .Map(dest => dest.CurrentStep, src => permitProjectFlowSteps.StepId)
                .Map(dest => dest.NextStep, src => permitProjectFlowSteps.StepId)
                .Map(dest => dest.LastUserActionJobRole, src => userJobRole)

                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.IsDeleted, src => false)

                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddCarPermitConfig(int userId, int userJobRole, Models.FlowStepperProjects permitProjectFlowSteps)
        {
            return new TypeAdapterConfig()
                .NewConfig<PermitRequest, Models.PermitsRequest>().IgnoreNullValues(true)
                .Map(dest => dest.Code, src => $"{DateTime.Now.ToString("yyMMddhhmm")}{Strings.RandomDigits(4).ToString()}")

                .Map(dest => dest.PermitEndDate, src => src.PermitStartDate.Value.AddDays(src.PermitDays.Value))

                .Map(dest => dest.PermitType, src => src.PermitType.Value)
                .Map(dest => dest.PermitState, src => (int)PermitEnums.PermitRequestStatus.New)

                .Map(dest => dest.CurrentStep, src => permitProjectFlowSteps.StepId)
                .Map(dest => dest.NextStep, src => permitProjectFlowSteps.StepId)
                .Map(dest => dest.LastUserActionJobRole, src => userJobRole)

                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.IsDeleted, src => false)

                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }


    }

    public class PermitRequestStatuses
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
    public class ActionState
    {
        public bool Verified { get; set; } = false;
        public bool Canceld { get; set; } = false;
        public bool Rejected { get; set; } = false;
        public bool CanPrinted { get; set; } = false;
    }



}
