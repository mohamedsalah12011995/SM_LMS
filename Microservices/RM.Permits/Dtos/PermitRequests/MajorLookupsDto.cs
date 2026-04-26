using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class MajorLookupsDto : BaseDto<MajorLookupsDto, Models.MajorLookup>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
