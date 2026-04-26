using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone
{
    public class UserDepartmentInput
    {

        [JsonIgnore]
        public int? ExamId { get; set; }

        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get(ExamId); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }

        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }



    }
}
