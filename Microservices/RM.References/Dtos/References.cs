using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using System.Text.Json.Serialization;

namespace RM.References.Dtos
{
    public class References : BaseDto<References, Models.Reference>, IFilteration<Models.Reference>
    {
        public References()
        {
            SearchInReferences = new List<int>();
        }
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
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string CreatedPerson { get; set; }
        public string UpdatedPerson { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsAgency { get; set; }
        public List<int> SearchInReferences { get; set; }

        public ReferenceContent Content { get; set; }
        public List<JobRole> ReferenceJobRole { get; set; }
        public List<ChildrenReference> Childrens { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get(ParentId); } }
        public bool? HasContent { get; set; }
        public bool? IsPortal { get; set; }
        public string Url { get; set; }

        public ExpressionStarter<Models.Reference> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Reference>(true);

            if (SearchInReferences != null && SearchInReferences.Count > 0)
                filter.And(u => SearchInReferences.Contains(u.ReferencesMajorId.Value));

            if (HasContent.HasValue)
                filter.And(u => u.HasContent == true);

            if (ReferencesMajorId is not null)
                filter.And(u => u.ReferencesMajorId == ReferencesMajorId);

            if (ParentId is not null)
                filter.And(u => u.ParentId == ParentId);

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

        public static TypeAdapterConfig SelectConfig(string referenceImagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Reference, References>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.CreatedPerson, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.UpdatedPerson, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.Content, src => src.ReferenceContents != null ? src.ReferenceContents.FirstOrDefault().Adapt<ReferenceContent>(ReferenceContent.GetCustomMapping(referenceImagesGetPath)) : new ReferenceContent
                {
                    EntityId = src.ReferencesMajorId == (int)Enums.ReferenceMajor.Departments ? (int)Enums.Entities.Departments : (int)Enums.Entities.Municipalities,
                    ReferenceId = src.Id
                })
                .Map(dest => dest.Childrens, src => src.InverseParent != null ? src.InverseParent.Adapt<List<ChildrenReference>>() : new List<ChildrenReference>())
                .Map(dest => dest.IsAgency, src => src.ReferencesMajorId == (int)Enums.ReferenceMajor.Agencies ? true : false)
                .Config;

        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<References, Models.Reference>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
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

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                  .NewConfig<References, Models.Reference>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }



    }
    public class ChildrenReference
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferencesMajorId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referencesMajorId { set { ReferencesMajorId = Accessor.Set(value); } get { return Accessor.Get(ReferencesMajorId); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }


    public class ReferencesTree : BaseDto<ReferencesTree, Models.Reference>
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
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }
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
