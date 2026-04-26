
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.Models;
using System.Text.Json.Serialization;

namespace RM.ContactUs.Dtos
{
    public class Action : BaseDto<Action, Models.Actions>
    {
        public Action()
        {
            ActionFiles = new List<ActionFiles>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ContactId { get; set; }

        [JsonIgnore]
        public int? ToReferenceId { get; set; }
        [JsonIgnore]
        public int? UserReferenceId { get; set; }

        [JsonIgnore]
        public int? FromUserId { get; set; }


        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedDateString { get; set; }


        public int? StatusId { get; set; }
        public string StatusNameAr { get; set; }
        public string StatusNameEn { get; set; }
        public string FromUserName { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string fromUserId { set { FromUserId = Accessor.Set(value); } get { return Accessor.Get<int?>(FromUserId); } }

        public string toReferenceId { set { ToReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ToReferenceId); } }
        public string userReferenceId { set { UserReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(UserReferenceId); } }
        public string contactId { set { ContactId = Accessor.Set(value); } get { return Accessor.Get<int?>(ContactId); } }

        public string Note { get; set; }

        public string ToReferenceAr { get; set; }
        public string FromRefernceAr { get; set; }
        public string FromRefernceEn { get; set; }
        public string ToReferenceEn { get; set; }
        public bool? LastActionisOfficer { get; set; }

        public List<ActionFiles> ActionFiles { get; set; }

        public static TypeAdapterConfig SelectConfig(string ImagesGetPath, string FilesGetPath, bool FullDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<Actions, Action>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? FullDate ?
                 src.CreatedDate.Value.ToString("yyyy-MM-dd h:mm:ss tt") : src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)

                .Map(dest => dest.StatusNameAr, src => src.Status != null ? src.Status.NameAr : null)
                .Map(dest => dest.StatusNameEn, src => src.Status != null ? src.Status.NameEn : null)

                .Map(dest => dest.FromUserName, src => src.FromUser != null ? src.FromUser.Name : null)
                .Map(dest => dest.ToReferenceAr, src => src.ToReference != null ? src.ToReference.NameAr : null)
                .Map(dest => dest.ToReferenceEn, src => src.ToReference != null ? src.ToReference.NameEn : null)

                .Map(dest => dest.FromRefernceAr, src => src.FromUser != null && src.FromUser.Reference != null ? src.FromUser.Reference.NameAr : null)
                .Map(dest => dest.FromRefernceEn, src => src.FromUser != null && src.FromUser.Reference != null ? src.FromUser.Reference.NameEn : null)

                .Map(dest => dest.ActionFiles, src => src.ActionFiles.Any() ? src.ActionFiles.Adapt<List<Dtos.ActionFiles>>(Dtos.ActionFiles.SelectConfig(ImagesGetPath, FilesGetPath)) : new List<ActionFiles>())

                .Config;
        }
    }
}
