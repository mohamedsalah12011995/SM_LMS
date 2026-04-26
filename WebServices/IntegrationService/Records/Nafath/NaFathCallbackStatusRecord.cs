using System.Text.Json.Serialization;

namespace IntegrationService.Records.Nafath
{
    public class NaFathCallbackStatusRecord
    {
        public string Response { get; set; }
        public string AccessToken { get; set; }
    }

    public class NaFathCallbackStatusResult
    {
        [JsonPropertyName("transId")]
        public string TransId { get; set; }
        [JsonPropertyName("status")]

        public string Status { get; set; }
    }
}
