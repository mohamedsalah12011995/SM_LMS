using System.Text.Json.Serialization;

namespace RM.Competitions.Dtos
{
    public class Cities
    {
        [JsonPropertyName("id")]
        public string ID { get; set; }
        [JsonPropertyName("nameAr")]
        public string NameAr { get; set; }
    }
}
