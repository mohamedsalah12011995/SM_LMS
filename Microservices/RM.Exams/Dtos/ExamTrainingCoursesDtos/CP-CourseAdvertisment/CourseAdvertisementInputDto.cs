using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.CP_CourseAdvertisment
{
    public class CourseAdvertisementInputDto : BaseDto<CourseAdvertisementInputDto, Models.CourseAdvertisement>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<CourseAdvertisementsCoursesDto> CourseAdvertisementsCoursesDto { get; set; }

        public TypeAdapterConfig AddConfig(int? userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<CourseAdvertisementInputDto, Models.CourseAdvertisement>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => currentDate)
                .Map(dest => dest.IsActive, src => true)
                .Map(dest => dest.IsDeleted, src => false)
                .AfterMapping((src, dest) =>
                  {
                      dest.AdvertisementsCourses = src.CourseAdvertisementsCoursesDto.Select(courses =>
                      {
                          var advertisementCourse = courses.Adapt<Models.AdvertisementsCourses>();
                          advertisementCourse.IsActive = true;
                          advertisementCourse.IsDeleted = false;
                          return advertisementCourse;
                      }).ToList();
                  })
                .Config;
        }


        public TypeAdapterConfig UpdateConfig(int? userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<CourseAdvertisementInputDto, Models.CourseAdvertisement>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => currentDate)
                .Ignore(c => c.AdvertisementsCourses)

                .Config;
        }




    }

    public class CourseAdvertisementsCoursesDto
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }


        [JsonIgnore]
        public int? TrainingCourseScheduleId { get; set; }
        public string trainingCourseScheduleId { set { TrainingCourseScheduleId = Accessor.Set(value); } get { return Accessor.Get(TrainingCourseScheduleId); } }

        [JsonIgnore]
        public int? TrainingCourseTypeId { get; set; }
        public string trainingCourseTypeId { set { TrainingCourseTypeId = Accessor.Set(value); } get { return Accessor.Get(TrainingCourseTypeId); } }

    }
}
