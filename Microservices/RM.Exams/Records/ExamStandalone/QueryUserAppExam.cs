using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.ExamStandalone
{
    public record QueryUserAppExam
    {
        [JsonIgnore]
        public int? ExamId { get; set; }

        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get<int?>(ExamId); } }

        public string IdCardNumber { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
