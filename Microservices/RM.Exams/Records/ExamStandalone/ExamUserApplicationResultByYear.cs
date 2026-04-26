using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.ExamStandalone
{
    public record ExamUserApplicationResultByYear
    {

        [JsonIgnore]
        public int? ExamId { get; set; }
        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get(ExamId); } }

        public int Year { get; set; }

    }
}
