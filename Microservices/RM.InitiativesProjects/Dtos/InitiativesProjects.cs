using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.InitiativesProjects.Dtos
{
    public class InitiativesProjects:BaseDto<InitiativesProjects,Models.InitiativesProject>
    {
        public InitiativesProjects()
        {
            ListOfBeneficiaries = new List<Beneficiaries>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? TypeId { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? BeneficiaryId { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(TypeId); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string beneficiaryId { set { BeneficiaryId = Accessor.Set(value); } get { return Accessor.Get<int?>(BeneficiaryId); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public int? CommentsNumber { get; set; }
        public DateTime? InitiativeDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string ContentAr { get; set; }
        public string ContentEn { get; set; }
        public string GoalsAr { get; set; }
        public string GoalsEn { get; set; }
        public string LocationAr { get; set; }
        public string LocationEn { get; set; }
        public string TypeTextAr { get; set; }
        public string TypeTextEn { get; set; }
        public string StatusString { get; set; }
        public List<Beneficiaries> ListOfBeneficiaries { get; set; }
        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string InitiativeDateString { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<InitiativesProject> Filteration()
        {
            var filter = PredicateBuilder.New<InitiativesProject>(true);

            filter.And(u => u.ReferenceId == ReferenceId);


            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);
            
            if (InitiativeDate.HasValue)
                filter.And(u => u.InitiativeDate == null || u.InitiativeDate.Value.Date == InitiativeDate.Value.Date);

            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));
            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));


            if (TypeId.HasValue)
                filter.And(u => u.TypeId == TypeId);
            
            if (BeneficiaryId.HasValue)
                filter.And(u => u.InitiativesProjectsBeneficiaries.Any(u => u.BeneficiaryId == BeneficiaryId));
            
            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            if (ListOfBeneficiaries != null && ListOfBeneficiaries.Any())
            {
                foreach (var Benef in ListOfBeneficiaries)
                {
                    int? tempId = Benef.Id; // To avoid closure issue
                    filter.Or(u => u.InitiativesProjectsBeneficiaries.Any(u => u.BeneficiaryId == tempId));
                    filter.Or(u => u.InitiativesProjectsBeneficiaries.Any(u => u.InitiativesProjectId == u.Id));
                }
            }

            filter.And(u => u.IsDeleted != true);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig(bool selectList, int? CommentsCount=null)
        {
            return new TypeAdapterConfig()
                .NewConfig<InitiativesProject, InitiativesProjects>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير مفعل")
                .Map(dest => dest.CommentsNumber, src => CommentsCount)
                .Map(dest => dest.ListOfBeneficiaries, src => src.InitiativesProjectsBeneficiaries!=null && !selectList ? src.InitiativesProjectsBeneficiaries.Where(x=>x.Beneficiary.IsDeleted != true)
                .Select(v => new Beneficiaries()
                {
                    Id = v.Id,
                    NameAr = v.Beneficiary.NameAr,
                    NameEn = v.Beneficiary.NameEn,

                }).ToList() : null)
                    .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<InitiativesProjects, InitiativesProject>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<InitiativesProjects, InitiativesProject>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsActive, src => false)
                .Map(dest => dest.IsDeleted, src => false)
                .Config;
        }

    }
}
