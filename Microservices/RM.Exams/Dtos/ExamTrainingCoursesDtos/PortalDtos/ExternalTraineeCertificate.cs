using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Exams.Dtos.Exams.Certifications;
using RM.Exams.Helper;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.PortalDtos
{
    public class TraineeCertificate : BaseDto<TraineeCertificate, CertificateData>
    {
        [JsonIgnore]
        public int? CertificateId { get; set; }
        public string certificateId { set { CertificateId = Accessor.Set(value); } get { return Accessor.Get(CertificateId); } }

        public string TranieeName { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Signature { get; set; }
        public string LogoUrl { get; set; }
        public string IssueDate { get; set; }
        public string CertificateNumber { get; set; }
        public string CertificateThemeName { get; set; }

        public static TypeAdapterConfig SelectConfig(string ImagesGetPath, string tranieeName, string certificateThemeName, DateTime? examEndAt, string certificateNumber)
        {
            return new TypeAdapterConfig()
                .NewConfig<CertificateData, TraineeCertificate>()

                 .Map(dest => dest.LogoUrl, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.LogoUrl) ? $"{ImagesGetPath}/{src.LogoUrl}" : string.Empty)
                 .Map(dest => dest.TranieeName, src => tranieeName)

                 .Map(dest => dest.CertificateNumber, src => !string.IsNullOrEmpty(certificateNumber) ? certificateNumber
                 : RandomCertifcationNumberGenerator.GenerateCertifcationNumber())

                 .Map(dest => dest.IssueDate, src => examEndAt.Value.ToString("yyyy-MM-dd"))
                 .Map(dest => dest.CertificateThemeName, src => certificateThemeName)
                 .Config;
        }




    }
}
