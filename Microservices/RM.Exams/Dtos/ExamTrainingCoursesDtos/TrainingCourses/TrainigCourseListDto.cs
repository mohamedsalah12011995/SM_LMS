using LinqKit;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamTrainingCourses.TrainingCourses
{
    public class TrainigCourseListDto : BaseDto<TrainigCourseListDto, TrainingCourse>, IFilteration<TrainingCourse>
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string Code { get; set; }
        public int? Type { get; set; }
        public bool? IsActive { get; set; }


        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<TrainingCourse> Filteration()
        {
            var filter = PredicateBuilder.New<TrainingCourse>(true);

            filter.And(u => u.ReferenceId == ReferenceId);


            if (Type.HasValue)
                filter.And(u => u.Type == Type);

            if (!string.IsNullOrEmpty(Code))
                filter.And(u => u.Code.Trim() == Code.Trim());

            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));

            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }




    }




}
