
using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Users.Dtos
{
    public class ReferencesTree
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferencesMajorId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referencesMajorId { set { ReferencesMajorId = Accessor.Set(value); } get { return Accessor.Get(ReferencesMajorId); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        public string Label { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }


        [JsonIgnore]
        public int? ParentId { get; set; }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get(ParentId); } }
        public IEnumerable<ReferencesTree> Children { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Reference, ReferencesTree>()
                .Map(dest => dest.Label, src => src.NameAr)
                .Config;

        }

    }
}
