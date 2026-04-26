using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.Exam.Certifications
{
    public record ActivationCertificate
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public bool IsActive { get; set; }


    }
}
