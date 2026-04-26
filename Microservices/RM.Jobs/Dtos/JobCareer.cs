
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.Jobs.Dtos
{
    public class JobCareer:BaseDto<JobCareer,Models.JobCareer>
    {
        public JobCareer()
        {
            ListOfSkills = new List<JobLookUp>();
            ListOfSpecifications=new List<JobLookUp> ();
            ListOfTags=new List<JobLookUp> ();
        }
        [JsonIgnore]
        public int? Id { get; set; }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string JobLocationAr { get; set; }
        public string JobLocationEn { get; set; }
        public string JobCondationsAr { get; set; }
        public string JobCondationsEn { get; set; }
        public int? Type { get; set; }
        public string Tags { get; set; }
        public string Skills { get; set; }
        public string Specifications { get; set; }
        public string SpecificationsEn { get; set; }
        public List<JobLookUp> ListOfTags { get; set; }
        public List<JobLookUp> ListOfSkills { get; set; }
        public List<JobLookUp> ListOfSpecifications { get; set; }
        [JsonIgnore]
        public int? QualificationId { get; set; }
    
        public string qualificationId { set { QualificationId = Accessor.Set(value); } get { return Accessor.Get<int?>(QualificationId); } }
        [JsonIgnore]
        public int? JobAdvertisementId { get; set; }
        public string jobAdvertisementId { set { JobAdvertisementId = Accessor.Set(value); } get { return Accessor.Get<int?>(JobAdvertisementId); } }
        public int? MaxLimit { get; set; }
        public string StatusString { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string QualificationNameAr { get; set; }
        public string QualificationNameEn { get; set; }
        public bool? IsNoticeBeneficiaries { get; set; }

        public Qualification Qualification { get; set; }
        public JobAdvertisement JobAdvertisement { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.JobCareer, JobCareer>()
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير فعال")
                .Map(dest => dest.QualificationNameAr, src => src.Qualification != null ? src.Qualification.NameAr : string.Empty)
                .Map(dest => dest.QualificationNameEn, src => src.Qualification != null ? src.Qualification.NameEn : string.Empty)

                .Config;
        }

        public TypeAdapterConfig UpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<JobCareer, Models.JobCareer>().IgnoreNullValues(true)
                .Config;
        }

        public TypeAdapterConfig AddConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<JobCareer, Models.JobCareer>().IgnoreNullValues(true)
                .Config;
        }
    }
}
