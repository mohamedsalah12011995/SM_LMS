
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Entities.Dtos
{
    public class Lookup:BaseDto<Lookup,Models.Form>
    {
        [JsonIgnore]
        public int? Id { get; set; }
       
        public string? NameAr { get; set; }
        public string? NameEn { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

    }
}
