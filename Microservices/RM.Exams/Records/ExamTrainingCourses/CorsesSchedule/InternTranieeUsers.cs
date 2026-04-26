using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Records.ExamTrainingCourses
{
    public record InternTraineeUsers
    {
        [JsonIgnore]
        public int? UserId { get; set; }

        public string userId { set { UserId = Accessor.Set(value); } get { return Accessor.Get<int?>(UserId); } }

        public string Name { get; set; }
        public string EmployeeID { get; set; }
        public string IdCardNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsSelected { get; set; }

    }
}
