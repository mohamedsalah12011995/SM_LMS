
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.Text.Json.Serialization;

namespace RM.Volunteers.Dtos
{
    public class VolunteersLookups:BaseDto<VolunteersLookups,Models.MajorLookup>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        [JsonIgnore]
        public int? TypeId { get; set; }
        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(TypeId); } }

    }
}
