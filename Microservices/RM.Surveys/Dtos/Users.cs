using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Surveys.Dtos
{
    public class Users 
    {

        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }


        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }


        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string IdCardNumber { get; set; }
        public DateTime? BirthDate { get; set; }



        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.User, Users>().IgnoreNullValues(true)
                .Config;
        }
    }

}
