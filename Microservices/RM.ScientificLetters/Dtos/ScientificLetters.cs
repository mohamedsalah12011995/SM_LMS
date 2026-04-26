
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.Text.Json.Serialization;

namespace RM.ScientificLetters.Dtos
{
    public class ScientificLetters:BaseDto<ScientificLetters,Models.ScientificLetters>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public string ResearcherNameAr { get; set; }
        public string ResearcherNameEn { get; set; }
        public string ResearchPlaceAr { get; set; }
        public string ResearchPlaceEn { get; set; }
        public string FileName { get; set; }

        public string DetailsAr { get; set; }
        public string DetailsEn { get; set; }

        public DateTime? PublishedOn { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public DateTime? ActivatedDate { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public string CreatedDateString { get; set; }

        public string PublishedOnString { get; set; }
        public string UpdatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string DeletedDateString { get; set; }
        public string ActivatedDateString { get; set; }
        public string PersonDeletedBy { get; set; }
        public string PersonActivatedBy { get; set; }
        public string StatusString { get; set; }
        public string Url { get; set; }


        [JsonIgnore]
        public int? ReferenceId { get; set; }
        
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        [JsonIgnore]
        public int? EntityId { get; set; }
        
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }

        [JsonIgnore]
        public int? DegreeId { get; set; }
        
        public string degreeId { set { DegreeId = Accessor.Set(value); } get { return Accessor.Get<int?>(DegreeId); } }
        public string DegreeNameAr { get; set; }
        public string DegreeNameEn { get; set; }



        [JsonIgnore]
        public int? CreatedBy { get; set; }
        
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }

        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }

        [JsonIgnore]
        public int? DeletedBy { get; set; }
        
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }

        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }

        public string FileBase64 { get; set; }
        public string FileType { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.ScientificLetters> Filteration()
        {
            var filter = PredicateBuilder.New<Models.ScientificLetters>(true);

            if (ReferenceId.HasValue)
                filter.And(u => u.ReferenceId == ReferenceId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));
            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));

            if (!string.IsNullOrEmpty(ResearcherNameAr))
                filter.And(u => u.ResearcherNameAr.Contains(ResearcherNameAr));
            if (!string.IsNullOrEmpty(ResearcherNameEn))
                filter.And(u => u.ResearcherNameEn.Contains(ResearcherNameEn));

            if (!string.IsNullOrEmpty(ResearchPlaceAr))
                filter.And(u => u.ResearchPlaceAr.Contains(ResearchPlaceAr));
            if (!string.IsNullOrEmpty(ResearchPlaceEn))
                filter.And(u => u.ResearchPlaceEn.Contains(ResearchPlaceEn));

            if(IsActive.HasValue)
            filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig(string fileGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.ScientificLetters, ScientificLetters>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.DeletedDateString, src => src.DeletedDate.HasValue ? src.DeletedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.ActivatedDateString, src => src.ActivatedDate.HasValue ? src.ActivatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonDeletedBy, src => src.DeletedByNavigation != null ? src.DeletedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonActivatedBy, src => src.ActivatedByNavigation != null ? src.ActivatedByNavigation.Name : string.Empty)
                .Map(dest => dest.Url, src => fileGetPath + "/" + src.FileName)
                .Map(dest => dest.DegreeNameAr, src => src.Degree != null ? src.Degree.NameAr : null)
                .Map(dest => dest.DegreeNameEn, src => src.Degree != null ? src.Degree.NameEn : null)
                .Map(dest => dest.PublishedOnString, src => src.PublishedOn.HasValue ? src.PublishedOn.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.ActivatedDateString, src => src.IsActive == true ? "فعال" : "غير فعال")
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير فعال")

                    .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<ScientificLetters, Models.ScientificLetters>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<ScientificLetters, Models.ScientificLetters>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }
    }
}
