using DocumentFormat.OpenXml.Office2010.ExcelAc;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RM.Jobs.Dtos
{
    public class MilitaryJobsIntegration
    {
        [JsonPropertyName("header")]
        public Header Header { get; set; }
        [JsonPropertyName("body")]
        public BodyMilitary Body { get; set; }
    }
    public class BodyMilitary
    {
        public BodyMilitary()
        {
            MillitaryApplicationInfo = new List<MillitaryApplicationInfo>();
        }
        [JsonPropertyName("millitaryApplicationInfo")]
        public  List<MillitaryApplicationInfo> MillitaryApplicationInfo { get; set; }
    }

    public class MillitaryApplicationInfo
    {
        public string jobId { get; set; }
        public string jobName { get; set; }
        public string fullName { get; set; }
        public string applicantNo { get; set; }
        public string status { get; set; }
             
    }
}
