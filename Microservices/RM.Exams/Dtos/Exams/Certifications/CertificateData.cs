using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.Exams.Certifications
{
    public class CertificateData
    {

        [JsonIgnore]
        public int? CertificateId { get; set; }
        public string certificateId { set { CertificateId = Accessor.Set(value); } get { return Accessor.Get(CertificateId); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Signature { get; set; }
        public string LogoUrl { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
