using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using System.Text.Json.Serialization;

namespace RM.Permits.Dtos
{
    public class Project : BaseDto<Project, Models.Project>, IFilteration<Models.Project>
    {
        public Project()
        {
            FlowStepperProject = new List<FlowStepperProject>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        [JsonIgnore]
        public int? EntityId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DetailsAr { get; set; }
        public string DetailsEn { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }

        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string StatusString { get; set; }
        public string StatusStringEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string PersonCreatedBy { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public List<FlowStepperProject> FlowStepperProject { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }
        public bool IsDisabeldSteps { get; set; }

        public ExpressionStarter<Models.Project> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Project>(true);
            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));

            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));

            if (!string.IsNullOrEmpty(DetailsAr))
                filter.And(u => u.DetailsAr.Contains(DetailsAr));

            if (!string.IsNullOrEmpty(DetailsEn))
                filter.And(u => u.DetailsEn.Contains(DetailsEn));

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }
        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Project, Project>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير فعال")
                .Map(dest => dest.StatusStringEn, src => src.IsActive == true ? "Active" : "Deactive")

                 .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Project, Models.Project>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Project, Models.Project>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }




    }
}
