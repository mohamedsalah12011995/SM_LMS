using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Records
{
    public record EngineActionJobRoleEmailDataRecord
    {
        [JsonIgnore]
        public int? _engineActionJobRoleId { get; set; }
        public string EngineActionJobRoleId { set { _engineActionJobRoleId = Accessor.Set(value); } get { return Accessor.Get(_engineActionJobRoleId); } }
        public bool? IsSendEmail { get; set; }
        public string EmailBody { get; set; }
        public string SendTo { get; set; }
        public string Subject { get; set; }

    }
}
