

using DocumentFormat.OpenXml.Bibliography;
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.OpenData.Dtos
{
    public class OpenDataRequest:BaseDto<OpenDataRequest,Models.OpenDataRequest>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string Address { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Details { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        [JsonIgnore]
        public int? ModifiedBy { get; set; }
        public string modifiedBy { set { ModifiedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ModifiedBy); } }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public string ModifiedDateString { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public bool? IsDeleted { get; set; }

        public string Capcha { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.OpenDataRequest> Filteration()
        {
            var filter = PredicateBuilder.New<Models.OpenDataRequest>(true);

            filter.And(u => u.ReferenceId == ReferenceId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (ModifiedDate.HasValue)
                filter.And(u => u.ModifiedDate == null || u.ModifiedDate.Value.Date == ModifiedDate.Value.Date);

            if (!string.IsNullOrEmpty(Name))
                filter.And(u => u.Name.Contains(Name));

            if (!string.IsNullOrEmpty(Address))
                filter.And(u => u.Address.Contains(Address));

            if (!string.IsNullOrEmpty(Mobile))
                filter.And(u => u.Mobile.Contains(Mobile));

            if (!string.IsNullOrEmpty(Email))
                filter.And(u => u.Email.Contains(Email));

            if (!string.IsNullOrEmpty(Details))
                filter.And(u => u.Details.Contains(Details));

            filter.And(u => u.IsDeleted == false);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.OpenDataRequest, OpenDataRequest>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.ModifiedDateString, src => src.ModifiedDate.HasValue ? src.ModifiedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                    .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<OpenDataRequest, Models.OpenDataRequest>().IgnoreNullValues(true)
                .Map(dest => dest.ModifiedBy, src => userId)
                .Map(dest => dest.ModifiedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<OpenDataRequest, Models.OpenDataRequest>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)

                .Config;
        }
    }
}
