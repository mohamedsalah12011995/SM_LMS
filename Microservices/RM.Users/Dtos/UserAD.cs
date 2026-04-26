using System.Text.Json.Serialization;

namespace RM.Users.Dtos
{
    public class UserAD
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }
        [JsonPropertyName("body")]
        public Body Body { get; set; }


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

    public class Body
    {
        [JsonPropertyName("userInformation")]
        public UserInformation userInformation { get; set; }
        public bool? loginSuccess { get; set; }

    }

    public class UserInformation
    {
        public string username { get; set; }
        public string FullName { get; set; }
        public string PositionTitle { get; set; }
        public string Email { get; set; }
        public string PhoneExtention { get; set; }
        public string MobileNumber { get; set; }
        public string IdentityNumber { get; set; }
        public string UnitName { get; set; }
        public string[] unitInfo { get; set; }




    }


}
