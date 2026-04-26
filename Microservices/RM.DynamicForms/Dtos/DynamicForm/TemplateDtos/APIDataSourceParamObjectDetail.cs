using RM.Core.CommonDtos;
using RM.Core.Helpers;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.TemplateDtos
{
    public class APIDataSourceParamObjectDetail : BaseDto<APIDataSourceParamObjectDetail, Models.APIDataSourceParamObjectDetail>
    {
        [JsonIgnore]
        public int? Id { get; set; }

        [JsonIgnore]
        public int? APIDataSourceId { get; set; }
        public string ParamName { get; set; }
        public string DisplayParamNameAr { get; set; }
        public string DisplayParamNameEn { get; set; }
        public string ParameterDataListMethodName { get; set; }
        public string ParameterControlTypeName { get; set; }
        public string ID { set { Id = Accessor.Set(value); } get { return Accessor.Get(Id); } }
        public string aPIDataSourceId { set { APIDataSourceId = Accessor.Set(value); } get { return Accessor.Get(APIDataSourceId); } }


    }
}
