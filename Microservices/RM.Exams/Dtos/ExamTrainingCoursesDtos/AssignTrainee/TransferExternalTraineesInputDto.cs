using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.AssignTrainee
{
    public class TransferExternalTraineesInputDto
    {

        [JsonIgnore]
        public int? CourseId { get; set; }
        public string courseId { set { CourseId = Accessor.Set(value); } get { return Accessor.Get(CourseId); } }

        [JsonIgnore]
        public int? DepartmentReferenceId { get; set; }
        public string departmentReferenceId { set { DepartmentReferenceId = Accessor.Set(value); } get { return Accessor.Get(DepartmentReferenceId); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }


        [JsonIgnore]
        public int? TrainingCourseScheduleId { get; set; }
        public string trainingCourseScheduleId { set { TrainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get(TrainingCourseScheduleId); } }

        public List<ExternalTraineesForAssign> Trainees { get; set; }

    }

}
