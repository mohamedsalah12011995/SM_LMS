
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.Text.Json.Serialization;

namespace RM.MobileApplications.Dtos
{
    public class QuestionsAnswers:BaseDto<QuestionsAnswers,Models.QuestionsAnswer>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        [JsonIgnore]
        public int? ItemId { get; set; }
        public string itemId { set { ItemId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemId); } }
        public string QuestionAr { get; set; }
        public string QuestionEn { get; set; }
        public string AnswerAr { get; set; }
        public string AsnwerEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public DateTime? UpdatedDate { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public bool? IsDeleted { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.QuestionsAnswer, QuestionsAnswers>()
                    .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<QuestionsAnswers, Models.QuestionsAnswer>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<QuestionsAnswers, Models.QuestionsAnswer>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.EntityId, src => (int)Enums.Entities.MobileApplication)
                .Config;
        }
    }
}
