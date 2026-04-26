using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos
{

    public class ExamQuestionAnswer : BaseDto<ExamQuestionAnswer, Models.ExamQuestionAnswer>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string? ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        [JsonIgnore]
        public int? QuestionId { get; set; }
        public string? questionId { set { QuestionId = Accessor.Set(value); } get { return Accessor.Get<int?>(QuestionId); } }
        [JsonIgnore]
        public int? DataSourceId { get; set; }
        public string? dataSourceId { set { DataSourceId = Accessor.Set(value); } get { return Accessor.Get<int?>(DataSourceId); } }
        [JsonIgnore]
        public int? ExamAnswerActionId { get; set; }
        public string? examAnswerActionId { set { ExamAnswerActionId = Accessor.Set(value); } get { return Accessor.Get<int?>(ExamAnswerActionId); } }


        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public string? CreatedDateString { get; set; }
        public string? UpdatedDateString { get; set; }


        public string? Text { get; set; }
        public int? Value { get; set; }



    }

}
