using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Exams.ExamEnums;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.TrainingCourses
{
    public class TrainigCourseInputDto : BaseDto<TrainigCourseInputDto, Models.TrainingCourse>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }

        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        [JsonIgnore]
        public int? Type { get; set; }
        public bool? HasCertificate { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string type { set { Type = Accessor.Set(value); } get { return Accessor.Get(Type); } }


        public TypeAdapterConfig AddConfig(int userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<TrainigCourseInputDto, Models.TrainingCourse>().IgnoreNullValues(true)
                .Map(dest => dest.Code, src => $"{currentDate.ToString("yyMMddhhmm")}{Strings.RandomDigits(4).ToString()}")
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => currentDate)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.LoginRequired, src => src.Type == (int)CourseType.Internal ? true : false)

                .Config;
        }


        public TypeAdapterConfig UpdateConfig(int? userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<TrainigCourseInputDto, Models.TrainingCourse>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => currentDate)
                .Map(dest => dest.LoginRequired, src => src.Type == (int)CourseType.Internal ? true : false)

                .Config;
        }



    }




}
