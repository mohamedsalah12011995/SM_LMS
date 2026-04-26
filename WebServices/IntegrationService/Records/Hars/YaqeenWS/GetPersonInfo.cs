using IntegrationService.Records.Hars;
using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace IntegrationService.Records.Hars
{

    public class GetPersonInfo
    {

        [JsonPropertyName("PersonInfoRequest")]
        public GetPersonInfoRequestBody PersonInfoRequest { get; set; }

        public static TypeAdapterConfig MapConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<GetPersonInfoRequestBody, GetPersonInfo>()
                .Map(dest => dest.PersonInfoRequest, src => src)

                .Config;
        }
    }

    public record GetPersonInfoResponse
    {
        [JsonPropertyName("return")]
        public GetPersonInfoResponseBody ResponseBody { get; set; }
    }

    public class GetPersonInfoRequestBody
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


        [JsonPropertyName("idNumber")]
        public string IdNumber { get; set; }


        [JsonPropertyName("dateOfBirth")]
        public string DateOfBirth { get; set; }

    }

    public record GetPersonInfoResponseBody
    {
        [JsonPropertyName("passportNumber")]
        public long? PassportNumber { get; set; }

        [JsonPropertyName("arabicName")]
        public string ArabicName { get; set; }

        [JsonPropertyName("englishName")]
        public string EnglishName { get; set; }

        [JsonPropertyName("sponsorName")]
        public string SponsorName { get; set; }

        [JsonPropertyName("passportExpiryDate")]
        public string PassportExpiryDate { get; set; }

        [JsonPropertyName("idExpiryDateH")]
        public string IdExpiryDateH { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("lifeStatus")]
        public string LifeStatus { get; set; }

        [JsonPropertyName("logId")]
        public long? LogId { get; set; }

        [JsonPropertyName("dateOfBirthH")]
        public string DateOfBirthH { get; set; }

        [JsonPropertyName("dateOfBirthG")]
        public string DateOfBirthG { get; set; }

        [JsonPropertyName("visaNumber")]
        public string VisaNumber { get; set; }

        [JsonPropertyName("securityStatus")]
        public string SecurityStatus { get; set; }

        [JsonPropertyName("occupationDesc")]
        public string OccupationDesc { get; set; }

        [JsonPropertyName("nationalityDesc")]
        public string NationalityDesc { get; set; }

        [JsonPropertyName("idVersionNumber")]
        public int? IdVersionNumber { get; set; }

        [JsonPropertyName("idIssueDateH")]
        public string IdIssueDateH { get; set; }

        [JsonPropertyName("idIssuePlace")]
        public string IdIssuePlace { get; set; }

        [JsonPropertyName("sponsorMoiNumber")]
        public string SponsorMoiNumber { get; set; }
    }
}



public class GetPersonInfoData
{
    [JsonPropertyName("header")]
    public Header Header { get; set; }
    [JsonPropertyName("body")]
    public BodyIntegration Body { get; set; }

    public static TypeAdapterConfig MapConfig()
    {
        return new TypeAdapterConfig()
            .NewConfig<GetPersonInfoResponseBody, GetPersonInfoData>()
            .Map(dest => dest.Body.UserInformation, src => src)
            .Map(dest => dest.Header.IsSuccess, src => true)

            .Config;
    }
}
public class Header
{
    [JsonPropertyName("isSuccess")]
    public bool? IsSuccess { get; set; }
    [JsonPropertyName("code")]
    public int? Code { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
}

public class BodyIntegration
{
    [JsonPropertyName("userInformation")]
    public GetPersonInfoResponseBody UserInformation { get; set; }
}
