using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Dtos
{
    public class EnginesActionsJobRole : BaseDto<EnginesActionsJobRole, Models.EngineActionJobRole>
    {

        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        [JsonIgnore]
        public int? EngineId { get; set; }
        public string engineId { set { EngineId = Accessor.Set(value); } get { return Accessor.Get(EngineId); } }

        [JsonIgnore]
        public int? ActionId { get; set; }
        public string actionId { set { ActionId = Accessor.Set(value); } get { return Accessor.Get(ActionId); } }

        [JsonIgnore]
        public int? ReturnStep { get; set; }
        public string returnStep { set { ReturnStep = Accessor.Set(value); } get { return Accessor.Get(ReturnStep); } }

        [JsonIgnore]
        public int? RejectStep { get; set; }
        public string rejectStep { set { RejectStep = Accessor.Set(value); } get { return Accessor.Get(RejectStep); } }


        [JsonIgnore]
        public int? NextStep { get; set; }

        [JsonIgnore]
        public int? CloseStep { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? JobRoleId { get; set; }


        public string nextStep { set { NextStep = Accessor.Set(value); } get { return Accessor.Get(NextStep); } }
        public string closeStep { set { CloseStep = Accessor.Set(value); } get { return Accessor.Get(CloseStep); } }

        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get(UpdatedBy); } }

        public string jobRoleId { set { JobRoleId = Accessor.Set(value); } get { return Accessor.Get(JobRoleId); } }
        public int? StepNo { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }


        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateString { get; set; }
        public string PersonUpdatedBy { get; set; }
        public bool? HasNote { get; set; }
        public bool? NoteIsRequired { get; set; }
        public bool? IsTransferToReference { get; set; }
        public bool? IsSendEmail { get; set; }
        public string EmailBody { get; set; }
        [JsonIgnore]
        public int? ReferenceJobRole { get; set; }
        public string referenceJobRole { set { ReferenceJobRole = Accessor.Set(value); } get { return Accessor.Get(ReferenceJobRole); } }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.EngineActionJobRole, EnginesActionsJobRole>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)

                .Config;

        }

        public TypeAdapterConfig AddConfig(int? userId, Models.Engine engine)
        {
            return new TypeAdapterConfig()
                  .NewConfig<EnginesActionsJobRole, Models.EngineActionJobRole>().IgnoreNullValues(true)
                // .Map(dest => dest.EngineId, src => engine.Id)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => DateTime.Now)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => false)

                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<EnginesActionsJobRole, Models.EngineActionJobRole>().IgnoreNullValues(true)

                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)

                .Config;
        }
        public static TypeAdapterConfig DeleteConfig(int? userId)
        {
            return new TypeAdapterConfig()
                .NewConfig<Engine, Models.Engine>().IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => DateTime.Now)
                .Map(dest => dest.IsDeleted, src => true)

                .Config;
        }

    }
}
