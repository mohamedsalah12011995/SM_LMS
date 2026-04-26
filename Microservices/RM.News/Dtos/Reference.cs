using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.News.Dtos
{
    public class Reference : BaseDto<Reference, Models.Reference>
    {
        [JsonIgnore]
        internal int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }

    }
}
