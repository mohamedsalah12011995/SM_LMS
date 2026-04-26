using DocumentFormat.OpenXml.Bibliography;
using System.Drawing;
using System.Text.Json.Serialization;

namespace IntegrationService.Records.Hars
{
    public record GetDetailedStatistics
    {
        [JsonPropertyName("categoryId")]
        public int? CategoryId { get; set; }

        [JsonPropertyName("year")]
        public int? Year { get; set; }

        [JsonPropertyName("fromMonth")]
        public int? FromMonth { get; set; }

        [JsonPropertyName("toMonth")]
        public string ToMonth { get; set; }

        [JsonPropertyName("regionId")]
        public long? RegionId { get; set; }

        [JsonPropertyName("isHijri")]
        public bool? IsHijri { get; set; }
    }
}
