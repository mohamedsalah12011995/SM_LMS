using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Comments.Dtos
{
    public class EntitiesItem
    {
        public EntitiesItem()
        {
            UserEntities = new List<EntitiesItem>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? TypeId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? ReferencesMajorId { get; set; }
        public string? ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string? typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(TypeId); } }
        public string? referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string? referencesMajorId { set { ReferencesMajorId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferencesMajorId); } }
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }
        public List<EntitiesItem> UserEntities { get; set; }
    }
}
