using Mapster;
using RM.Core.Helpers;
using RM.Exams.Dtos.ExamTrainingCourses.PortalDtos;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone.PortalDtos
{
    public class QueryUserApplicationOutputDto
    {
        [JsonIgnore]
        public int? Id { get; set; } // userApplicationexam  Id
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        [JsonIgnore]
        public int? UserId { get; set; }
        public string userId { set { UserId = Accessor.Set(value); } get { return Accessor.Get(UserId); } }

        [JsonIgnore]
        public int? _userReferenceId { get; set; }
        public string userReferenceId { set { _userReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(_userReferenceId); } }

        public string UserReferenceNameAr { get; set; }
        public string UserReferenceNameEn { get; set; }

        public string UserName { get; set; }
        public string IdCardNumber { get; set; }


        public bool IsExamAvailable { get; set; }

        public string ExamTitleAr { get; set; }
        public string ExamTitleEn { get; set; }
        public string ExamDateStringAr { get; set; }
        public string ExamDateStringEn { get; set; }
        public string ExamResultStringAr { get; set; }
        public string ExamResultStringEn { get; set; }
        public bool? IsSuccess { get; set; }

        public TraineeCertificate TraineeCertificate { get; set; }



        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.UserApplicationExam, QueryUserApplicationOutputDto>()
                .Map(dest => dest._userReferenceId, src => src.User != null ? src.User.ReferenceId : null)
                .Map(dest => dest.UserReferenceNameAr, src => src.User != null && src.User.Reference != null ? src.User.Reference.NameAr : string.Empty)
                .Map(dest => dest.UserReferenceNameEn, src => src.User != null && src.User.Reference != null ? src.User.Reference.NameEn : string.Empty)
                .Map(dest => dest.IdCardNumber, src => src.User != null ? src.User.IdCardNumber : string.Empty)
                 .Config;
        }
    }
}
