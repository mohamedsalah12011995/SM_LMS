using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Multimedia.Dtos
{
    public class Attachments : BaseDto<Attachments, Models.Attachment>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? ItemId { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string itemId { set { ItemId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Url { get; set; }
        public string Extention { get; set; }
        public DateTime? CreatedDate { get; set; }

        public static TypeAdapterConfig SelectConfig(string galleryImagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Attachment, Attachments>()
                .Map(dest => dest.Url, src => $"{galleryImagesGetPath}/{src.Url}")

                .Config;

        }
        public static TypeAdapterConfig SelectAlbumConfig(string galleryImagesSavePath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Attachment, Attachments>()
                .Map(dest => dest.Url, src => Images.ConvertFromImageToBase64(galleryImagesSavePath+"/"+src.Url))

                .Config;

        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                  .NewConfig<Attachments, Models.Attachment>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.EntityId, src => (int)Enums.Entities.ImageGallery)


                .Config;
        }
    }
}
