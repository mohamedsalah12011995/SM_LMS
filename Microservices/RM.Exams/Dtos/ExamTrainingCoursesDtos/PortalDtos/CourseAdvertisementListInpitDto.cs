using LinqKit;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.PortalDtos
{
    public class CourseAdvertisementPortalListInputDto
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }

        [JsonIgnore]
        public int? CourseTypeId { get; set; }
        public string courseTypeId { set { CourseTypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(CourseTypeId); } }

        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.CourseAdvertisement> Filteration(DateTime currentDate)
        {
            var filter = PredicateBuilder.New<Models.CourseAdvertisement>(true);

            filter.And(u => u.ReferenceId == ReferenceId);
            filter.And(u => u.ToDate.Date >= currentDate.Date);

            filter.And(u => u.AdvertisementsCourses
            .Any(c => c.TrainingCourseTypeId == CourseTypeId && c.IsDeleted == false && c.IsActive == true));

            filter.And(u => u.IsActive == true);
            filter.And(u => u.IsDeleted != true);

            return filter;

        }




    }

}
