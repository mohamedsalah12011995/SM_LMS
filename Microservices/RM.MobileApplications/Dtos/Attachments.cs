
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.Text.Json.Serialization;

namespace RM.MobileApplications.Dtos
{
    public class Attachments:BaseDto<Attachments,Models.Attachment>
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
        public string UrlBase64 { get; set; }
        public string Extention { get; set; }
        public DateTime? CreatedDate { get; set; }


        public static TypeAdapterConfig SelectConfig(string ImagesGetPath,string ImagesSavePath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Attachment, Attachments>()
                .Map(dest=>dest.Url,src => ImagesGetPath + "/" + src.Url)
                .Map(dest=>dest.UrlBase64, src => src.Url != null ? Images.ConvertFromImageToBase64(ImagesSavePath + "/" + src.Url) : null)
                    .Config;

        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Attachments, Models.Attachment>().IgnoreNullValues(true)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId,string ImagesSavePath,string ThumbsSavePath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Attachments, Models.Attachment>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.EntityId, src => (int)Enums.Entities.MobileApplication)
                .Map(dest => dest.Url, src => Images.SaveSingleImageOnServer(src.Url, null, ImagesSavePath, true, 400, ThumbsSavePath))
                .Config;
        }
    }
}
