
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Jobs.Dtos
{
    public class JobLookUp:BaseDto<JobLookUp,Models.JobLookUp>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string id { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }

        public string Name { get; set; }
        public string NameEn { get; set; }

        [JsonIgnore]
        public int? TypeId { get; set; }
        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(TypeId); } }
        public TypeAdapterConfig UpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<JobLookUp, Models.JobLookUp>().IgnoreNullValues(true)
                .Config;
        }

        public TypeAdapterConfig AddConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<JobLookUp, Models.JobLookUp>().IgnoreNullValues(true)
                .Config;
        }
    }
}
