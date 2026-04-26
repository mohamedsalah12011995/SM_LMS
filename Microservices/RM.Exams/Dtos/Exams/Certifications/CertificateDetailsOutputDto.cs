using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.Exams.Certifications
{
    public class CertificateDetailsOutputDto
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Signature { get; set; }
        public string LogoUrl { get; set; }
        public bool? IsActive { get; set; }


        public static TypeAdapterConfig SelectConfigForDetails(string ImagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Certificate, CertificateDetailsOutputDto>()

                 .Map(dest => dest.LogoUrl, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.LogoUrl) ? $"{ImagesGetPath}/{src.LogoUrl}" : string.Empty)


                 .Config;
        }



    }



}
