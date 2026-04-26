using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Dtos
{
    public class WorkFlowActions : BaseDto<WorkFlowActions, Models.WorkFlowActions>, IFilteration<Models.WorkFlowActions>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        [JsonIgnore]
        public int? CreatedBy { get; set; }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }

        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string _referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }


        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public DateTime? ActivatedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string DeletedDateString { get; set; }
        public string ActivatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string StatusString { get; set; }

        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.WorkFlowActions> Filteration()
        {
            var filter = PredicateBuilder.New<Models.WorkFlowActions>(true);

            if (ReferenceId is not null)
                filter.And(u => u.ReferenceId == ReferenceId);

            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));

            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;

        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                  .NewConfig<WorkFlowActions, Models.WorkFlowActions>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsActive, src => false)
                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<WorkFlowActions, Models.WorkFlowActions>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.WorkFlowActions, WorkFlowActions>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير مفعل")

                .Config;

        }

    }
}
