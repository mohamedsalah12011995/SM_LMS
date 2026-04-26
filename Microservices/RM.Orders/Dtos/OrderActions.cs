
using Mapster;
using RM.Core.Helpers;
using System;
using System.Text.Json.Serialization;

namespace RM.Orders.Dtos
{
    public class OrderActions
    {
        [JsonIgnore]
        public int? Id { get; set; }
        
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        [JsonIgnore]
        public int? OrderId { get; set; }
        
        public string orderId { set { OrderId = Accessor.Set(value); } get { return Accessor.Get<int?>(OrderId); } }


        [JsonIgnore]
        public int? TypeId { get; set; }
        
        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(TypeId); } }
        public string ActionName { get; set; }

        [JsonIgnore]
        public int? CreatedBy { get; set; }
        
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string PersonCreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString { get; set; }

        public string Note { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.OrderActions, OrderActions>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.ActionName, src => src.Type != null ? src.Type.NameAr : string.Empty)

                    .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<OrderActions, Models.OrderActions>().IgnoreNullValues(true)

                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<OrderActions, Models.OrderActions>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.TypeId, src => (int)Enums.OrderActionType.New)
                .Config;

        }
    }
}
