using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone.PortalDtos
{
    public class GetUserExamDto
    {
        [JsonIgnore]
        public int? UserApplicationExamId { get; set; }
        public string userApplicationExamId { set { UserApplicationExamId = Accessor.Set(value); } get { return Accessor.Get(UserApplicationExamId); } }

    }
}
