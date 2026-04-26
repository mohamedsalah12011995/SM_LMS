using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.ContactUs.Dtos
{
    public class ContactUs : BaseDto<ContactUs, ContactU>
    {
        [JsonIgnore]

        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        [JsonIgnore]

        public int? RegionReferenceId { get; set; }
        public string regionReferenceId { set { RegionReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(RegionReferenceId); } }
        public string Name { get; set; }
        public string Code { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        [JsonIgnore]

        public int? CreatedBy { get; set; }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        [JsonIgnore]

        public int? ModifiedBy { get; set; }
        public string modifiedBy { set { ModifiedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ModifiedBy); } }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
        [JsonIgnore]

        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public bool? IsDeleted { get; set; }
        [JsonIgnore]

        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }

        [JsonIgnore]
        public int? ItemEntityId { get; set; }
        public string itemEntityId { set { ItemEntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemEntityId); } }


        [JsonIgnore]
        public int? ItemId { get; set; }
        public string itemId { set { ItemId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemId); } }



        public string FileUrl { get; set; }
        public string FileUrlBase64 { get; set; }
        public string ComplainId { get; set; }
        public string CategoryID { get; set; }
        public string SubCategoryID { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        //Properties for 940 Inquiry
        public string CategoryDesc { get; set; }
        public string SubCategory_Desc { get; set; }
        public string ProblemStatus_Desc { get; set; }
        public string Problem_Desc { get; set; }
        public string District_Desc { get; set; }
        public string Capcha { get; set; }
        public bool? IsFollowUpContactUsUser { get; set; }
        public int? FilterStatusId { get; set; }
        public bool? IsUnderImplementationStatus { get; set; }
        public int? LastStatusId { get; set; }
        public string LastStatusStringAr { get; set; }
        public string LastStatusStringEn { get; set; }
        public bool? TransferFromManager { get; set; }
        public bool? ReturnedToManager { get; set; }
        public string ReferenceNameAr { get; set; }
        public string ReferenceNameEn { get; set; }
        public string LastActionUserName { get; set; }
        public string FromRefernceAr { get; set; }
        public string FromRefernceEn { get; set; }
        public string Notes { get; set; }
        public int? ProcessingTimesCount { get; set; }
        public string LastFeedbackNote { get; set; }
        public bool? IsImageAttached { get; set; }
        public bool? RejectedByOfficer { get; set; }
        public int? LastActionUser { get; set; }
        public UserDto LastUserAction { get; set; }
        public int? LastActionReference { get; set; }
        public int? ParentLastReference { get; set; }

        //Properties for Mayor Complain Inquiry
        [JsonIgnore]

        public int? IdeaId { get; set; }
        public string ideaId { set { IdeaId = Accessor.Set(value); } get { return Accessor.Get<int?>(IdeaId); } }

        public DateTime? FromCreatedDate { get; set; }
        public DateTime? ToCreatedDate { get; set; }
        public List<Tuple<string, string, string>> Replies { get; set; }
        public List<Tuple<string, string, string>> Comments { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

        private ExpressionStarter<ContactU> BaseFilteration()
        {
            var filter = PredicateBuilder.New<ContactU>(true);

            filter.And(u => u.EntityId == EntityId);

            if (ItemEntityId.HasValue)
                filter.And(u => u.ItemEntityId == ItemEntityId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (FromCreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date >= FromCreatedDate.Value.Date);

            if (ToCreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date <= ToCreatedDate.Value.Date);

            if (!string.IsNullOrEmpty(Name))
                filter.And(u => u.Name.Contains(Name));
            if (!string.IsNullOrEmpty(Code))
                filter.And(u => u.Code.Contains(Code));
            if (!string.IsNullOrEmpty(MobileNo))
                filter.And(u => u.MobileNo.Contains(MobileNo));

            if (!string.IsNullOrEmpty(Email))
                filter.And(u => u.Email.Contains(Email));
            if (!string.IsNullOrEmpty(UserId))
                filter.And(u => u.UserId.Contains(UserId));
            if (!string.IsNullOrEmpty(Subject))
                filter.And(u => u.Subject.Contains(Subject));

            filter.And(u => u.IsDeleted != true);

            return filter;
        }

        public ExpressionStarter<ContactU> ContactUsFilteration()
        {
            var filter = BaseFilteration();
            filter.And(u => u.ReferenceId == ReferenceId);
            return filter;
        }
        public ExpressionStarter<ContactU> FollowUpFilteration()
        {
            var filter = BaseFilteration();
            filter.And(u => u.ReferenceId == ReferenceId);
            filter.And(u => u.LastStatusId != (int)Enums.ContactStatus.New);

            if (FilterStatusId.HasValue && FilterStatusId != 0)
                filter.And(u => u.LastStatusId == FilterStatusId);

            return filter;
        }
        public ExpressionStarter<ContactU> ManagerContactUsFilteration()
        {
            var filter = BaseFilteration();
            filter.And(u => u.ReferenceId == ReferenceId);

            if (FilterStatusId.HasValue && FilterStatusId != 0)
                filter.And(u => u.LastStatusId == FilterStatusId);

            return filter;
        }
        public ExpressionStarter<ContactU> ProccessingFilteration()
        {
            var filter = BaseFilteration();

            filter.And(u => u.LastReferenceAction.ParentId == ReferenceId || u.LastActionReference == ReferenceId);
            filter.And(u => u.LastStatusId == (int)Enums.ContactStatus.Done || u.LastStatusId == (int)Enums.ContactStatus.TransferTo || u.LastStatusId == (int)Enums.ContactStatus.Returned);

            if (FilterStatusId.HasValue && FilterStatusId != 0)
                filter.And(u => u.LastStatusId == FilterStatusId);

            return filter;
        }

        public ExpressionStarter<ContactU> QualityAssuranceFilteration()
        {
            var filter = PredicateBuilder.New<ContactU>(true);

            filter.And(u => u.ReferenceId == ReferenceId);
            filter.And(u => u.EntityId == EntityId);
            filter.And(u => u.LastStatusId == (int)Enums.ContactStatus.Closed);
            filter.And(u => u.Feedbacks.Any(c => !c.IsClosed && !c.IsPositive));
            filter.And(u => u.IsDeleted != true);

            return filter;
        }
        public static TypeAdapterConfig SelectContactUsConfig(string VideosGetPath, bool FullDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<ContactU, ContactUs>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? FullDate ?
                 src.CreatedDate.Value.ToString("yyyy-MM-dd h:mm:ss tt") : src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)

                .Map(dest => dest.FileUrl, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.FileUrl) ? VideosGetPath + "/" + src.FileUrl : null)
                .Map(dest => dest.ParentLastReference, src => (src.LastUserAction != null && src.LastUserAction.Reference != null) ? src.LastUserAction.Reference.ParentId : null)

                .Map(dest => dest.RejectedByOfficer, src => (src.LastStatusId == (int)Enums.ContactStatus.Rejected
                   && (src.LastUserAction != null && (Enums.JobRole)src.LastUserAction.JobRole! == Enums.JobRole.FollowUpContactUs)) ? true : false)

                .Map(dest => dest.ReturnedToManager, src => (src.LastStatusId == (int)Enums.ContactStatus.Returned
                   && (src.LastUserAction != null && (Enums.JobRole)src.LastUserAction.JobRole! == Enums.JobRole.FollowUpContactUs)) ? true : false)

                .Map(dest => dest.IsFollowUpContactUsUser, src => (src.CreatedByNavigation != null && (Enums.JobRole)src.CreatedByNavigation.JobRole! == Enums.JobRole.FollowUpContactUs) ? true : false)

                .Map(dest => dest.Notes, src => (src.LastStatusId == (int)Enums.ContactStatus.Closed ||
                        src.LastStatusId == (int)Enums.ContactStatus.FinalRejected) ?
                        src.Actions.First(a => a.Id == src.LastActionId!.Value).Note : null)
                .Map(dest => dest.LastStatusStringAr, src => src.LastStatus.MajorStatus != null ? src.LastStatus.MajorStatus.NameAr : null)
                .Map(dest => dest.LastStatusStringEn, src => src.LastStatus.MajorStatus != null ? src.LastStatus.MajorStatus.NameEn : null)
                .Map(dest => dest.LastUserAction, src => src.LastUserAction != null ? new UserDto { Id = src.LastUserAction.Id, JobRole = src.LastUserAction.JobRole } : null)

                .Config;
        }

        public static TypeAdapterConfig SelectFollowUpConfig(bool FullDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<ContactU, ContactUs>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? FullDate ?
                 src.CreatedDate.Value.ToString("yyyy-MM-dd h:mm:ss tt") : src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)

                .Map(dest => dest.LastStatusStringAr, src => src.LastStatus != null ? src.LastStatus.NameAr : null)
                .Map(dest => dest.LastStatusStringEn, src => src.LastStatus != null ? src.LastStatus.NameEn : null)

                .Map(dest => dest.ReferenceNameAr, src => src.LastReferenceAction != null ? src.LastReferenceAction.NameAr : null)
                .Map(dest => dest.ReferenceNameEn, src => src.LastReferenceAction != null ? src.LastReferenceAction.NameEn : null)

                .Map(dest => dest.LastActionUserName, src => src.LastUserAction != null ? src.LastUserAction.Name : null)

                .Map(dest => dest.FromRefernceAr, src => src.LastUserAction != null && src.LastUserAction.Reference != null ? src.LastUserAction.Reference.NameAr : null)
                .Map(dest => dest.FromRefernceEn, src => src.LastUserAction != null && src.LastUserAction.Reference != null ? src.LastUserAction.Reference.NameEn : null)

                .Map(dest => dest.ReturnedToManager, src => (src.LastStatusId == (int)Enums.ContactStatus.Returned &&
                (src.LastUserAction != null && (Enums.JobRole)src.LastUserAction.JobRole! == Enums.JobRole.FollowUpContactUs)) ? true : false)

                .Config;
        }

        public static TypeAdapterConfig SelectQualityAssuranceConfig(bool FullDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<ContactU, ContactUs>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? FullDate ?
                 src.CreatedDate.Value.ToString("yyyy-MM-dd h:mm:ss tt") : src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)

                .Map(dest => dest.LastStatusStringAr, src => src.LastStatus != null ? src.LastStatus.NameAr : null)
                .Map(dest => dest.LastStatusStringEn, src => src.LastStatus != null ? src.LastStatus.NameEn : null)

                .Map(dest => dest.ReferenceNameAr, src => src.LastReferenceAction != null ? src.LastReferenceAction.NameAr : null)
                .Map(dest => dest.ReferenceNameEn, src => src.LastReferenceAction != null ? src.LastReferenceAction.NameEn : null)

                .Map(dest => dest.LastActionUserName, src => src.LastUserAction != null ? src.LastUserAction.Name : null)

                .Map(dest => dest.FromRefernceAr, src => src.LastUserAction.Reference != null ? src.LastUserAction.Reference.NameAr : null)
                .Map(dest => dest.FromRefernceEn, src => src.LastUserAction.Reference != null ? src.LastUserAction.Reference.NameEn : null)

                .Map(dest => dest.ReturnedToManager, src => (src.LastStatusId == (int)Enums.ContactStatus.Returned &&
                (src.LastUserAction != null && (Enums.JobRole)src.LastUserAction.JobRole! == Enums.JobRole.FollowUpContactUs)) ? true : false)

                .Map(dest => dest.LastFeedbackNote, src => src.Feedbacks != null ? src.Feedbacks.Where(x => !x.IsPositive).OrderByDescending(x => x.Id).First().Note : null)

                .Config;

        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<ContactUs, ContactU>().IgnoreNullValues(true)
                .Map(dest => dest.ModifiedBy, src => userId)
                .Map(dest => dest.ModifiedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId, string Code)
        {
            return new TypeAdapterConfig()
                .NewConfig<ContactUs, ContactU>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.Code, src => Code)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }
    }
}
