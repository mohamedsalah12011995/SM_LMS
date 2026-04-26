using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Multimedia.Dtos
{
    public class MultimediaDetailsDto
    {
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        [JsonIgnore]
        public int? ReferenceId { get; set; }

        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }

    }
}
