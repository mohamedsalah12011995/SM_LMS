
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.Jobs.Dtos
{
    public class SmsIntegrationData
    {

        [JsonPropertyName("header")]
        public Header Header { get; set; }
        [JsonPropertyName("body")]
        public Body Body { get; set; }

    }
    public class SmsResponse
    {
       
        public bool success { get; set; }
        [JsonPropertyName("providerResult")]
        public ProviderResult providerResult { get; set; }
    }
    public class Body
    {
        [JsonPropertyName("smsResponse")]
        public SmsResponse smsResponse { get; set; }
      
    }

    public class ProviderResult
    {
        public int? statusMy { get; set; }
        public string ResponseStatus { get; set; }
        public string  Data{ get; set; }
        public Error Error { get; set; }

    }
    public class Error
    {
     
        public int? ErrorCode { get; set; }
        public string MessageAr { get; set; }
        public string MessageEn { get; set; }
    }

    public class SmsBody
    {
        public SmsBody()
        {
            phonesNumber = new List<string>();
        }
        public string messageBody { get; set; }
        public List<string> phonesNumber { get; set; }
    }
}
