
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Orders.Dtos
{
    public class Type:BaseDto<Type,Models.MajorLookup>
    {

            [JsonIgnore]
            public int? Id { get; set; }

            public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
            public string NameAr { get; set; }
            public string NameEn { get; set; }


        
    }
}
