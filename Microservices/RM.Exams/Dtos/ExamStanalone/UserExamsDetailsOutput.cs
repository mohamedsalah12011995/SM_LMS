using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone
{
    public class UserExamsDetailsOutput
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        [JsonIgnore]
        public int? UserId { get; set; }
        public string userId { set { UserId = Accessor.Set(value); } get { return Accessor.Get<int?>(UserId); } }

        public string UserName { get; set; }
        public string ExamTitleAr { get; set; }
        public string ExamTitleEn { get; set; }
        public string UserReferenceNameAr { get; set; }
        public string UserReferenceNameEn { get; set; }
        public string IdCardNumber { get; set; }
        public bool? IsAttendedExam { get; set; }
        public string ExamDate { get; set; }
        public string ExamTime { get; set; }
        public string AttendedExamAr { get; set; }
        public string AttendedExamEn { get; set; }
        public double Result { get; set; }
        public string StatusAr { get; set; }
        public string StatusEn { get; set; }
        public double? SuccessRate { get; set; }

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.UserApplicationExam, UserExamsDetailsOutput>()
                .Map(dest => dest.IsAttendedExam, src => src.IsSuccess != null ? true : false)
                .Map(dest => dest.AttendedExamAr, src => src.IsSuccess != null ? "تم الحضور" : "لم يتم الحضور")
                .Map(dest => dest.AttendedExamEn, src => src.IsSuccess != null ? "Attended" : "Not Attended")
                .Map(dest => dest.StatusAr, src => src.IsSuccess == true ? "ناجح" : src.IsSuccess == false ? "راسب" : string.Empty)
                .Map(dest => dest.StatusEn, src => src.IsSuccess == true ? "Passed" : src.IsSuccess == false ? "Failed" : string.Empty)
                .Map(dest => dest.SuccessRate, src => src.IsSuccess == true && src.Result > 0 ? (src.Result / 100) * 100.0 : 0)
                .Map(dest => dest.ExamTitleAr, src => src.Exam != null ? src.Exam.TitleAr : string.Empty)
                .Map(dest => dest.ExamTitleEn, src => src.Exam != null ? src.Exam.TitleEn : string.Empty)
                .Map(dest => dest.IdCardNumber, src => src.User != null ? src.User.IdCardNumber : string.Empty)
                .Map(dest => dest.ExamDate, src => src.FromDate.Value.ToString("yyyy-MM-dd"))
                .Map(dest => dest.ExamTime, src => $"{src.FromDate.Value.ToString("hh:mm tt")}{src.ToDate.Value.ToString("hh:mm tt")}")

                .Config;
        }




    }
}
