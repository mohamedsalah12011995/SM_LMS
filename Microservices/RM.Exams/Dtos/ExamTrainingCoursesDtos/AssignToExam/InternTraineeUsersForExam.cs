using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.AssignToExam
{
    public class InternTraineeUsersForExam : BaseDto<InternTraineeUsersForExam, Models.InternalCourseTrainees>
    {

        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }


        [JsonIgnore]
        public int? TraineeId { get; set; }

        public string traineeId { set { TraineeId = Accessor.Set(value); } get { return Accessor.Get<int?>(TraineeId); } }

        public string Code { get; set; }
        public string IdCardNumber { get; set; }
        public string TraineeName { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsSelected { get; set; }



    }
}
