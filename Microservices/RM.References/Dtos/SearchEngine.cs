using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.References.Dtos
{
    public class SearchEngine
    {

        public class Request
        {
            [JsonIgnore]
            public int? _referenceId { get; set; }
            public string ReferenceID { set { _referenceId = Accessor.Set(value); } get { return Accessor.Get(_referenceId); } }
            public string SearchWord { get; set; }
            public DateTime? FromDate { set; get; }
            public DateTime? ToDate { set; get; }
            public List<TargetedEntities> ListTargetedEntities { get; set; }
            public string _targetedEntities { get { return ListTargetedEntities != null ? Strings.ConvertListToStringInSearchEngine(ListTargetedEntities.Select(x => x._id.ToString()).ToList()) : null; } }
        }

        public class Response
        {
            [JsonIgnore]
            public int? _id { get; set; }
            [JsonIgnore]
            public int? _entityId { get; set; }
            public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
            public string EntityID { set { _entityId = Accessor.Set(value); } get { return Accessor.Get(_entityId); } }
            public DateTime? CreatedDate { get; set; }
            public string TitleAr { get; set; }
            public string TitleEn { get; set; }
            public string BriefeContentAr { get; set; }
            public string BriefeContentEn { get; set; }
            public string Url { get; set; }
            public string SearchNameAr { get; set; }
            public string SearchNameEn { get; set; }

        }
        public class TargetedEntities
        {
            [JsonIgnore]
            public int? _id { get; set; }
            public string ID { set { _id = Accessor.Set(value); } get { return Accessor.Get(_id); } }
        }

    }
}
