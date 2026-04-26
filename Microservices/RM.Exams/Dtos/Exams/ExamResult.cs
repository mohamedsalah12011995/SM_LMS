
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos
{
    public class ExamResult
    {
        [JsonIgnore]
        public int? ExamId { get; set; }
        public string? examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get<int?>(ExamId); } }

        public string? ExamTitle { get; set; }

        [JsonIgnore]
        public int? QuestionId { get; set; }
        public string? questionId { set { QuestionId = Accessor.Set(value); } get { return Accessor.Get<int?>(QuestionId); } }

        public string? Question { get; set; }

        public int? Count { get; set; }

        [JsonIgnore]
        public int? DataSourceId { get; set; }
        public string? dataSourceId { set { DataSourceId = Accessor.Set(value); } get { return Accessor.Get<int?>(DataSourceId); } }
        public string? Choice { get; set; }
        public List<string> DropDownIds { get; set; } = new List<string>();

    }


}
