using LinqKit;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.CP_CourseAdvertisment
{
    public class CourseAdvertisementCPListInputDto : BaseDto<CourseAdvertisementCPListInputDto, CourseAdvertisement>, IFilteration<CourseAdvertisement>
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? IsActive { get; set; }


        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<CourseAdvertisement> Filteration()
        {
            var filter = PredicateBuilder.New<CourseAdvertisement>(true);

            filter.And(u => u.ReferenceId == ReferenceId);

            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));

            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));

            if (FromDate.HasValue && ToDate.HasValue)
            {
                filter.And(u => u.FromDate >= FromDate.Value && u.ToDate <= ToDate.Value);
            }
            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }




    }
}
