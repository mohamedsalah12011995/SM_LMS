using Mapster;
using System.Text.Json.Serialization;

namespace RM.Users.Dtos
{
    public class SMSIntegration
    {
        [JsonPropertyName("header")]
         public Header Header { get; set; }
         [JsonPropertyName("body")]
         public SMSDataBodyIntegration Body { get; set; }  
    }

    public class SMSDataBodyIntegration
    {
        [JsonPropertyName("SMSInformation")]
        public dynamic SMSInformation { get; set; }
    }
}
