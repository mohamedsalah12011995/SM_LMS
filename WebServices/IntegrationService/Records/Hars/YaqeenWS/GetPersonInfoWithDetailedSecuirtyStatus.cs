//using System.Text.Json.Serialization;

//namespace IntegrationService.Records.Hars
//{
//    public record GetPersonInfoWithDetailedSecuirtyStatus
//    {
//        [JsonPropertyName("PersonInfoRequest")]
//        public GetPersonInfoWithDetailedSecuirtyStatusRequestBody PersonInfoRequest { get; set; }
//    }

//    public record GetPersonInfoWithDetailedSecuirtyStatusResponse
//    {
//        [JsonPropertyName("return")]
//        public GetPersonInfoWithDetailedSecuirtyStatusResponseBody ResponseBody { get; set; }
//    }

//    public record GetPersonInfoWithDetailedSecuirtyStatusRequestBody
//    {
//        [JsonPropertyName("operatorId")]
//        public string OperatorId { get; set; }

//        [JsonPropertyName("systemCode")]
//        public string SystemCode { get; set; }

//        [JsonPropertyName("idType")]
//        public string IdType { get; set; }

//        [JsonPropertyName("idNumber")]
//        public string IdNumber { get; set; }

//        [JsonPropertyName("dateOfBirth")]
//        public string DateOfBirth { get; set; }

//        [JsonPropertyName("clientIpAddress")]
//        public string ClientIpAddress { get; set; }

//        [JsonPropertyName("referenceNumber")]
//        public string ReferenceNumber { get; set; }

//    }

//    public record GetPersonInfoWithDetailedSecuirtyStatusResponseBody
//    {
//        [JsonPropertyName("passportNumber")]
//        public long? PassportNumber { get; set; }

//        [JsonPropertyName("arabicName")]
//        public string ArabicName { get; set; }

//        [JsonPropertyName("englishName")]
//        public string EnglishName { get; set; }

//        [JsonPropertyName("sponsorName")]
//        public string SponsorName { get; set; }

//        [JsonPropertyName("passportExpiryDate")]
//        public string PassportExpiryDate { get; set; }

//        [JsonPropertyName("idExpiryDateH")]
//        public string IdExpiryDateH { get; set; }

//        [JsonPropertyName("gender")]
//        public string Gender { get; set; }

//        [JsonPropertyName("lifeStatus")]
//        public string LifeStatus { get; set; }

//        [JsonPropertyName("logId")]
//        public long? LogId { get; set; }

//        [JsonPropertyName("dateOfBirthH")]
//        public string DateOfBirthH { get; set; }

//        [JsonPropertyName("dateOfBirthG")]
//        public string DateOfBirthG { get; set; }

//        [JsonPropertyName("visaNumber")]
//        public string VisaNumber { get; set; }

//        [JsonPropertyName("securityStatus")]
//        public string SecurityStatus { get; set; }

//        [JsonPropertyName("occupationDesc")]
//        public string OccupationDesc { get; set; }

//        [JsonPropertyName("nationalityDesc")]
//        public string NationalityDesc { get; set; }

//        [JsonPropertyName("idVersionNumber")]
//        public int? IdVersionNumber { get; set; }

//        [JsonPropertyName("idIssueDateH")]
//        public string IdIssueDateH { get; set; }

//        [JsonPropertyName("idIssuePlace")]
//        public string IdIssuePlace { get; set; }

//        [JsonPropertyName("sponsorMoiNumber")]
//        public string SponsorMoiNumber { get; set; }


//    }
//}
