using System.Text.Json.Serialization;

namespace IntegrationService.Records.AD
{
    public class GetADUserRecord
    {
        [JsonPropertyName("username")]
        public string UserName {  get; set; }   
        public string DesiredUserName { get; set; }
        public string Password { get; set; }
    }
}
