
using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Innovations.Dtos
{
    public class Statistics
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }


        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }

        [JsonIgnore]
        public int? ItemEntityId { get; set; }
        public string itemEntityId { set { ItemEntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemEntityId); } }



        public DateTime? CreatedDate { get; set; }
        public int? Year { get; set; }
        public int? Quarter { get; set; }

        [JsonIgnore]
        public int? ItemId { get; set; }
        public string itemId { set { ItemId = Accessor.Set(value); } get { return Accessor.Get<int?>(ItemId); } }


        public List<string> Emails { get; set; } = new List<string>();

        public string Subject { get; set; }
        public string Body { get; set; }
        public string FileName { get; set; }

    }
}
