

using RM.Core.Helpers;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.OpenData.Dtos
{
    public class OpenDataIntegration
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }
        [JsonPropertyName("body")]
        public Body Body { get; set; }


    }
   public class Header
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class Body
    {
        [JsonPropertyName("openDataStatistics")]
        public OpenDataStatistics OpenDataStatistics { get; set; }

    }

    public class OpenDataStatistics
    {
    
        [JsonPropertyName("arrestingDetailed")]
        public List<ArrestingDetailed> ArrestingDetailed { get; set; }
        [JsonPropertyName("InfiltrationDetailed")]
        public List<InfiltrationDetailed> InfiltrationDetailed { get; set; }
        [JsonPropertyName("SmugglingDetailed")]
        public List<SmugglingDetailed> SmugglingDetailed { get; set; }
        [JsonPropertyName("SearchShareMissionDetailed")]
        public List<SearchShareMissionDetailed> SearchShareMissionDetailed { get; set; }
        [JsonPropertyName("ViolationsDetailed")]
        public List<ViolationsDetailed> ViolationsDetailed { get; set; }



    }


    public class OpenDataDetails
    {

        [JsonPropertyName("region")]

        public string Region { get; set; }
        [JsonPropertyName("regionId")]

        public long? RegionId { get; set; }
        [JsonPropertyName("year")]

        public int? Year { get; set; }
        [JsonPropertyName("month")]

        public int? Month { get; set; }

    }

    public class InfiltrationDetailed : OpenDataDetails
    {
        [JsonPropertyName("count")]
        public float? Count { get; set; }

    }

    public class SmugglingDetailed : OpenDataDetails
    {
        [JsonPropertyName("count")]
        public float? Count { get; set; }

    }

    public class SearchShareMissionDetailed : OpenDataDetails
    {
        [JsonPropertyName("marineRescueOperationsCount")]
        public float MarinCount { get; set; }
        [JsonPropertyName("landRescueOperationsCount")]
        public float WildRescue { get; set; }
        [JsonPropertyName("supportOperationsCount")]
        public float ProvideBacking { get; set; }

    }

    public class ArrestingDetailed : OpenDataDetails
    {
        [JsonPropertyName("accomodationSecuritySysViolatorsCount")]
        public float? AccomodationSecuritySysViolatorsCount { get; set; }

        [JsonPropertyName("bordersSecuritySysViolatorsCount")]
        public float? BordersSecuritySysViolatorsCount { get; set; }

    }

    public class ViolationsDetailed : OpenDataDetails
    {
        [JsonPropertyName("hayKgCount")]
        public float? HayKgCount { get; set; }

        [JsonPropertyName("qatKgCount")]
        public float? QatKgCount { get; set; }

        [JsonPropertyName("shabuKgCount")]
        public float? ShabuKgCount { get; set; }

        [JsonPropertyName("prohibitedDrugsCount")]
        public float? ProhibitedDrugsCount { get; set; }

        [JsonPropertyName("narcoticPillsCount")]
        public float? NarcoticPillsCount { get; set; }

    }

    

    public class RequestRecords
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        [JsonIgnore]
        [JsonPropertyName("categoryId")]

        public int? CategoryId { get; set; }
        public string categoryId { set { CategoryId = Accessor.Set(value); } get { return Accessor.Get<int?>(CategoryId); } }
        [JsonIgnore]
        [JsonPropertyName("regionId")]

        public long? RegionId { get; set; }
        public string regionId { set { RegionId = Accessor.Set(value); } get { return Accessor.Get<long?>(RegionId); } }

        [JsonPropertyName("year")]

        public int? Year { get; set; }
        [JsonPropertyName("fromMonth")]

        public int? FromMonth { get; set; }
        [JsonPropertyName("toMonth")]

        public int? ToMonth { get; set; }
        public bool? IsGregorian { get; set; }

    }

    public class SyncOpenData
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }

        [JsonIgnore]
        public int? TypeId { get; set; }
        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(TypeId); } }

        public List<OpenDataDetails> OpenDataDetails { get;set; } = new List<OpenDataDetails>();
    }


}

