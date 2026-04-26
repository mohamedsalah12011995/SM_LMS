
using LinqKit;
using Mapster;
using RM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.Jobs.Dtos
{
    public class JobAdvertisement
    {
        public JobAdvertisement()
        {
            jobCareers = new List<JobCareer>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
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
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string Code { get; set; }
        public int? Type { get; set; }
        public string StatusString { get; set; }
        public string StatusStringEn { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public DateTime? ActivatedDate { get; set; }
        public bool? IsContinuing { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string StartDateString { get; set; }
        public string EndDateString { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string PersonCreatedBy { get; set; }
        public List<JobCareer>  jobCareers { get; set; }

        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.JobAdvertisement> Filteration()
        {
            var filter = PredicateBuilder.New<Models.JobAdvertisement>(true);

            if (ReferenceId.HasValue)
                filter.And(u => u.ReferenceId == ReferenceId);

            if (Type.HasValue)
                filter.And(u => u.Type == Type);

            if (IsContinuing == true)
                filter.And(u => u.EndDate.Date >= DateTime.Now.Date && u.StartDate.Date <= DateTime.Now.Date);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (StartDate.HasValue)
                filter.And(u => u.StartDate.Date == StartDate.Value.Date);

            if (EndDate.HasValue)
                filter.And(u => u.EndDate.Date == EndDate.Value.Date);

            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));

            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;

        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.JobAdvertisement, JobAdvertisement>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)                               
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StartDateString, src =>  src.StartDate.ToString("yyyy-MM-dd"))
                .Map(dest => dest.EndDateString, src =>  src.EndDate.ToString("yyyy-MM-dd"))
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير فعال" )
                .Map(dest => dest.StatusStringEn, src => src.IsActive == true ? "Active" : "Inactive")
                .Map(dest => dest.jobCareers, src => src.JobCareers != null ? src.JobCareers.Where(x=>x.IsDeleted != true).ToList().Adapt<List<Dtos.JobCareer>>(Dtos.JobCareer.SelectConfig()):new List<JobCareer>())

                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<JobAdvertisement, Models.JobAdvertisement>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<JobAdvertisement, Models.JobAdvertisement>().IgnoreNullValues(true)
                .Map(dest => dest.Code, src => DateTime.Now.ToString("yyyyMMdd") + Strings.RandomDigits(4).ToString())
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }


    }
}
