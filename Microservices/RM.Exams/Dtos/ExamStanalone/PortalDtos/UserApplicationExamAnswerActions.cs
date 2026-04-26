using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone.PortalDtos
{
    public class UserApplicationExamAnswerActions
    {
        [JsonIgnore]
        public int? Id { get; set; } // userApplicationExamId

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        [JsonIgnore]
        public int? ExamId { get; set; }
        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get<int?>(ExamId); } }


        public string Note { get; set; }

        public List<ExamQuestionAnswer> ExamQuestionAnswers { get; set; } = new List<ExamQuestionAnswer>();

    }
}
