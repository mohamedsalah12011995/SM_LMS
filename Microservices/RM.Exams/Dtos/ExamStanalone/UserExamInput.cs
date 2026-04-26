using Mapster;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos.ExamStanalone
{
    public class UserExamInput
    {
        [JsonIgnore]
        public int? ReferenceId { get; set; }// portal Id
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }

        [JsonIgnore]
        public int? EntityId { get; set; }

        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get(EntityId); } }

        [JsonIgnore]
        public int? ExamId { get; set; }
        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get<int?>(ExamId); } }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        [JsonIgnore]
        public int? CertificateThemeId { get; set; }
        public string certificateThemeId { set { CertificateThemeId = Accessor.Set(value); } get { return Accessor.Get<int?>(CertificateThemeId); } }
        public List<UserData> Users { get; set; }

        public TypeAdapterConfig AddConfig(int createdUserId, int userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<UserExamInput, Models.UserApplicationExam>().IgnoreNullValues(true)
                .Map(dest => dest.UserId, src => userId)
                .Map(dest => dest.CreatedBy, src => createdUserId)
                .Map(dest => dest.CreatedDate, src => currentDate)

                .Config;
        }


    }

    public class UserData
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

    }



}
