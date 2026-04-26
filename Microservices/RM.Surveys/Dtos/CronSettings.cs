

using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Surveys.Dtos
{
    public class CronSettings
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        [JsonIgnore]
        public int? EntityId { get; set; }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }

        [JsonIgnore]
        public int? SubEntityId { get; set; }
        public string subEntityId { set { SubEntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(SubEntityId); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        //  [JsonIgnore]
        public int? CronTypeId { get; set; }
      //  public string cronTypeId { set { CronTypeId = Accessor.Set(value); } get { return Accessor.Get(CronTypeId); } }


        [JsonIgnore]
        public int? SurveyId { get; set; }
        public string surveyId { set { SurveyId = Accessor.Set(value); } get { return Accessor.Get(SurveyId); } }


        public List<string> Emails { get; set; } = new List<string>();
        public List<Users> Users { get; set; } = new List<Users>();

        public bool? IsActive { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.CronSettings, CronSettings>()
                .Map(dest => dest.Emails, src => Strings.ConvertStringToList(src.Emails, "$"))
                .Config;
        }

        public TypeAdapterConfig AddConfig(int SurveyId)
        {
            return new TypeAdapterConfig()
                .NewConfig<CronSettings, Models.CronSettings>().IgnoreNullValues(true)
                .Map(dest => dest.SurveyId, src => SurveyId)
                .Map(dest => dest.Emails, src => Strings.ConvertListToString(Emails, "$"))
             //   .Map(dest => dest.EntityId, src => (int) Enums.Entities.Survey)
                .Config;
        }

        public TypeAdapterConfig UpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<CronSettings, Models.CronSettings>().IgnoreNullValues(true)
                .Map(dest => dest.Emails, src => Strings.ConvertListToString(Emails, "$"))
                .Config;
        }

    }
}
