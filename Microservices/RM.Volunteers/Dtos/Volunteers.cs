
using DocumentFormat.OpenXml.Wordprocessing;
using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.Text.Json.Serialization;

namespace RM.Volunteers.Dtos
{
    public class Volunteers:BaseDto<Volunteers,Models.Volunteer>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? ModifiedBy { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        
        public string modifiedBy { set { ModifiedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ModifiedBy); } }
       
        public string CreatedPerson { get; set; }
        public string ModifiedPerson { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedDateString { get; set; }

        public string Name { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        [JsonIgnore]
        public int? QualificationId { get; set; }
        
        public string qualificationId { set { QualificationId = Accessor.Set(value); } get { return Accessor.Get<int?>(QualificationId); } }
        public string QualificationName { get; set; }
        [JsonIgnore]
        public int? DistrictId { get; set; }
        
        public string districtId { set { DistrictId = Accessor.Set(value); } get { return Accessor.Get<int?>(DistrictId); } }
        public string DistrictName { get; set; }
        [JsonIgnore]
        public int? AgeId { get; set; }
        
        public string ageId { set { AgeId = Accessor.Set(value); } get { return Accessor.Get<int?>(AgeId); } }
        public string AgeName { get; set;}
        [JsonIgnore]
        public int? GenderId { get; set; }
        
        public string genderId { set { GenderId = Accessor.Set(value); } get { return Accessor.Get<int?>(GenderId); } }


        [JsonIgnore]
        public int? VolunteerFieldId { get; set; }
        
        public string volunteerFieldId { set { VolunteerFieldId = Accessor.Set(value); } get { return Accessor.Get<int?>(VolunteerFieldId); } }
        public string VolunteerFieldName { get; set; }


        public string GenderName { get; set;}
        public string Email { get; set; }
        public string Description { get; set; }
        public DateTime? Birthday { get; set; }
        public string BirthdayString { get; set; }

        public string Capcha { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.Volunteer> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Volunteer>(true);

                filter.And(u => u.ReferenceId == ReferenceId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (ModifiedDate.HasValue)
                filter.And(u => u.ModifiedDate == null || u.ModifiedDate.Value.Date == ModifiedDate.Value.Date);

            if (!string.IsNullOrEmpty(Name))
                filter.And(u => u.Name.Contains(Name));
            if (!string.IsNullOrEmpty(UserId))
                filter.And(u => u.UserId.Contains(UserId));

            if (!string.IsNullOrEmpty(MobileNo))
                filter.And(u => u.MobileNo.Contains(MobileNo));
            if (!string.IsNullOrEmpty(Email))
                filter.And(u => u.Email.Contains(Email));
            if (!string.IsNullOrEmpty(Description))
                filter.And(u => u.Description.Contains(Description));

            if (AgeId.HasValue)
                filter.And(u => u.AgeId == AgeId);

            if (QualificationId.HasValue)
                filter.And(u => u.QualificationId == QualificationId);

            if (GenderId.HasValue)
                filter.And(u => u.GenderId == GenderId);

            if (DistrictId.HasValue)
                filter.And(u => u.DistrictId == DistrictId);

            filter.And(u => u.IsDeleted != true);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Volunteer, Volunteers>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.ModifiedDateString, src => src.ModifiedDate.HasValue ? src.ModifiedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.BirthdayString, src => src.Birthday.HasValue ? src.Birthday.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.QualificationName, src => src.Qualification != null ? src.Qualification.NameAr : string.Empty)
                .Map(dest => dest.AgeName, src => src.Age != null ? src.Age.NameAr : string.Empty)
                .Map(dest => dest.DistrictName, src => src.District != null ? src.District.NameAr : string.Empty)
                .Map(dest => dest.GenderName, src => src.Gender != null ? src.Gender.NameAr : string.Empty)
                .Map(dest => dest.VolunteerFieldName, src => src.VolunteerField != null ? src.VolunteerField.NameAr : string.Empty)

                    .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Volunteers, Models.Volunteer>().IgnoreNullValues(true)
                .Map(dest => dest.ModifiedBy, src => userId)
                .Map(dest => dest.ModifiedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Volunteers, Models.Volunteer>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }
    }
}
