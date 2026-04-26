using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Feedbacks.Dtos
{
    public class FeedbacksDataSource
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string TextAr { get; set; }
        public string TextEn { get; set; }

        public bool? HasNote { get; set; }
        public bool? IsHelpful { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }


        [JsonIgnore]
        public int? FeedbacksId { get; set; }
        public string feedbacksId { set { FeedbacksId = Accessor.Set(value); } get { return Accessor.Get<int?>(FeedbacksId); } }


        [JsonIgnore]
        public int? LessAverageId { get; set; }
        public string lessAverageId { set { LessAverageId = Accessor.Set(value); } get { return Accessor.Get<int?>(LessAverageId); } }


        [JsonIgnore]
        public int? AverageId { get; set; }
        public string averageId { set { AverageId = Accessor.Set(value); } get { return Accessor.Get<int?>(AverageId); } }


        [JsonIgnore]
        public int? AboveAverageId { get; set; }
        public string aboveAverageId { set { AboveAverageId = Accessor.Set(value); } get { return Accessor.Get<int?>(AboveAverageId); } }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.FeedbacksDataSource, FeedbacksDataSource>()
                    .Config;
        }

        public TypeAdapterConfig UpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<FeedbacksDataSource, Models.FeedbacksDataSource>().IgnoreNullValues(true)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int feedbackId)
        {
            return new TypeAdapterConfig()
                .NewConfig<FeedbacksDataSource, Models.FeedbacksDataSource>().IgnoreNullValues(true)
                .Map(dest => dest.FeedbacksId, src => feedbackId)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }
    }
}
