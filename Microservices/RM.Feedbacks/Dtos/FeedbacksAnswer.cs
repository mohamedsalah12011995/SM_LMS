
using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Feedbacks.Dtos
{
    public class FeedbacksAnswer
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string Text { get; set; }

        [JsonIgnore]
        public int? FeedbacksDataSourceId { get; set; }
        public string feedbacksDataSourceId { set { FeedbacksDataSourceId = Accessor.Set(value); } get { return Accessor.Get<int?>(FeedbacksDataSourceId); } }

        [JsonIgnore]
        public int? FeedbacksAnswerActionId { get; set; }
        public string feedbacksAnswerActionId { set { FeedbacksAnswerActionId = Accessor.Set(value); } get { return Accessor.Get<int?>(FeedbacksAnswerActionId); } }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.FeedbacksAnswer, FeedbacksAnswer>()
                    .Config;
        }

        public TypeAdapterConfig UpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<FeedbacksAnswer, Models.FeedbacksAnswer>().IgnoreNullValues(true)
                .Config;
        }

        public TypeAdapterConfig AddConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<FeedbacksAnswer, Models.FeedbacksAnswer>().IgnoreNullValues(true)
                .Config;
        }
    }
}
