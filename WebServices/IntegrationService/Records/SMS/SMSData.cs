using Mapster;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IntegrationService.Records.SMS
{
    public record SMSData
    {
        [JsonPropertyName("sendMsgData")]
        public AllMessagesObj SendMsgData { get; set; }

        //public static TypeAdapterConfig MapConfig()
        //{
        //    return new TypeAdapterConfig()
        //        .NewConfig<SMSDataRequestBody, SMSData>()
        //        .Map(dest => dest.SendMsgData.AllMessages, src => src.Adapt<AllMessagesObj>(AllMessagesObj.MapConfig()))

        //        .Config;
        //}
    }

    public record AllMessagesObj
    {
        [JsonPropertyName("allMessages")]
        public SMSDataRequestBody AllMessages { get; set; }

        //public static TypeAdapterConfig MapConfig()
        //{
        //    return new TypeAdapterConfig()
        //        .NewConfig<SMSDataRequestBody, AllMessagesObj>()
        //        .Map(dest => dest.AllMessages, src => src)

        //        .Config;
        //}
    }
    public record SMSDataResponse
    {
        [JsonPropertyName("return")]
        public List<dynamic> ResponseBody { get; set; }
    }

    public record SMSDataRequestBody
    {
        [JsonPropertyName("mobile")]
        public string Mobile { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("sender")]
        public string Sender { get; set; }

        [JsonPropertyName("numbers")]
        public string Numbers { get; set; }

        [JsonPropertyName("msg")]
        public string Msg { get; set; }

    }



    public record SMSDataResponseBody
    {

        [JsonPropertyName("status")]
        public int? Status { get; set; }

        [JsonPropertyName("ResponseStatus")]
        public string ResponseStatus { get; set; }

        [JsonPropertyName("Msg")]
        public string Msg { get; set; }

        [JsonPropertyName("Data")]
        public object Data { get; set; }

    }

    public class GetSMSData
    {
        public GetSMSData(){ }

        [JsonPropertyName("header")]
        public Header Header { get; set; }
        [JsonPropertyName("body")]
        public SMSDataBodyIntegration Body { get; set; }

        public static TypeAdapterConfig MapConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<SMSDataResponse, GetSMSData>()
                .Map(dest => dest.Body.SMSInformation, src => src.ResponseBody)
                .Map(dest => dest.Header.IsSuccess, src => true)

                .Config;
        }
    }

    public class SMSDataBodyIntegration
    {
        [JsonPropertyName("SMSInformation")]
        public List<dynamic> SMSInformation { get; set; }
    }
}
