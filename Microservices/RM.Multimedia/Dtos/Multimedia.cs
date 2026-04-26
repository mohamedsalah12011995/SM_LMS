using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using System.Text.Json.Serialization;

namespace RM.Multimedia.Dtos
{
    public class MultimediaDto : BaseDto<MultimediaDto, Models.Multimedia>, IFilteration<Models.Multimedia>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ImageUrl { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string PersonCreatedBy { get; set; }
        public string StatusString { get; set; }
        public string Url { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string ActivatedDateString { get; set; }
        public string CreatedDateString { get; set; }
        public string DeletedDateString { get; set; }
        public string UpdatedDateString { get; set; }



        public List<Attachments> ListOfImages { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.Multimedia> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Multimedia>(true);

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


        public static TypeAdapterConfig SelectConfig(Enums.UsersRoles RequestUserRole, bool isImageGallery, string galleryImagesGetPath = null)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Multimedia, MultimediaDto>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.ImageUrl, src => !isImageGallery ? Images.GetYouTubeThumbnail(src.Url)
                : $"{galleryImagesGetPath}/{src.Attachments.OrderByDescending(v => v.Id).Select(v => v.Url).FirstOrDefault()}")
                .Map(dest => dest.StatusString, src => RequestUserRole != Enums.UsersRoles.NormalUser ? src.IsActive == true ? "فعال" : "غير فعال" : null)

                .Config;

        }

        public static TypeAdapterConfig SelectAlbumDetailsConfig(string galleryImagesSavePath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Multimedia, MultimediaDto>()

                .Map(dest => dest.ListOfImages, src => src.Attachments != null ? src.Attachments.Adapt<List<Attachments>>(Attachments.SelectAlbumConfig(galleryImagesSavePath)) : new List<Attachments>())

                .Config;

        }
        public TypeAdapterConfig UpdateConfig(int? userId, bool isVedio)
        {
            return new TypeAdapterConfig()
                .NewConfig<MultimediaDto, Models.Multimedia>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.EntityId, src => isVedio ? (int)Enums.Entities.VideoGallery : (int)Enums.Entities.ImageGallery)

                .Config;
        }



        public TypeAdapterConfig AddConfig(int? userId, bool isVedio)
        {
            return new TypeAdapterConfig()
                  .NewConfig<MultimediaDto, Models.Multimedia>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.EntityId, src => isVedio ? (int)Enums.Entities.VideoGallery : (int)Enums.Entities.ImageGallery)
                .Config;
        }





    }
}
