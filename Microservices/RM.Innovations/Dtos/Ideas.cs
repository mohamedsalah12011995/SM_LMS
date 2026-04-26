
using DocumentFormat.OpenXml.Wordprocessing;
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.Innovations.Dtos
{
    public class Ideas:BaseDto<Ideas,Models.Idea>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        [JsonIgnore]
        public int? Type { get; set; }
        [JsonIgnore]
        public int? Category { get; set; }

        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]

        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? ToReference { get; set; }

        [JsonIgnore]
        public int? Priority { get; set; }
        [JsonIgnore]
        public int? Status { get; set; }
        [JsonIgnore]
        public int? ActionId { get; set; }

        [JsonIgnore]
        public int? DeclineActionId { get; set; }

        //[JsonIgnore]
        //public int? _transToJobRoleActionId { get; set; }
        
        public int? NeedsPeriod { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string type { set { Type = Accessor.Set(value); } get { return Accessor.Get<int?>(Type); } }
        public string category { set { Category = Accessor.Set(value); } get { return Accessor.Get<int?>(Category); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }

        public string toReference { set { ToReference = Accessor.Set(value); } get { return Accessor.Get<int?>(ToReference); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }

        public string priority { set { Priority = Accessor.Set(value); } get { return Accessor.Get<int?>(Priority); } }
        public string status { set { Status = Accessor.Set(value); } get { return Accessor.Get<int?>(Status); } }
        
        public string actionId { set { ActionId = Accessor.Set(value); } get { return Accessor.Get<int?>(ActionId); } }
        
        public string declineActionId { set { DeclineActionId = Accessor.Set(value); } get { return Accessor.Get<int?>(DeclineActionId); } }
        //public string TransToJobRoleActionId { set { _transToJobRoleActionId = Accessor.Set(value); } get { return Accessor.Get<int?>(_transToJobRoleActionId); } }
        public string FullName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string IdeaAddress { get; set; }
        public string IdeaDescription { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Attachment { get; set; }
        public string AttachmentBase64 { get; set; }
        public string CreatedByText { get; set; }
        public string CreatedByPhone { get; set; }
        public string PriorityText { get; set; }
        public string StatusText { get; set; }
        public string TypeText { get; set; }
        public string CategoryText { get; set; }
        public string ActionNote { get; set; }
        public string TransfereToText { get; set; }
        public string NeedsBudgetText { get; set; }
        public string CapabilityText { get; set; }
        public string FeasibilityText { get; set; }
        public string CurrentActionText { get; set; }
      
        public long? Code { get; set; }
        

        public DateTime? DeletedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }

        public string DeletedDateString { get; set; }
        public string CreatedDateString { get; set; }
        public string ActivatedDateString { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IdeaExist { get; set; }
        public bool? IsShow { get; set; }

        public bool? Capability { get; set; }
        public bool? NeedsBudget { get; set; }
        public bool? Feasibility { get; set; }

        public bool? IsAgree { get; set; }
        public int? AgreeCount { get; set; }
        public int? CommentsCount { get; set; }
        public int? DisAgreeCount { get; set; }
        public string Capcha { get; set; }  
        public ActionPermission ActionPermission { get; set; }
        public Users User { get; set; }
        public List<IdeasActions> Actions { get; set; }
        public List<IdeaComments>  IdeaComments { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.Idea> Filteration(bool? IsEntityRepresentativeJobRole = null)
        {
            var filter = PredicateBuilder.New<Models.Idea>(true);

            if(IsEntityRepresentativeJobRole != true)
            filter.And(u => u.ReferenceId == ReferenceId);

            if (ToReference.HasValue)
                filter.And(u => u.ToReference == ToReference);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            if (IsShow.HasValue)
                filter.And(u => u.IsShow == IsShow);

            if (Priority.HasValue)
                filter.And(u => u.Priority == Priority);

            if (Status.HasValue)
                filter.And(u => u.Status == Status);

            if (Type.HasValue)
                filter.And(u => u.Type == Type);

            if (Category.HasValue)
                filter.And(u => u.Category == Category);

            if (NeedsBudget.HasValue)
                filter.And(u => u.NeedsBudget == NeedsBudget);

            if (Feasibility.HasValue)
                filter.And(u => u.Feasibility == Feasibility);

            if (Capability.HasValue)
                filter.And(u => u.Capability == Capability);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig(string FilesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Idea, Ideas>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.FullName, src => src.FullName != null ? src.FullName : src.CreatedByNavigation != null ? src.CreatedByNavigation.Name:string.Empty)
                .Map(dest => dest.CreatedByText, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name:null)
                .Map(dest => dest.PriorityText, src => src.PriorityNavigation != null ? src.PriorityNavigation.NameAr:null)
                .Map(dest => dest.TypeText, src => src.TypeNavigation != null ? src.TypeNavigation.NameAr:null)
                .Map(dest => dest.CategoryText, src => src.CategoryNavigation != null ? src.CategoryNavigation.NameAr:null)
                .Map(dest => dest.TransfereToText, src => src.ToReferenceNavigation != null ? src.ToReferenceNavigation.NameAr:null)
                .Map(dest => dest.CreatedByText, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name:null)
                .Map(dest => dest.StatusText, src => src.StatusNavigation != null ? src.StatusNavigation.NameAr : null)
                .Map(dest => dest.CreatedByPhone, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Phone : null)
                .Map(dest => dest.NeedsBudgetText, src => src.NeedsBudget.HasValue ? src.NeedsBudget.Value == true ? "نعم" : "لا" : null)
                .Map(dest => dest.CapabilityText, src => src.Capability.HasValue ? src.Capability.Value == true ? "قابلة للتطبيق" : "غير قابلة للتطبيق" : null)
                .Map(dest => dest.FeasibilityText, src => src.Feasibility.HasValue ? src.Feasibility.Value == true ? "مجدية" : "غير مجدية" : null)
                .Map(dest => dest.Attachment, src => src.Attachment != null ? !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.Attachment) ? FilesGetPath + "/" + src.Attachment : null:null)
                .Map(dest => dest.CurrentActionText, src => src.LastAction != null ? src.LastAction.TypeNavigation != null ? src.LastAction.TypeNavigation.NameAr : "غير متوفر":null)
                .Map(dest => dest.CommentsCount, src => src.IdeaComments != null ? src.IdeaComments.Where(c => c.IsApproved == true).Count() : 0)
                .Map(dest => dest.Actions, src => src.IdeaActions != null ? src.IdeaActions.ToList().Adapt<List<Dtos.IdeasActions>>(Dtos.IdeasActions.SelectConfig()):null)
                .Ignore(x => x.actionId)
                .Map(dest => dest.ActionId, src => src.LastAction != null ? src.LastAction.Type : null)
                .Ignore(x => x.declineActionId)
                .Map(dest => dest.DeclineActionId, src => (int)Enums.InnovationIdeaActions.Declined)
                .Map(dest => dest.User, src => new Users() 
                { 
                    Email = src.Email != null ? src.Email : src.CreatedByNavigation != null ? src.CreatedByNavigation.Email : string.Empty,
                    Name = src.FullName != null ? src.FullName : src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty,
                    Phone = src.MobileNo != null ? src.MobileNo : src.CreatedByNavigation != null ? src.CreatedByNavigation.Phone : string.Empty
                })

                 .Config;

        }

        public TypeAdapterConfig AddPublicConfig(string FileExtention, string FilesSavePath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Ideas, Models.Idea>().IgnoreNullValues(true)
                .Map(dest => dest.Code, src => Strings.RandomDigits(DateTime.Now, 1, 9999999999))
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IdeaActions, src => new List<Models.IdeaAction>() { new IdeaAction() { Type = (int)Enums.InnovationIdeaActions.New, CreatedDate = DateTime.Now } })
                .Map(dest => dest.Attachment, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(AttachmentBase64) ?
                 Files.SaveBase64FileToServer(Guid.NewGuid().ToString() + "." + FileExtention, AttachmentBase64, FilesSavePath) : null)
                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Ideas, Models.Idea>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Ideas, Models.Idea>().IgnoreNullValues(true)
                .Map(dest => dest.Code, src => Strings.RandomDigits(DateTime.Now, 1, 9999999999))
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }

    }

    public class ActionPermission
    {
        public bool AllowSave { get; set;}
        public bool AllowTransToReference { get; set; }
        public bool AllowTransToJobRole { get; set; }
        public bool AllowClose { get; set; }
        public bool AllowDecline { get; set; }
        public bool IsJobRoleContentManager { get; set; }
        public bool IsJobRoleEntityRepresentative { get; set; }
        public bool IsJobRoleDigitalTransformationCommittee { get; set; }

    }
}
