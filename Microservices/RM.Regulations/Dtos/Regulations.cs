using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.Regulations.Dtos
{
    public class Regulations : BaseDto<Regulations, Models.TermsAndRegulation>, IFilteration<Models.TermsAndRegulation>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get<int?>(ParentId); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Url { get; set; }
        public string ParentNameAr { get; set; }
        public string ParentNameEn { get; set; }
        public string StatusString { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string PersonCreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string ActivatedDateString { get; set; }
        public string DeletedDateString { get; set; }

        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsFinalRoot { get; set; }
        public List<Regulations> ListOfRegulations { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<TermsAndRegulation> Filteration()
        {
            var filter = PredicateBuilder.New<Models.TermsAndRegulation>(true);

            if (ReferenceId.HasValue)
                filter.And(u => u.ReferenceId == ReferenceId);

            if (ParentId.HasValue)
                filter.And(u => u.ParentId == ParentId);

            if (EntityId.HasValue)
                filter.And(u => u.EntityId == EntityId);

            if (!string.IsNullOrEmpty(PersonCreatedBy))
                filter.And(u => u.CreatedByNavigation.Name.Contains(PersonCreatedBy));

            if (!string.IsNullOrEmpty(PersonUpdatedBy))
                filter.And(u => u.UpdatedByNavigation.Name.Contains(PersonUpdatedBy));

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));

            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);
            filter.And(u => u.IsFinalRoot == true);
            return filter;






        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.TermsAndRegulation, Regulations>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.ListOfRegulations, src => src.InverseParent.Any() ? src.InverseParent.Where(v => v.IsDeleted != true).Adapt<List<Dtos.Regulations>>(Dtos.Regulations.SelectConfig()) : new List<Dtos.Regulations>())
                .Map(dest => dest.ParentNameAr, src => src.Parent != null ? src.Parent.NameAr : null)
                .Map(dest => dest.ParentNameEn, src => src.Parent != null ? src.Parent.NameEn : null)
                .Map(dest => dest.StatusString, src =>  src.IsActive == true ? "فعال" : "غير فعال")



                .Config;

        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.Regulations, Models.TermsAndRegulation>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)

                .Config;
        }



        public TypeAdapterConfig AddConfig(int? userId, bool isCategory)
        {
            return new TypeAdapterConfig()
                  .NewConfig<Dtos.Regulations, Models.TermsAndRegulation>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.IsFinalRoot, src => isCategory ? false : true)
                .Config;
        }


    }
}
