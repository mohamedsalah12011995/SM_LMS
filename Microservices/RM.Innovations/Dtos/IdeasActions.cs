
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.Text.Json.Serialization;

namespace RM.Innovations.Dtos
{
    public class IdeasActions:BaseDto<IdeasActions,Models.IdeaAction>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ActionId { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string actionId { set { ActionId = Accessor.Set(value); } get { return Accessor.Get<int?>(ActionId); } }
        public string ActionName { get; set; }
        public string CreatedBy { get; set; }
        public string ActionNote { get; set; }
        public DateTime? CreatedDate { get; set; }


        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.IdeaAction, IdeasActions>()
                .Map(dest => dest.ActionName, src => src.TypeNavigation != null ? src.TypeNavigation.NameAr : string.Empty)
                .Map(dest => dest.ActionNote, src => src.Note)
                .Map(dest => dest.CreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)

                 .Config;

        }
    }
}
