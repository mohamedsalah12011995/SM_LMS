using System.Text.Json.Serialization;

namespace RM.Users.Dtos
{
    public class EmployeeInformation
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }
        [JsonPropertyName("body")]
        public BodyInfo Body { get; set; }
    }

    public class BodyInfo
    {
        public employeeInformation employeeInformation { get; set; }
    }

    public class employeeInformation
    {
        public string birthDateString { get; set; }
        public string rankTitleDesc { get; set; }
        public long? empId { get; set; }
        public string physicalUnitFullName { get; set; }
        public string ipPhoneExt { get; set; }
        public string officialUnitFullName { get; set; }
        public int? officialRegionId { get; set; }
        public string socialID { get; set; }
        public string shieldMobileNumber { get; set; }
        public string email { get; set; }
        public long? officialUnitId { get; set; }
        public string phoneExt { get; set; }
        public string statusDesc { get; set; }
        public string rankDesc { get; set; }
        public string jobDesc { get; set; }
        public long? jobId { get; set; }
        public long? statusId { get; set; }
        public long? rankId { get; set; }
        public string name { get; set; }
        public string privateMobileNumber { get; set; }
        public int? rankTitleId { get; set; }
        public string officialMobileNumber { get; set; }
        public long? physicalUnitId { get; set; }
        public int? categoryId { get; set; }
        public int? physicalRegionId { get; set; }




    }
}
