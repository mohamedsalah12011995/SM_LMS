using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.Officials.Dtos
{
    public class OfficialDto : BaseDto<OfficialDto, Models.Official>, IFilteration<Models.Official>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string ThumbPic { get; set; }
        public string OriginalPic { get; set; }
        public string OriginalPicBase64 { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string PersonCreatedBy { get; set; }
        public string StatusString { get; set; }
        public string Email { get; set; }
        public string JobTitleAr { get; set; }
        public string JobTitleEn { get; set; }
        public string CvUrl { get; set; }
        public string CvUrlBase64 { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? PeriodFrom { get; set; }
        public DateTime? PeriodTo { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public int? OfficialOrder { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string PeriodFromString { get; set; }
        public string PeriodToString { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Official> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Official>(true);

            if (ReferenceId.HasValue)
                filter.And(u => u.ReferenceId == ReferenceId);

            if (EntityId.HasValue)
                filter.And(u => u.EntityId == EntityId);

            if (!string.IsNullOrEmpty(PersonCreatedBy))
                filter.And(u => u.CreatedByNavigation.Name.Contains(PersonCreatedBy));

            if (!string.IsNullOrEmpty(PersonUpdatedBy))
                filter.And(u => u.UpdatedByNavigation.Name.Contains(PersonUpdatedBy));

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));

            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);
            return filter;
        }


        public static TypeAdapterConfig SelectConfig(Enums.UsersRoles RequestUserRole, string ThumbsGetPath, string ImagesGetPath, string DocumentsGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Official, OfficialDto>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)

                .Map(dest => dest.OriginalPic, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.OriginalPic) ? $"{ThumbsGetPath}/{src.OriginalPic}" : null)
                .Map(dest => dest.ThumbPic, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.OriginalPic) ? $"{ImagesGetPath}/{src.OriginalPic}" : null)
                .Map(dest => dest.CvUrl, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.CvUrl) ? $"{DocumentsGetPath}/{src.CvUrl}" : null)

                .Map(dest => dest.StatusString, src => RequestUserRole != Enums.UsersRoles.NormalUser ? (src.IsActive == true ? "فعال" : "غير فعال") : null)
                .Map(dest => dest.PeriodFromString, src => src.PeriodFrom.HasValue ? src.PeriodFrom.GetValueOrDefault().ToString("yyyy-MM-dd") : null)
                .Map(dest => dest.PeriodToString, src => src.PeriodTo.HasValue ? src.PeriodTo.GetValueOrDefault().ToString("yyyy-MM-dd") : null)

                .Config;

        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<OfficialDto, Models.Official>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)

                .Config;
        }



        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                  .NewConfig<OfficialDto, Models.Official>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsActive, src => false)


                .Config;
        }



    }
}
