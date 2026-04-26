using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using System.Text.Json.Serialization;

namespace RM.References.Dtos
{
    public class Entity : BaseDto<Entity, Models.Entity>, IFilteration<Models.Entity>
    {
        public Entity()
        {
            SubEntities = new List<Entity>();

        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        [JsonIgnore]
        public int? TypeId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string FrontIdentity { get; set; }
        public string BackendIdentity { get; set; }
        public string CmsIdentity { get; set; }
        public bool? Searchable { get; set; }
        public bool? IsActive { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }



        public bool? ShowMenuIdentifier { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceID { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string parentID { set { ParentId = Accessor.Set(value); } get { return Accessor.Get(ParentId); } }
        public string typeID { set { TypeId = Accessor.Set(value); } get { return Accessor.Get(TypeId); } }

        public string StatusStringAr { get; set; }
        public string StatusStringEn { get; set; }
        public string FormId { get; set; }

        public List<Entity> SubEntities { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.Entity> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Entity>(true);
            if (ParentId == null)
                filter.And(u => u.ParentId == null);

            if (ParentId == null && !string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));

            if (ParentId == null && !string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            return filter;
        }




        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Entity, Entity>()
                .Map(dest => dest.StatusStringAr, src => src.IsActive.HasValue && src.IsActive == true ? "مفعل" : "غير مفعل")
                .Map(dest => dest.StatusStringEn, src => src.IsActive.HasValue && src.IsActive == true ? "Active" : "Deactivated")
                .Map(dest => dest.SubEntities, src => src.InverseParent != null ? src.InverseParent.Adapt<List<Entity>>() : new List<Entity>())
                .Config;
        }

    }
}
