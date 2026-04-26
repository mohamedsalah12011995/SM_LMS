
using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RM.Jobs.Dtos
{
    public class ExamQuesion:BaseDto<ExamQuesion,Models.ExamQuestion>
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

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.ExamQuestion, ExamQuesion>()
                .Map(dest => dest.CreatedDateString, src => src.CreatedDate.HasValue ? src.CreatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.UpdatedDateString, src => src.UpdatedDate.HasValue ? src.UpdatedDate.Value.ToString("yyyy-MM-dd") : string.Empty)
                .Map(dest => dest.PersonCreatedBy, src => src.CreatedByNavigation != null ? src.CreatedByNavigation.Name : string.Empty)
                .Map(dest => dest.PersonUpdatedBy, src => src.UpdatedByNavigation != null ? src.UpdatedByNavigation.Name : string.Empty)
                .Map(dest => dest.Type, src => src.QuestionType != null ? src.QuestionType.Type : string.Empty)
 
                .Map(dest => dest.StatusString, src => src.IsActive == true ? "فعال" : "غير فعال")
                .Map(dest => dest.ExamDataSources, src => src.ExamDataSources != null ? src.ExamDataSources.Where(x => x.IsDeleted != true).OrderBy(o => o.Id).ToList().Adapt<List<Dtos.ExamDataSource>>(Dtos.ExamDataSource.SelectConfig()) : new List<ExamDataSource>())

                .Config;
        }

    }
}
