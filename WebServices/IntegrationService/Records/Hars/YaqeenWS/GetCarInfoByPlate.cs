using Mapster;
using System.Text.Json.Serialization;

namespace IntegrationService.Records.Hars
{
    public record GetCarInfoByPlate
    {
        [JsonPropertyName("CarInfoByPlateRequest")]
        public GetCarInfoByPlateRequestBody CarInfoByPlateRequest { get; set; }

        public static TypeAdapterConfig MapConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<GetCarInfoByPlateRequestBody, GetCarInfoByPlate>()
                .Map(dest => dest.CarInfoByPlateRequest, src => src)

                .Config;
        }
    }

    public record GetCarInfoByPlateResponse
    {
        [JsonPropertyName("return")]
        public GetCarInfoByPlateResponseBody ResponseBody { get; set; }
    }

    public record GetCarInfoByPlateRequestBody
    {
        [JsonPropertyName("operatorId")]
        public string OperatorId { get; set; } = "1023859216";

        [JsonPropertyName("systemCode")]
        public string SystemCode { get; set; } = "ESB";

        [JsonPropertyName("clientIpAddress")]
        public string ClientIpAddress { get; set; } = "127.0.0.1";

        [JsonPropertyName("referenceNumber")]
        public string ReferenceNumber { get; set; } = "123";


        [JsonPropertyName("plateType")]
        public int? PlateType { get; set; }

        [JsonPropertyName("plateNumber")]
        public int? PlateNumber { get; set; }

        [JsonPropertyName("plateText1")]
        public string PlateText1 { get; set; }

        [JsonPropertyName("plateText2")]
        public string PlateText2 { get; set; }

        [JsonPropertyName("plateText3")]
        public string PlateText3 { get; set; }

    }



    public record GetCarInfoByPlateResponseBody
    {

        [JsonPropertyName("sequenceNumber")]
        public long? SequenceNumber { get; set; }

        [JsonPropertyName("regIssueDate")]
        public string RegIssueDate { get; set; }

        [JsonPropertyName("vehicleMaker")]
        public string VehicleMaker { get; set; }

        [JsonPropertyName("legalStatus")]
        public bool? LegalStatus { get; set; }

        [JsonPropertyName("vehicleModel")]
        public string VehicleModel { get; set; }

        [JsonPropertyName("logId")]
        public long? LogId { get; set; }

        [JsonPropertyName("majorColor")]
        public string MajorColor { get; set; }

        [JsonPropertyName("regPlace")]
        public string RegPlace { get; set; }

        [JsonPropertyName("modelYear")]
        public int? ModelYear { get; set; }

        [JsonPropertyName("ownerId")]
        public long? OwnerId { get; set; }

        [JsonPropertyName("regExpiryHDate")]
        public string RegExpiryHDate { get; set; }

    }

    public class GetCarInfoData
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }
        [JsonPropertyName("body")]
        public CarBodyIntegration Body { get; set; }

        public static TypeAdapterConfig MapConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<GetCarInfoByPlateResponseBody, GetCarInfoData>()
                .Map(dest => dest.Body.CarInformation, src => src)
                .Map(dest => dest.Header.IsSuccess, src => true)

                .Config;
        }
    }

    public class CarBodyIntegration
    {
        [JsonPropertyName("carInformation")]
        public GetCarInfoByPlateResponseBody CarInformation { get; set; }
    }
}
