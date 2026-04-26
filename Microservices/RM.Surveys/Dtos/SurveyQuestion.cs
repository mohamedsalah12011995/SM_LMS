using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;


namespace RM.Surveys.Dtos
{
    public class SurveyQuestion : BaseDto<SurveyQuestion, Models.SurveyQuestion>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? CreatedBy { get; set; }

        [JsonIgnore]
        public int? UpdateBy { get; set; }
        [JsonIgnore]
        public int? DeletedBy { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public string createdById { set { CreatedBy = Accessor.Set(value); } get { return Accessor.Get(CreatedBy); } }
        public string updatedById { set { UpdateBy = Accessor.Set(value); } get { return Accessor.Get(UpdateBy); } }

        public string deletedById { set { DeletedBy = Accessor.Set(value); } get { return Accessor.Get(DeletedBy); } }


        [JsonIgnore]
        public int? TypeId { get; set; }
        public string typeQuestionId { set { TypeId = Accessor.Set(value); } get { return Accessor.Get(TypeId); } }

        public string Type { get; set; }
        public string TypeAr { get; set; }
        public string TypeEn { get; set; }

        [JsonIgnore]
        public int? SurveyId { get; set; }
        public string surveyID { set { SurveyId = Accessor.Set(value); } get { return Accessor.Get(SurveyId); } }


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
        public string GroupId { get; set; }

        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsGlobal { get; set; }
        public bool? WithId { get; set; }

        public string StatusString { get; set; }
        public int? SubQuestionOrder { get; set; }
        public int? GroupOrder { get; set; }

        public int? MinValue { get; set; }
        public int? MaxValue { get; set; }
        public bool? IsFiltration { get; set; }

        public string Image { get; set; }
        public string ImageBase64 { get; set; }

        [JsonIgnore]
        public int? ReferenceId { get; set; }
        public string referenceID { set { ReferenceId = Accessor.Set(value); } get { return Accessor.Get(ReferenceId); } }
        public bool? IsNoteRequired { get; set; }

        public List<SurveyDataSource> SurveyDataSources { get; set; } = new List<SurveyDataSource>();

        public QuestionsRecommendations QuestionsRecommendations { get; set; } 

        public static TypeAdapterConfig SelectConfig(string imagesGetPath)
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.SurveyQuestion, SurveyQuestion>()
                .Map(dest => dest.TextEn, src => src.TextEn)
                .Map(dest => dest.Type, src => src.QuestionType != null ? src.QuestionType.Type : string.Empty)
                .Map(dest => dest.TypeAr, src => src.QuestionType != null ? src.QuestionType.TextAr : string.Empty)
                .Map(dest => dest.TextEn, src => src.QuestionType != null ? src.QuestionType.TextEn : string.Empty)
                .Map(dest => dest.Image, src => !string.IsNullOrEmpty(src.Image) ? $"{imagesGetPath}/{src.Image}" : $"{imagesGetPath}/noImage.png")
                .Config;
        }

    }
}
