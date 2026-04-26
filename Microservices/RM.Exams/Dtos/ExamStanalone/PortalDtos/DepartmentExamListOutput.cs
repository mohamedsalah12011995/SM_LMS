using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone.PortalDtos
{
    public class DepartmentExamListOutput
    {
        [JsonIgnore]
        public int? ExamId { get; set; }

        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get(ExamId); } }

        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FromDateString { get; set; }
        public string ToDateString { get; set; }
        public string RemainTime { get; set; }

        //public static TypeAdapterConfig SelectConfig()
        //{
        //    return new TypeAdapterConfig()
        //        .NewConfig<IGrouping<GroupExam, Models.UserApplicationExam>, DepartmentExamListOutput>()

        //       .Map(dest => dest.ExamId, src => src.Key.ExamId)
        //       .Map(dest => dest.TitleAr, src => src.First().Exam.TitleAr)
        //       .Map(dest => dest.TitleEn, src => src.First().Exam.TitleEn)
        //       .Map(dest => dest.DescriptionAr, src => src.First().Exam.DescriptionAr)
        //       .Map(dest => dest.DescriptionEn, src => src.First().Exam.DescriptionEn)
        //       .Map(dest => dest.FromDate, src => src.Key.FromDate.Value)
        //       .Map(dest => dest.ToDate, src => src.Key.ToDate.Value)
        //       .Map(dest => dest.FromDateString, src => src.Key.FromDate.Value.ToString("yyyy-MM-dd"))
        //       .Map(dest => dest.ToDateString, src => src.Key.ToDate.Value.ToString("yyyy-MM-dd"))
        //       .Map(dest => dest.RemainTime, src => src.Key.ToDate.Value.Subtract(GetDateTime()).TotalMinutes.ToString())
        //        .Config;
        //}


    }

    public record GroupExam(int ExamId, DateTime? FromDate, DateTime? ToDate);

}
