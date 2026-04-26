using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Documents.Dtos
{
    public class GuideDocument
    {
        [JsonIgnore]
        public int? _typeId { get; set; }
        [JsonIgnore]
        public int? _entityId { get; set; }

        [JsonIgnore]
        public int? _referenceId { get; set; }
        public string TypeId { set { _typeId = Accessor.Set(value); } get { return Accessor.Get<int?>(this._typeId); } }
        public string ReferenceId { set { _referenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(this._referenceId); } }
        public string EntityId { set { _entityId = Accessor.Set(value); } get { return Accessor.Get<int?>(this._entityId); } }

    }
}
