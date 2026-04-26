using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.Exams.Certifications
{
    public class CertificateDetailInput
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

    }



}
