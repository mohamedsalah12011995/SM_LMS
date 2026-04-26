using System.Text.Json.Serialization;

namespace IntegrationService.Records.Hars
{
    public record GetWSData
    {
        public dynamic Object {  get; set; }
        public string API { get; set; }
    }
}
