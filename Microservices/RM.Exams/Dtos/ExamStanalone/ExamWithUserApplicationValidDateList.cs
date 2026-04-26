using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone
{
    public class ExamWithUserApplicationValidDateList
    {
        public ExamWithUserApplicationValidDateList()
        {
            DateList = new List<UserApplicationValidDates>();
        }
        [JsonIgnore]
        public int? Id { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }

        public string CertificateTitleAr { get; set; }
        public string CertificateTitleEn { get; set; }

        public List<UserApplicationValidDates> DateList { get; set; }


    }

    public class UserApplicationValidDates
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string FromDateString { get; set; }
        public string ToDateString { get; set; }

    }
}
