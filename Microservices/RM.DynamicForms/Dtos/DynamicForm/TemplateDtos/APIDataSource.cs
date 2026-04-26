using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class APIDataSource : BaseDto<APIDataSource, Models.APIDataSource>
    {
        [JsonIgnore]
        public int? Id { get; set; }
        public string DisplayMthod { get; set; }
        public string SourceMethod { get; set; }
        public string Parameter { get; set; }
        public bool ParamTypeIsObject { get; set; }
        public string ParameterDataListMethodName { get; set; }
        public string ParameterControlTypeName { get; set; }
        public string OnChangeAPIMethodName { get; set; }
        public string OnChangeParameterName { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }

        public List<APIDataSourceParamObjectDetail> APIParamObjectDetails { get; set; } = new List<APIDataSourceParamObjectDetail>();

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.APIDataSource, APIDataSource>()

                .Map(dest => dest.APIParamObjectDetails, src => src.APIParamObjectDetail.Any() ? src.APIParamObjectDetail.Adapt<List<APIDataSourceParamObjectDetail>>() : new List<APIDataSourceParamObjectDetail>())
                .Config;
        }

        public TypeAdapterConfig AddAndUpdateConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<APIDataSource, Models.APIDataSource>()
                .IgnoreNullValues(true)

                .Map(dest => dest.APIParamObjectDetail, src => src.APIParamObjectDetails.Any() ? src.APIParamObjectDetails.Adapt<List<Models.APIDataSourceParamObjectDetail>>() : new List<Models.APIDataSourceParamObjectDetail>())
                .Config;
        }




    }
}
