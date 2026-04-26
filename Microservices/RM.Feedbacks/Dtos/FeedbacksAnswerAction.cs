
using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Feedbacks.Dtos
{
    public class FeedbacksAnswerAction
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }


        [JsonIgnore]
        public int? FeedbacksId { get; set; }
        public string feedbacksId { set { FeedbacksId = Accessor.Set(value); } get { return Accessor.Get<int?>(FeedbacksId); } }

        public DateTime? CreatedDate { get; set; }
        public int? Year { get; set; }
        public int? Quarter { get; set; }

        [JsonIgnore]
        public int? ItemId { get; set; }
        public string itemId { set { ItemId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemId); } }

        public bool? IsHelpful { get; set; }
        public string ItemUrl { get; set; }
        public virtual List<FeedbacksAnswer> FeedbacksAnswers { get; set; } = new List<FeedbacksAnswer>();

        public List<string> Emails { get; set; } = new List<string>();

        public string Subject { get; set; }
        public string Body { get; set; }
        public string FileName { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.FeedbacksAnswerAction, FeedbacksAnswerAction>()
                    .Config;
        }

        public TypeAdapterConfig UpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<FeedbacksAnswerAction, Models.FeedbacksAnswerAction>().IgnoreNullValues(true)
                .Config;
        }

        public TypeAdapterConfig AddConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<FeedbacksAnswerAction, Models.FeedbacksAnswerAction>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedDate,src => DateTime.Now)
                .Config;
        }
    }
}
