
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.ContactUs.Dtos
{
    public class LookupItem:BaseDto<LookupItem,Reference>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string? id { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string?  NameAr { get; set; }
        public string?  NameEn{ get; set; }


    }
}
