using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos
{
    public class ExamAnswerAction : BaseDto<ExamAnswerAction, Models.ExamAnswerAction>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }

        public string? ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string? createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        [JsonIgnore]
        public int? ExamId { get; set; }
        public string? examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get<int?>(ExamId); } }

        [JsonIgnore]
        public int? ItemId { get; set; }
        public string? itemId { set { ItemId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemId); } }

        public string? Note { get; set; }
        public DateTime? CreatedDate { get; set; }

        public List<ExamQuestionAnswer> ExamQuestionAnswers { get; set; } = new List<ExamQuestionAnswer>();

    }

}
