
using LinqKit;
using Mapster;
using RM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.MobileApplications.Dtos
{
    public class MobileApplications
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
        public int? ActivatedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string BriefeContentAr { get; set; }
        public string BriefeContentEn { get; set; }
        public string ApplicationSize { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        public DateTime? ActivatedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string DeletedDateString { get; set; }
        public string ActivatedDateString { get; set; }

        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string UserManualUrlAr { get; set; }
        public string UserManualUrlArBase64 { get; set; }
        public string UserManualUrlEn { get; set; }
        public string UserManualUrlEnBase64 { get; set; }
        public string AndroidUrl { get; set; }
        public string IosUrl { get; set; }
        public string ImageUrl { get; set; }
        public string ImageUrlBase64 { get; set; }
        public string UpdatedPerson { get; set; }
        public string CreatedPerson { get; set; }
        public string StatusString { get; set; }
        public List<QuestionsAnswers> ListOfQuestions { get; set; } 
        public List<Attachments> ListOfImage { get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.MobileApplication> Filteration()
        {
            var filter = PredicateBuilder.New<Models.MobileApplication>(true);

            if (ReferenceId.HasValue)
                filter.And(u => u.ReferenceId == ReferenceId);


            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(NameAr))
                filter.And(u => u.NameAr.Contains(NameAr));
            if (!string.IsNullOrEmpty(NameEn))
                filter.And(u => u.NameEn.Contains(NameEn));

            if (!string.IsNullOrEmpty(CreatedPerson))
                filter.And(u => u.CreatedByNavigation.Name.Contains(CreatedPerson));
            if (!string.IsNullOrEmpty(UpdatedPerson))
                filter.And(u => u.UpdatedByNavigation.Name.Contains(UpdatedPerson));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);

            return filter;

        }

        public static TypeAdapterConfig SelectConfig(string ImagesGetPath,string DocumentsGetPath,string ImagesSavePath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.MobileApplication, MobileApplications>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.CreatedPerson, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.UpdatedPerson, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.ImageUrl, src => ImagesGetPath + "/" + src.ImageUrl)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير فعال")
                .Map(dest => dest.UserManualUrlAr, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.UserManualUrlAr) ? DocumentsGetPath + "/" + src.UserManualUrlAr : null)
                .Map(dest => dest.UserManualUrlEn, src => !Strings.CheckStringNullOrEmptyOrWhiteSpaceOrZero(src.UserManualUrlEn) ? DocumentsGetPath + "/" + src.UserManualUrlEn : null)
                .Map(dest => dest.ListOfQuestions, src => src.QuestionsAnswers.Where(x => x.IsDeleted != true).ToList().Adapt<List<Dtos.QuestionsAnswers>>(Dtos.QuestionsAnswers.SelectConfig()))
                .Map(dest => dest.ListOfImage, src => src.Attachments.ToList().Adapt<List<Dtos.Attachments>>(Dtos.Attachments.SelectConfig(ImagesGetPath, ImagesSavePath)))

                    .Config;

        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<MobileApplications, Models.MobileApplication>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<MobileApplications, Models.MobileApplication>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)
                .Map(dest => dest.IsActive, src => false)
                .Config;
        }
    }
}
