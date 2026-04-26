using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class WorkFlowLookup : BaseDto<WorkFlowLookup, Models.Engine>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }

        public List<ActionsLookup> Actions { get; set; } = new List<ActionsLookup>();



    }

    public class ActionsLookup : BaseDto<ActionsLookup, Models.EngineActionJobRole>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        [JsonIgnore]
        public int? EngineActionJobRoleId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string engineActionJobRoleId { set { EngineActionJobRoleId = Accessor.Set(value); } get { return Accessor.Get(EngineActionJobRoleId); } }

        public override void AddCustomMappings()
        {
            SetCustomMappings();


            SetCustomMappingsInverse()

                .Map(dest => dest.Id, src => src.ActionId)
                .Map(dest => dest.EngineActionJobRoleId, src => src.Id)
                .Map(dest => dest.NameAr, src => $" {src.ActionNavigation.NameAr} - {(src.JobRole != null ? src.JobRole.NameAr : string.Empty)}")
                .Map(dest => dest.NameEn, src => $" {src.ActionNavigation.NameEn} - {(src.JobRole != null ? src.JobRole.NameEn : string.Empty)}");
        }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.EngineActionJobRole, ActionsLookup>()

               .Map(dest => dest.Id, src => src.ActionId)
                .Map(dest => dest.EngineActionJobRoleId, src => src.Id)
                .Map(dest => dest.NameAr, src => $" {src.ActionNavigation.NameAr} - {(src.JobRole != null ? src.JobRole.NameAr : string.Empty)}")
                .Map(dest => dest.NameEn, src => $" {src.ActionNavigation.NameEn} - {(src.JobRole != null ? src.JobRole.NameEn : string.Empty)}")

                .Config;

        }


    }

    public class Reference
    {
        [JsonIgnore]
        public int? _id { get; set; }
        public string Id { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
