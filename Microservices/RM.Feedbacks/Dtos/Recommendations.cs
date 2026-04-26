

using LinqKit;
using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Feedbacks.Dtos
{
    public class Recommendations
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public string ContentAr { get; set; }
        public string ContentEn { get; set; }

        [JsonIgnore]
        public int? EntityId { get; set; }

        [JsonIgnore]
        public int? ReferenceId { get; set; }

        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }

        [JsonIgnore]
        public int? UpdatedBy { get; set; }

        public string referenceID { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }

        public string createdById { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public string deletedById { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get(DeletedBy); } }
        public string updatedById { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.Recommendations> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Recommendations>(true);

            if (ReferenceId != null)
                filter.And(u => u.ReferenceId == ReferenceId);


            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));
            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));


            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Recommendations, Recommendations>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? UserId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Recommendations, Models.Recommendations>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => UserId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? UserId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Recommendations, Models.Recommendations>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => UserId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }


    }
}
