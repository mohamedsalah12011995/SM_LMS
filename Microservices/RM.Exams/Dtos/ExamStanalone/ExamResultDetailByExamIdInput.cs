using LinqKit;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone
{
    public class ExamResultDetailByExamIdInput : BaseDto<ExamResultDetailByExamIdInput, Models.UserApplicationExam>, IFilteration<Models.UserApplicationExam>
    {
        [JsonIgnore]
        public int? ExamId { get; set; }
        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get(ExamId); } }

        public int Year { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<UserApplicationExam> Filteration()
        {
            var filter = PredicateBuilder.New<UserApplicationExam>(true);

            filter.And(u => u.ExamId == ExamId);
            filter.And(u => u.FromDate.Value.Year == Year);
            filter.And(u => u.IsDeleted != true);

            return filter;
        }
    }
}
