
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos
{
    public class ExamQuestionType:BaseDto<ExamQuestionType,Models.ExamQuestionType>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string? ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string? TextAr { get; set; }
        public string? TextEn { get; set; }
        public bool? HasDataSource { get; set; }
        public override void AddCustomMappings()
        {
            SetCustomMappings();

            SetCustomMappingsInverse()
                .Map(dest => dest.HasDataSource, src => src.HasDataSource.HasValue?true:false);

        }

    }
}
