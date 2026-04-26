using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos
{
    public class Exam : BaseDto<Exam, Models.Exam>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? ReferenceId { get; set; }
        [JsonIgnore]
        public int? EntityId { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        [JsonIgnore]
        public int? ActivatedBy { get; set; }
        [JsonIgnore]
        public int? UpdatedBy { get; set; }
        [JsonIgnore]
        public int? CertificateId { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }
        public string referenceId { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get<int?>(ReferenceId); } }
        public string entityId { set { EntityId = Accessor.Set(value); } get { return Accessor.Get<int?>(EntityId); } }
        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }
        public string activatedBy { set { ActivatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(ActivatedBy); } }
        public string updatedBy { set { UpdatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdatedBy); } }
        public string certificateId { set { CertificateId = Accessor.Set(value); } get { return Accessor.Get<int?>(CertificateId); } }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? DeletedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }
        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }
        public string StatusString { get; set; }
        public string StatusStringEn { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }
        public string Code { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public int? Duration { get; set; }
        public int? TotalMark { get; set; }
        public int? SuccessMark { get; set; }
        public int? DistributionGradeMethod { get; set; }
        public List<ExamQuestion> ExamQuesions { get; set; } = new List<ExamQuestion>();
        public ApplicationOperation.Pagination Pagination { get; set; }

        public ExpressionStarter<Models.Exam> Filteration()
        {
            var filter = PredicateBuilder.New<Models.Exam>(true);
            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(TitleAr))
                filter.And(u => u.TitleAr.Contains(TitleAr));
            if (!string.IsNullOrEmpty(TitleEn))
                filter.And(u => u.TitleEn.Contains(TitleEn));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.ReferenceId == ReferenceId);

            filter.And(u => u.EntityId == EntityId);

            filter.And(u => u.IsDeleted != true);
            return filter;

        }

        public static TypeAdapterConfig SelectConfig(bool? IsActive)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.Exam, Dtos.Exam>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير مفعل")
                .Map(dest => dest.StatusStringEn, src => src.IsActive == true ? "Active" : "Not activated")
                .Map(dest => dest.ExamQuesions, src => src.ExamQuestions != null ? src.ExamQuestions.Where(x => x.IsDeleted != true).OrderBy(o => o.Order ?? o.Id).ToList().Adapt<List<Dtos.ExamQuestion>>(Dtos.ExamQuestion.SelectConfig(IsActive)) : new List<ExamQuestion>())
                .Config;
        }

        public TypeAdapterConfig UpdateConfig(int userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.Exam, Models.Exam>()
                .IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => currentDate)
                .Ignore(dest => dest.ExamQuestions)

                .Config;
        }

        public TypeAdapterConfig AddConfig(int userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.Exam, Models.Exam>()
                .IgnoreNullValues(true)
                .Map(dest => dest.Code, src => Strings.RandomDigits(10).ToString())
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => currentDate)
                .Map(dest => dest.IsDeleted, src => false)
                .Ignore(dest => dest.ExamQuestions)

                .Config;
        }

    }
}
