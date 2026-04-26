using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.AssignToExam
{
    public class TraineeExamInputDto
    {

        [JsonIgnore]
        public int? TrainingCourseScheduleId { get; set; }
        public string trainingCourseScheduleId { set { TrainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get<int?>(TrainingCourseScheduleId); } }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<TraineesForExam> Trainees { get; set; }


        public TypeAdapterConfig AddInternalTraineeConfig(int userId, int? examId, int id, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<TraineeExamInputDto, Models.InternalCourseExams>().IgnoreNullValues(true)
                .Map(dest => dest.ExamId, src => examId)
                .Map(dest => dest.InternalCourseTraineeId, src => id)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => currentDate)

                .Config;
        }

        public TypeAdapterConfig UpdateInternalTraineeConfig(int userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<TraineeExamInputDto, Models.InternalCourseExams>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => currentDate)

                .Config;
        }
        public TypeAdapterConfig AddExternalTraineeConfig(int userId, int? examId, int id, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<TraineeExamInputDto, Models.ExternalCourseExams>().IgnoreNullValues(true)
                .Map(dest => dest.ExamId, src => examId)
                .Map(dest => dest.ExternalCourseTranieeId, src => id)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => currentDate)

                .Config;
        }
        public TypeAdapterConfig UpdateExternalTraineeConfig(int userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<TraineeExamInputDto, Models.ExternalCourseExams>().IgnoreNullValues(true)
                  .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => currentDate)

                .Config;
        }


    }

    public class TraineesForExam
    {
        // id : ExternalCourseTrainee   or InternalCourseTrainee table
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

    }




}
