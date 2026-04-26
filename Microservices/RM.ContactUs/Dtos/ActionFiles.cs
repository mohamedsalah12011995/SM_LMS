
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.ContactUs.Dtos
{
    public class ActionFiles : BaseDto<ActionFiles, Models.ActionFiles>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string? id { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        [JsonIgnore]
        public int? ActionId { get; set; }
        public string? actionId { set { ActionId = Accessor.Set(value); } get { return Accessor.Get<int?>(ActionId); } }

        public string? FileUrl { get; set; }
        public string? FileUrlBase64 { get; set; }

        public string? FileName { get; set; }
        public string? FileType { get; set; }

        public static TypeAdapterConfig SelectConfig(string ImagesGetPath, string FilesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.ActionFiles, ActionFiles>()
                .Map(dest => dest.FileUrl, src => !string.IsNullOrEmpty(src.FileType) ? src.FileType.Contains("image") ? $"{ImagesGetPath}/{src.FileUrl}" : $"{FilesGetPath}/{src.FileUrl}" : string.Empty)
                .Config;
        }
    }
}
