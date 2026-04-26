
using RM.Core.Helpers;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.Innovations.Dtos
{
    public class IdeasLookups
    {
        public List<Lookups>? Status { get; set; }
        public List<Lookups>? Category { get; set; }
        public List<Lookups>? Priority { get; set; }
        public List<Lookups>? Type { get; set; }
        public List<Lookups>? Actions { get; set; }
        public List<Lookups>? ToReference { get; set; }
        public List<Lookups>? Capability { get; set; }
        public List<Lookups>? Feasibility { get; set; }
        public List<Lookups>? NeedsBudget { get; set; }
        public List<Lookups>? ToJobRole { get; set; }

        public class Lookups
        {
            [JsonIgnore]
            public int? Id { get; set; }
            public string? ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
            public string? NameAr { get; set; }
            public string? NameEn { get; set; }
            public string? Status { get; set; }
        }
    }
    
}
