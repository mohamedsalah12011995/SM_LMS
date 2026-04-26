using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.TrainingCourseSchedule
{
    public class CourseScheduelListOutput : BaseDto<CourseScheduelListOutput, Models.TrainingCourseSchedule>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string DepartmentNameAr { get; set; }
        public string DepartmentNameEn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public string ExamTitleAr { get; set; }
        public string ExamTitleEn { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsClosed { get; set; }
        public string IsClosedStringAr { get; set; }
        public string IsClosedStringEn { get; set; }

        public string StatusStringAr { get; set; }
        public string StatusStringEn { get; set; }


        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.TrainingCourseSchedule, CourseScheduelListOutput>()

                .Map(dest => dest.StartDateString, src => src.StartDate.ToString("yyyy-MM-dd"))
                .Map(dest => dest.EndDateString, src => src.EndDate.ToString("yyyy-MM-dd"))

                .Map(dest => dest.DepartmentNameAr, src => src.DepartmentReference != null ? src.DepartmentReference.NameAr : string.Empty)
                .Map(dest => dest.DepartmentNameEn, src => src.DepartmentReference != null ? src.DepartmentReference.NameEn : string.Empty)

                .Map(dest => dest.ExamTitleAr, src => src.Exam != null ? src.Exam.TitleAr : string.Empty)
                .Map(dest => dest.ExamTitleEn, src => src.Exam != null ? src.Exam.TitleEn : string.Empty)

                .Map(dest => dest.StatusStringAr, src => src.IsActive == true ? "فعال" : "غير فعال")
                .Map(dest => dest.StatusStringEn, src => src.IsActive == true ? "Active" : "Inactive")


                .Map(dest => dest.IsClosedStringAr, src => src.IsClosed == true ? "مغلق" : "سارى")
                .Map(dest => dest.IsClosedStringEn, src => src.IsClosed == true ? "Closed" : "Activated")

                .Config;
        }


    }
}
