using Mapster;
using System.Text.Json.Serialization;

namespace IntegrationService.Records.Hars
{
    public record GetGccInfoByPassport
    {
        [JsonPropertyName("getGCCInfoByPassport")]
        public GetGccInfoByPassportRequestBody GetGCCInfoByPassport { get; set; }

        public static TypeAdapterConfig MapConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<GetGccInfoByPassportRequestBody, GetGccInfoByPassport>()
                .Map(dest => dest.GetGCCInfoByPassport, src => src)

                .Config;
        }
    }

    public record GetGccInfoByPassportResponse
    {
        [JsonPropertyName("return")]
        public GetGccInfoByPassportResponseBody ResponseBody { get; set; }
    }

    public record GetGccInfoByPassportRequestBody
    {
        [JsonPropertyName("operatorId")]
        public string OperatorId { get; set; } = "1064500620";


        [JsonPropertyName("systemCode")]
        public string SystemCode { get; set; } = "ESB";


        [JsonPropertyName("clientIpAddress")]
        public string ClientIpAddress { get; set; } = "127.0.0.1";


        [JsonPropertyName("referenceNumber")]
        public string ReferenceNumber { get; set; } = "123";


        [JsonPropertyName("idType")]
        public string IdType { get; set; }

        [JsonPropertyName("gccNIN")]
        public string GccNIN { get; set; }

        [JsonPropertyName("gccNationality")]
        public int? GccNationality { get; set; }

    }

    public record GetGccInfoByPassportResponseBody
    {
        [JsonPropertyName("passportNumber")]
        public long? PassportNumber { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("familyName")]
        public string FamilyName { get; set; }

        [JsonPropertyName("fatherName")]
        public string FatherName { get; set; }

        [JsonPropertyName("grandFatherName")]
        public string GrandFatherName { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("lifeStatusCode")]
        public int? LifeStatusCode { get; set; }

        [JsonPropertyName("logId")]
        public long? LogId { get; set; }

        [JsonPropertyName("dateOfBirthH")]
        public string DateOfBirthH { get; set; }

        [JsonPropertyName("passportExpiryDate")]
        public string PassportExpiryDate { get; set; }

    }


    public class GetPersonInfoDataByPassport
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }
        [JsonPropertyName("body")]
        public BodyIntegration Body { get; set; }

        public static TypeAdapterConfig MapConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<GetGccInfoByPassportResponseBody, GetPersonInfoDataByPassport>()
                .Map(dest => dest.Body.UserInformation, src => src)
                .Map(dest => dest.Header.IsSuccess, src => true)

                .Config;
        }
    }

    public class PassportBodyIntegration
    {
        [JsonPropertyName("userInformation")]
        public GetGccInfoByPassportResponseBody UserInformation { get; set; }
    }
}
