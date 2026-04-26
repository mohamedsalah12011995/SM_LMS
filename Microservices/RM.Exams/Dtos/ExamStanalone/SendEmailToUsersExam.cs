using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone
{
    public class SendEmailToUsersExam
    {
        [JsonIgnore]
        public int? ExamId { get; set; }

        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get(ExamId); } }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public List<UserData> Users { get; set; }
    }
}
