
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using System.Text.Json.Serialization;

namespace RM.References.Dtos
{
    public class ReferencesMajor : BaseDto<ReferencesMajor, Models.ReferencesMajor>, IFilteration<Models.ReferencesMajor>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? CreatedDate { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTime? UpdatedDate { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? IsDeleted { get; set; }
        [JsonIgnore]
        public int? LoginWayId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string loginWayId { set { LoginWayId = Accessor.Set(value); } get { return Accessor.Get(LoginWayId); } }
        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }
        public string CreatedPerson { get; set; }
        public string UpdatedPerson { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.ReferencesMajor> Filteration()
        {
            var filter = PredicateBuilder.New<Models.ReferencesMajor>(true);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));

            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));

            if (!string.IsNullOrEmpty(CreatedPerson))
                filter.And(u => u.CreatedByNavigation.Name.Contains(CreatedPerson));

            if (!string.IsNullOrEmpty(UpdatedPerson))
                filter.And(u => u.UpdatedByNavigation.Name.Contains(UpdatedPerson));

            filter.And(u => u.IsDeleted != true);

            return filter;

        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.ReferencesMajor, ReferencesMajor>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.CreatedPerson, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.UpdatedPerson, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Config;

        }



        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<ReferencesMajor, Models.ReferencesMajor>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                  .NewConfig<ReferencesMajor, Models.ReferencesMajor>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }

        public TypeAdapterConfig DeleteConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<References, Models.Reference>().IgnoreNullValues(true)
                .Map(dest => dest.DeletedBy, src => userId)
                .Map(dest => dest.DeletedDate, src => DateTime.Now)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }
    }
}
