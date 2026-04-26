using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Dtos
{
    public class EngineActions
    {
        public EngineActions()
        {
            EnginesActionsJobRoles = new List<EnginesActionsJobRole>();
        }

        [JsonIgnore]
        public int? _id { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }

        [JsonIgnore]
        public int? _engineId { get; set; }
        public string EngineId { set { _engineId = Accessor.Set(value); } get { return Accessor.Get(_engineId); } }

        [JsonIgnore]
        public int? _actionId { get; set; }
        public string ActionId { set { _actionId = Accessor.Set(value); } get { return Accessor.Get(_actionId); } }

        public bool? ISDeleted { get; set; }
        public List<EnginesActionsJobRole> EnginesActionsJobRoles { get; set; }

    }
}
