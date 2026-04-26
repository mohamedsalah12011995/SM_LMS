using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.Exams.Certifications
{
    public class CertificateInputDto
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Signature { get; set; }
        public string LogoBase64 { get; set; }

        public TypeAdapterConfig AddConfig(int? userId, string ImagesSavePath, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<CertificateInputDto, Models.Certificate>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => currentDate)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.LogoUrl, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.LogoBase64) ?
                Images.SaveSingleImageOnServer(src.LogoBase64, null, ImagesSavePath, false, null, null) : null)

                .Config;
        }


        public TypeAdapterConfig UpdateConfig(int? userId, string ImagesSavePath, Models.Certificate certificate, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<CertificateInputDto, Models.Certificate>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => currentDate)

                .Map(dest => dest.LogoUrl, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.LogoBase64) ?
                Images.SaveSingleImageOnServer(src.LogoBase64, null, ImagesSavePath, false, null, null) : certificate.LogoUrl)


                .Config;
        }

    }



}
