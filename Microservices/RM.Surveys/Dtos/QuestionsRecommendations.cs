
using DocumentFormat.OpenXml.Office2010.Excel;
using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Surveys.Dtos
{
    public class QuestionsRecommendations
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        [JsonIgnore]
        public int? LessAverageId { get; set; }
        public string lessAverageId { set { LessAverageId = Accessor.Set(value); } get { return Accessor.Get(LessAverageId); } }


        [JsonIgnore]
        public int? AverageId { get; set; }
        public string averageId { set { AverageId = Accessor.Set(value); } get { return Accessor.Get(AverageId); } }


        [JsonIgnore]
        public int? AboveAverageId { get; set; }
        public string aboveAverageId { set { AboveAverageId = Accessor.Set(value); } get { return Accessor.Get(AboveAverageId); } }


        [JsonIgnore]
        public int? QuestionId { get; set; }
        public string questionId { set { QuestionId = Accessor.Set(value); } get { return Accessor.Get(QuestionId); } }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.QuestionsRecommendations, QuestionsRecommendations>()
                .Config;
        }

        public TypeAdapterConfig AddConfig(int QuestId)
        {
            return new TypeAdapterConfig()
                .NewConfig<QuestionsRecommendations, Models.QuestionsRecommendations>()
                .Map(dest => dest.QuestionId, src => QuestId)
                .Config;
        }

        public TypeAdapterConfig UpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<QuestionsRecommendations, Models.QuestionsRecommendations>()
                .Ignore(x => x.Id)
                .Ignore(x => x.QuestionId)
                .Config;
        }

        public bool IsNull()
        {
            if (string.IsNullOrEmpty(lessAverageId) && string.IsNullOrEmpty(averageId) && string.IsNullOrEmpty(aboveAverageId) && string.IsNullOrEmpty(questionId))
                return true;
            return false;
        }
    }
}
