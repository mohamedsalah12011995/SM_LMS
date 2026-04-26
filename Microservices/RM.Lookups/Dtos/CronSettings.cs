

using DocumentFormat.OpenXml.Wordprocessing;
using LinqKit;
using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Lookups.Dtos
{
    public class CronSettings
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

      //  [JsonIgnore]
        public int? CronTypeId { get; set; }
     //   public string cronTypeId { set { CronTypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(CronTypeId); } }

        public List<string> Emails { get; set; } = new List<string>();
        public List<Users> Users { get; set; } = new List<Users>();

        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }

        [JsonIgnore]
        public int? SubEntityId { get; set; }
        public string subEntityId { set { SubEntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(SubEntityId); } }

        [JsonIgnore]
        public int? SurveyId { get; set; }
        public string surveyId { set { SurveyId = Accessor.Set(value); } get { return Accessor.Get<int?>(SurveyId); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        public bool? IsActive { get; set; }

        [JsonIgnore]
        public int? CreatedBy { get; set; }

        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        public string createdById { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public string updatedById { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }

        public string CronTypeNameAr {  get; set; }
        public string CronTypeNameEn {  get; set; }
        public string StatusStringAr {  get; set; }
        public string StatusStringEn {  get; set; }
        public string SearchWord {  get; set; }
        public ApplicationOperation.Pagination Pagination { get; set; }


        public ExpressionStarter<Models.CronSettings> Filteration()
        {
            var filter = PredicateBuilder.New<Models.CronSettings>(true);

            if (ReferenceId != null)
                filter.And(u => u.ReferenceId == ReferenceId);

           // if (EntityId != null)
                filter.And(u => u.EntityId == EntityId);

          // if (SubEntityId != null)
                filter.And(u => u.SubEntityId == SubEntityId);

            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            return filter;
        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.CronSettings, CronSettings>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.CronTypeNameAr, src => Helpers.CronName(src.CronTypeId,"ar"))
                .Map(dest => dest.CronTypeNameEn, src => Helpers.CronName(src.CronTypeId, "en"))
                .Map(dest => dest.Emails, src => Strings.ConvertStringToList(src.Emails, "$"))
                .Map(dest => dest.StatusStringAr, src => src.IsActive == true ? "مفعل" : "غير مفعل")
                .Map(dest => dest.StatusStringEn, src => src.IsActive == true ? "Active" : "Not Active")

                .Config;
        }

        public TypeAdapterConfig AddConfig(int? UserId)
        {
            return new TypeAdapterConfig()
                .NewConfig<CronSettings, Models.CronSettings>().IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => UserId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.Emails, src => Strings.ConvertListToString(Emails, "$"))

                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? UserId)
        {
            return new TypeAdapterConfig()
                .NewConfig<CronSettings, Models.CronSettings>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => UserId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.Emails, src => Strings.ConvertListToString(Emails, "$"))

                .Config;
        }

    }
}
