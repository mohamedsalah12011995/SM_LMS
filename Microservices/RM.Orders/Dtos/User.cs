
using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Orders.Dtos
{
    public class User
    {
        [JsonIgnore]
        public int? Id { get; set; }
        
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Order, User>()
                .Map(dest => dest.Id, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Id : 0)
                .Map(dest => dest.Name, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : null)
                .Map(dest => dest.Phone, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Phone : null)
                .Map(dest => dest.Email, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Email : null)

                    .Config;
        }
    }
}
