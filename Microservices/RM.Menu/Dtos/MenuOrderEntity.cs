using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Menu.Dtos
{
    public class MenuOrderEntity
    {
        [JsonIgnore]
        public int? _id { get; set; }
        public string Id { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }

        public List<MenuTree> menus { get; set; }


    }
}
