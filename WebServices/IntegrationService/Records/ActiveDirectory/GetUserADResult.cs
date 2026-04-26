
using System.Text.Json.Serialization;

namespace IntegrationService.Records.AD
{
    public class UserAD
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }
        [JsonPropertyName("body")]
        public Body Body { get; set; }


    }

    public class Body
    {
        [JsonPropertyName("userInformation")]
        public UserInformation UserInformation { get; set; }

        [JsonPropertyName("loginSuccess")]
        public bool? LoginSuccess { get; set; }

    }

    public class UserInformation
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }

        public string PositionTitle { get; set; }
        public string Email { get; set; }
        public string PhoneExtention { get; set; }
        public string MobileNumber { get; set; }
        public string IdentityNumber { get; set; }
        public string UnitName { get; set; }
        public string[] unitInfo { get; set; }
        public bool? IsLockout {  get; set; }
        public string SamAccountName {  get; set; }
        public string Sid {  get; set; }
    }

}
