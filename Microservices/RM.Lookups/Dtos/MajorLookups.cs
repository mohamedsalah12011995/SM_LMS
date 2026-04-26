
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Lookups.Dtos
{
    public class MajorLookups : BaseDto<MajorLookups, Models.MajorLookup>
    {
        public MajorLookups()
        {
            SubLookups = new List<MajorLookups>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? TypeId { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }


        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }


        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(TypeId); } }

        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get<int?>(ParentId); } }


        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ReferenceNameAr { get; set; }
        public string ReferenceNameEn { get; set; }
        public List<Dtos.MajorLookups> SubLookups { get; set; }


        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.MajorLookup, MajorLookups>()
                .Map(dest => dest.ReferenceNameAr, src => src.Reference != null ? src.Reference.NameAr : null)
                .Map(dest => dest.ReferenceNameEn, src => src.Reference != null ? src.Reference.NameEn : null)
                .Map(dest => dest.SubLookups, src => src.InverseParent != null ? src.InverseParent.Where(c => c.IsDeleted != true).ToList().Adapt<List<Dtos.MajorLookups>>(SelectConfig()) : new List<MajorLookups>())


                    .Config;
        }

        public TypeAdapterConfig UpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<MajorLookups, Models.MajorLookup>().IgnoreNullValues(true)
                .Config;
        }

        public TypeAdapterConfig AddConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<MajorLookups, Models.MajorLookup>().IgnoreNullValues(true)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }
    }
}
