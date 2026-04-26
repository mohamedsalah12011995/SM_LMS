using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.ContactUs.Dtos
{
    public class UserDto
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        [JsonIgnore]
        public int? JobRole { get; set; }
        public string jobRole { set { JobRole = Accessor.Set(value); } get { return Accessor.Get(JobRole); } }

    }
}