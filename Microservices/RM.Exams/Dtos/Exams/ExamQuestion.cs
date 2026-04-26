using LinqKit;
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.Exams.Dtos
{
    public class ExamQuestion : BaseDto<ExamQuestion, Models.ExamQuestion>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }

        [JsonIgnore]
        public int? UpdateBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get<int?>(Id); } }

        public string createdBy { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(CreatedBy); } }
        public string updatedBy { set { UpdateBy = Accessor.Set(value); } get { return Accessor.Get<int?>(UpdateBy); } }

        public string deletedBy { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get<int?>(DeletedBy); } }


        [JsonIgnore]
        public int? TypeId { get; set; }
        public string typeId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get<int?>(TypeId); } }

        public string Type { get; set; }

        [JsonIgnore]
        public int? ExamId { get; set; }
        public string examId { set { ExamId = Accessor.Set(value); } get { return Accessor.Get<int?>(ExamId); } }


        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public DateTime? DeletedDate { get; set; }

        public string CreatedDateString { get; set; }
        public string UpdatedDateString { get; set; }

        public string PersonCreatedBy { get; set; }
        public string PersonUpdatedBy { get; set; }


        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        public bool? Mandatory { get; set; }
        public bool? VerticalAnswersDirection { get; set; }
        public double? Mark { get; set; }

        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string StatusString { get; set; }

        public List<ExamDataSource> ExamDataSources { get; set; } = new List<ExamDataSource>();

        public ExpressionStarter<Models.ExamQuestion> Filteration()
        {
            var filter = PredicateBuilder.New<Models.ExamQuestion>(true);
            if (CreatedDate.HasValue)
                filter.And(u => u.CreatedDate == null || u.CreatedDate.Value.Date == CreatedDate.Value.Date);

            if (UpdatedDate.HasValue)
                filter.And(u => u.UpdatedDate == null || u.UpdatedDate.Value.Date == UpdatedDate.Value.Date);

            if (!string.IsNullOrEmpty(TextAr))
                filter.And(u => u.TextAr.Contains(TextAr));
            if (!string.IsNullOrEmpty(TextEn))
                filter.And(u => u.TextEn.Contains(TextEn));

            if (IsActive.HasValue)
                filter.And(u => u.IsActive == IsActive);

            filter.And(u => u.IsDeleted != true);
            return filter;
        }

        public static TypeAdapterConfig SelectConfig(bool? IsActive)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.ExamQuestion, Dtos.ExamQuestion>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير مفعل")
                .Map(dest => dest.Type, src => src.QuestionType != null ? src.QuestionType.Type : string.Empty)
                .Map(dest => dest.ExamDataSources, src => src.ExamDataSources != null ? src.ExamDataSources.Where(x => x.IsDeleted != true)
                .Where(x => IsActive.HasValue ? x.IsActive == true : true)
                .ToList().Adapt<List<Dtos.ExamDataSource>>(Dtos.ExamDataSource.SelectConfig()) : new List<ExamDataSource>())

                .Config;

        }

        public TypeAdapterConfig UpdateConfig(int userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.ExamQuestion, Models.ExamQuestion>()
                .IgnoreNullValues(true)
                .Map(dest => dest.UpdatedBy, src => userId)
                .Map(dest => dest.UpdatedDate, src => currentDate)
                .Ignore(dest => dest.ExamDataSources)
                .Config;
        }

        public TypeAdapterConfig AddConfig(int userId, DateTime currentDate)
        {
            return new TypeAdapterConfig()
                .NewConfig<Dtos.ExamQuestion, Models.ExamQuestion>()
                .IgnoreNullValues(true)
                .Map(dest => dest.CreatedBy, src => userId)
                .Map(dest => dest.CreatedDate, src => currentDate)
                .Map(dest => dest.IsDeleted, src => false)
                .Ignore(dest => dest.ExamDataSources)
                .Config;
        }


    }
}
