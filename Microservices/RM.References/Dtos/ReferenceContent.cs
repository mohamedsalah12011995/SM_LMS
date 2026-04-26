using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.References.Dtos
{

    public class ReferenceContent : BaseDto<ReferenceContent, Models.ReferenceContent>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string Url { get; set; }
        public string ChiefName { get; set; }
        public string ChiefNameEn { get; set; }
        public string ChiefImage { get; set; }
        public string ChiefImageBase64 { get; set; }
        public string ChiefWord { get; set; }
        public string ChiefWordEn { get; set; }
        public string Latitude { get; set; }
        public string Longtitude { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string AddressEn { get; set; }
        public string Region { get; set; }
        public string Mailbox { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string ManagerName { get; set; }
        public string RegistrationNo { get; set; }
        public DateTime? EndDateRegistrationNo { get; set; }
        public string EndDateRegistrationNoString { get; set; }


        public static TypeAdapterConfig GetCustomMapping(string referenceImagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.ReferenceContent, ReferenceContent>()
                .Map(dest => dest.ChiefImage, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.ChiefImage) ? $"{referenceImagesGetPath}/{src.ChiefImage}" : null)
                .Map(dest => dest.EndDateRegistrationNoString, src => src.EndDateRegistrationNo.HasValue ? src.EndDateRegistrationNo.Value.ToString("dd-MM-yyyy") : null)

                .Config;
        }



        public TypeAdapterConfig AddConfig(string chiefImageBase64, string referenceImagesSavePath, int referenceId)
        {
            return new TypeAdapterConfig()
                .NewConfig<ReferenceContent, Models.ReferenceContent>().IgnoreNullValues(true)
                .Map(dest => dest.ReferenceId, src => referenceId)
                .Map(dest => dest.ChiefImage, src =>
                !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(chiefImageBase64)
                ? Images.SaveSingleImageOnServer(chiefImageBase64, null, referenceImagesSavePath, false, null, null)
                : src.ChiefImage)
                .Config;


        }
    }

}
