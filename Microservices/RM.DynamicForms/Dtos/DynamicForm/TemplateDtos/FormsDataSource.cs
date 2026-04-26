using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class FormsDataSource : BaseDto<FormsDataSource, RM.Models.FormsDataSource>
    {
        public FormsDataSource()
        {
            SubDataSource = new List<FormsDataSource>();
        }
        [JsonIgnore]
        public int? Id { get; set; }
        [JsonIgnore]
        public int? FormInputDatasourceId { get; set; }
        [JsonIgnore]
        public int? ParentId { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public bool? IsDeleted { get; set; }

        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string formInputDatasourceId { set { FormInputDatasourceId = Accessor.Set(value); } get { return Accessor.Get(FormInputDatasourceId); } }
        public string parentId { set { ParentId = Accessor.Set(value); } get { return Accessor.Get(ParentId); } }

        public List<FormsDataSource> SubDataSource { get; set; }


        public TypeAdapterConfig AddConfig(FormInput input)
        {
            return new TypeAdapterConfig()
                .NewConfig<FormsDataSource, RM.Models.FormsDataSource>().IgnoreNullValues(true)
                .Map(dest => dest.IsDeleted, src => false)
               .Map(dest => dest.TextAr, src => !string.IsNullOrEmpty(input.FormsDataSource.TextAr) ? input.FormsDataSource.TextAr : string.Empty)
               .Map(dest => dest.TextEn, src => !string.IsNullOrEmpty(input.FormsDataSource.TextEn) ? input.FormsDataSource.TextEn : string.Empty)

                .Config;
        }

        public TypeAdapterConfig UpdateConfig(FormInput input)
        {
            return new TypeAdapterConfig()
                .NewConfig<FormsDataSource, RM.Models.FormsDataSource>().IgnoreNullValues(true)
                .Map(dest => dest.TextAr, src => !string.IsNullOrEmpty(input.FormsDataSource.TextAr) ? input.FormsDataSource.TextAr : string.Empty)
               .Map(dest => dest.TextEn, src => !string.IsNullOrEmpty(input.FormsDataSource.TextEn) ? input.FormsDataSource.TextEn : string.Empty)

                .Config;
        }

        public static TypeAdapterConfig SelectConfig(RM.Models.FormInput input)
        {
            return new TypeAdapterConfig()
                .NewConfig<RM.Models.FormsDataSource, FormsDataSource>()
                .Map(dest => dest.Id, src => input.FormInputDataSource.Any() ? input.FormInputDataSource.Select(ds => ds.DataSource != null ? ds.DataSource.ParentId : null).FirstOrDefault() : null)


                .Config;

        }



    }
}
