using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Menu.Dtos
{

    public class ReferenceDto : BaseDto<ReferenceDto, Models.Reference>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        [JsonIgnore]
        public int? ReferencesMajorId { get; set; }
        public string referencesMajorId { set { ReferencesMajorId = Accessor.Set(value); } get { return Accessor.Get(ReferencesMajorId); } }


    }
}
