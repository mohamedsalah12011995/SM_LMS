using LinqKit;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.TrainingCourseSchedule
{
    public class CourseScheduleListInput : BaseDto<CourseScheduleListInput, Models.TrainingCourseSchedule>
    {
        [JsonIgnore]
        public int? CourseId { get; set; }
        public string courseId { set { CourseId = Accessor.Set(value); } get { return Accessor.Get(CourseId); } }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsExpiredFilter { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.TrainingCourseSchedule> Filteration(DateTime currentDate)
        {
            var filter = PredicateBuilder.New<Models.TrainingCourseSchedule>(true);

            filter.And(u => u.CourseId == CourseId);

            if (FromDate.HasValue && ToDate.HasValue)
            {
                filter.And(u => u.StartDate >= FromDate.Value && u.EndDate <= ToDate.Value);
            }
            if (IsExpiredFilter.HasValue)
            {
                if (IsExpiredFilter == true)
                    filter.And(u => u.EndDate.Date < currentDate.Date);
                else
                    filter.And(u => u.EndDate.Date >= currentDate.Date);

            }
            return filter;
        }


    }

}
