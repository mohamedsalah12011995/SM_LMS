
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Innovations.Dtos
{
    public class Users
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}
