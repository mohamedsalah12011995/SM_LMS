using Mapster;
using RM.Core.CommonDtos;
using RM.Core.Helpers;
using RM.DynamicForms.Dtos.DynamicForm.TemplateDtos;
using System.Text;
using System.Text.Json.Serialization;

namespace RM.DynamicForms.Dtos.DynamicForm.FormModuleDtos
{

    public class AdvancedSearchDto
    {

        [JsonIgnore]
        public int? _formId { get; set; }
        public string FormId { set { _formId = Accessor.Set(value); } get { return Accessor.Get(_formId); } }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<AdvancedSearch> AdvancedSearchList { get; set; } = new List<AdvancedSearch>();
        public string _advancedSearchList { get { return AdvancedSearchList != null ? ConvertListToStringDynamicForm(AdvancedSearchList.Select(x => new AdvancedSearch { _key = x._key, CompareValue = x.CompareValue, CompareType = x.CompareType }).ToList()) : null; } }
        public static string ConvertListToStringDynamicForm(List<AdvancedSearch> searchData, string Splitter = "$")
        {
            var result = new StringBuilder();
            for (int i = 0; i < searchData.Count; i++)
            {
                searchData[i].CompareValue = searchData[i].ItemType != "hijriDate" ? searchData[i].CompareValue
                    : Dates.ConvertHijriStringDateToGeorogian(Convert.ToString(searchData[i].CompareValue).Replace("ValueKind = String :", ""));

                result.Append(string.Join('|', searchData[i]._key, searchData[i].CompareValue, searchData[i].CompareType));
                if (i < searchData.Count - 1)
                    result.Append(Splitter);
            }

            return result.ToString();
        }
        public ApplicationOperation.Pagination Pagination { get; set; }


    }

    public class AdvancedSearch
    {
        [JsonIgnore]
        public int? _key { get; set; }
        public string Key { set { _key = Accessor.Set(value); } get { return Accessor.Get(_key); } }
        public object CompareValue { get; set; }
        public string CompareType { get; set; }//Oprerator
        public string ItemType { get; set; }

    }
    public class KeysResult : BaseDto<KeysResult, Models.FormInput>
    {
        [JsonIgnore]
        public int? _key { get; set; }
        public string Key { set { _key = Accessor.Set(value); } get { return Accessor.Get(_key); } }
        [JsonIgnore]
        public int? Type { get; set; }
        public string type { set { Type = Accessor.Set(value); } get { return Accessor.Get(Type); } }

        public string LabelAr { get; set; }
        public string LabelEn { get; set; }
        public string TypeName { get; set; }
        [JsonIgnore]
        public bool? HasDataSourceFromAPI { get; set; }
        [JsonIgnore]
        public string DataSourceAPIRouting { get; set; }
        [JsonIgnore]
        public string APIParameters { get; set; }
        public bool? IsUnique { get; set; }

        public List<FieldOptions> Options { get; set; } = new List<FieldOptions>();

        public static TypeAdapterConfig SelectConfig()
        {
            return new TypeAdapterConfig()
                .NewConfig<Models.FormInput, KeysResult>()
                .Map(dest => dest._key, src => src.Id)
                // .Map(dest => dest.Type, src => src.Type)
                .Map(dest => dest.TypeName, src => src.InputsType != null ? src.InputsType.Type : string.Empty)
                .Map(dest => dest.LabelAr, src => src.NameAr)
                .Map(dest => dest.LabelEn, src => src.NameEn)
                .Map(dest => dest.Options, src => src.FormInputDataSource.Any() ? src.FormInputDataSource.OrderBy(c => c.Id).Adapt<List<FieldOptions>>(FieldOptions.SelectConfig()) : new List<FieldOptions>())

                .Config;

        }

    }
}
