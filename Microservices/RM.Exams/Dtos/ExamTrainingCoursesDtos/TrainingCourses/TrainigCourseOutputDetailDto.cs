using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Exams.ExamEnums;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.TrainingCourses
{
    public class TrainigCourseOutputDetailDto : BaseDto<TrainigCourseOutputDetailDto, Models.TrainingCourse>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Code { get; set; }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }

        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        [JsonIgnore]
        public int? Type { get; set; }
        public bool? HasCertificate { get; set; }
        public bool LoginRequired { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string type { set { Type = Accessor.Set(value); } get { return Accessor.Get(Type); } }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.TrainingCourse, TrainigCourseOutputDetailDto>()
                .Map(dest => dest.LoginRequired, src => src.Type == (int)CourseType.Internal ? true : false)

                  .Config;
        }



    }




}
