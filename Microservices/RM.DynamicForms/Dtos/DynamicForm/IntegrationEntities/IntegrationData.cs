using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.IntegrationEntities
{
    public class IntegrationData
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }
        [JsonPropertyName("body")]
        public BodyIntegration Body { get; set; }


    }
    public class Header
    {
        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }

    public class BodyIntegration
    {
        [JsonPropertyName("userInformation")]
        public UserInformation UserInformation { get; set; }
    }

    public class UserInformation
    {
        public UserInformation()
        {
            securityRecordList = new List<SecurityRecord>();
        }
        [JsonIgnore]
        public int? _genderId { get; set; }
        public string GenderId { set { _genderId = Accessor.Set(value); } get { return Accessor.Get<int?>(_genderId); } }
        [JsonIgnore]
        public int? _lifeCode { get; set; }
        public string LifeCode { set { _lifeCode = Accessor.Set(value); } get { return Accessor.Get<int?>(_lifeCode); } }


        public string idNumber { get; set; }

        public string birthOfDate { get; set; }
        public string birthOfDateGeorogian { get; set; }
        public string birthOfDateHijri { get; set; }

        public string idTypeCode { get; set; }
        public bool birthOfDateIsHijri { get; set; }

        public long logId { get; set; }

        public string arabicName { get; set; }
        public string englishName { get; set; }
        public string dateOfBirthG { get; set; }
        public string passportExpiryDate { get; set; }
        public string sponsorName { get; set; }
        public string idExpiryDateH { get; set; }
        public string gender { get; set; }
        public bool genderSpecified { get; set; }
        public string idIssueDateH { get; set; }
        public string idIssuePlace { get; set; }
        public string lifeStatus { get; set; }
        public bool lifeStatusSpecified { get; set; }
        public string nationalityDesc { get; set; }
        public string passportNumber { get; set; }
        public string occupationDesc { get; set; }
        public string visaNumber { get; set; }
        public bool IsSecutiryApproved { get; set; }
        public List<SecurityRecord> securityRecordList { get; set; }
        public string sponsorMoiNumber { get; set; }
        public bool isForigin { get; set; }

    }

    public class SecurityRecord
    {
        public int actionCode { get; set; }
        public string actionType { get; set; }
    }
}
