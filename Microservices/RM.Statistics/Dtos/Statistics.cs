using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Statistics.Dtos
{
    public class Statistics
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]

        public int? ItemId { get; set; }
        [JsonIgnore]

        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string itemId { set { ItemId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemId); } }

        public int ViewsCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;
        public int RatingCount { get; set; } = 0;
        public int HelpfulCount { get; set; } = 0;
        public int NotHelpfulCount { get; set; } = 0;
        public bool IsHelpful { get; set; } = true;
        public string ItemUrl { get; set; }
        public double RatingValue { get; set; } = 0;
        public DateTime? LatestUpdate { get; set; }
        public string LatestUpdateString { get; set; }
        public bool IsUserReview { get; set; }
        public int? StatisticsType { get; set; }
        public string EntityName { get; set; }
        public string EntityNameEn { get; set; }
        public string ArticleNameAr { get; set; }
        public string ArticleNameEn { get; set; }


        public List<Entity> Entities = new List<Entity>();


        [JsonIgnore]
        public int? ItemEntityId { get; set; }
        public string itemEntityId { set { ItemEntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemEntityId); } }



        public DateTime? CreatedDate { get; set; }
        public int? Year { get; set; }
        public int? Quarter { get; set; }


        public List<string> Emails { get; set; } = new List<string>();

        public string Subject { get; set; }
        public string Body { get; set; }
        public string FileName { get; set; }
    }
    public class SetStatisticsRequest
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }

        [JsonIgnore]
        public int? ItemId { get; set; }
        public string itemId { set { ItemId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemId); } }

        public string ItemUrl { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }

        public int? StatisticsType { get; set; }
        public bool IsHelpful { get; set; } = true;
        public int? value { get; set; }
    }

    public class Entity
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
    }

    public class InteractionStatisticsTypes
    {
        public int? Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
