using RM.Core.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace RM.Competitions.Dtos
{
    public class AttachmentType
    {
        [JsonIgnore]
        public int? _id { get; set; }
        [JsonIgnore]
        internal string _acceptedExtention { get; set; }
        public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get<int?>(_id); } }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int? MinCount { get; set; }
        public int? MaxCount { get; set; }
        public List<string> AcceptedExtention { get { return _acceptedExtention.Split(',').ToList(); } }
        public string Description { get; set; }
        public bool? IsRequired { get; set; }
        

    }
}
