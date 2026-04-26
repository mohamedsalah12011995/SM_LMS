using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.WorkFlow.Dtos.DyFormEntities
{
    public class Entity
    {

        [JsonIgnore]
        public int? _id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string FrontIdentity { get; set; }

        public string CmsIdentity { get; set; }

        public string Id { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }



    }
}
