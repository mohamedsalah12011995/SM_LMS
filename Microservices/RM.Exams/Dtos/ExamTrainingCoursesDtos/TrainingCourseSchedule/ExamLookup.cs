using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.TrainingCourseSchedule
{
    public class ExamLookup
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        [JsonIgnore]
        public int? CertificateId { get; set; }
        public string certificateId { set { CertificateId = Accessor.Set(value); } get { return Accessor.Get(CertificateId); } }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public string CertificateTitleAr { get; set; }
        public string CertificateTitleEn { get; set; }
    }
}
