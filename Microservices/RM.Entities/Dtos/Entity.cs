
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.Entities.Dtos
{
    public class Entity:BaseDto<Entity,Models.Entity>
    {
        public Entity()
        {
            SubEntities = new List<Dtos.Entity>();         
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
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get<int?>(ParentId); } }
        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(TypeId); } }
      
        public string StatusStringAr { get; set; }
        public string StatusStringEn { get; set; }
        public string FormId { get; set; }

        public List<Dtos.Entity> SubEntities { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.Entity> EntitiesFilteration()
        {
            var filter = PredicateBuilder.New<Models.Entity>(true);

            filter.And(u => u.ParentId == null);

            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));
            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig(int? FormId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Entity, Dtos.Entity>()
                .Map(dest => dest.SubEntities, src => src.InverseParent != null ? src.InverseParent.Adapt<List<Dtos.Entity>>(Dtos.Entity.SelectConfig(FormId)) : new List<Dtos.Entity>())
                .Map(dest => dest.FormId, src => FormId.HasValue ? Accessor.Get<int?>(FormId) : string.Empty)
                .Map(dest => dest.StatusStringAr, src => src.IsActive.HasValue && src.IsActive == true ? "مفعل" : "غير مفعل")
                .Map(dest => dest.StatusStringEn, src => src.IsActive.HasValue && src.IsActive == true ? "Active" : "Deactivated")
                .Config;
        }

        public TypeAdapterConfig UpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.Entity, Models.Entity>()
                .IgnoreNullValues(true)

                .Config;
        }

        public TypeAdapterConfig AddConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.Entity, Models.Entity>()
                .IgnoreNullValues(true)

                .Map(dest => dest.IsActive, src => true)
                .Config;
        }
    }
   
}
