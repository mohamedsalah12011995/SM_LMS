#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("APIDataSources", Schema = "DynamicForm")]
    public class APIDataSource
    {
        public int Id { get; set; }
        public string DisplayMthod { get; set; }
        public string SourceMethod { get; set; }
        public string? Parameter { get; set; }
        public bool ParamTypeIsObject { get; set; }
        public string? ParameterDataListMethodName { get; set; }
        public string? ParameterControlTypeName { get; set; }
        public string? OnChangeAPIMethodName { get; set; }
        public string? OnChangeParameterName { get; set; }
        public List<APIDataSourceParamObjectDetail> APIParamObjectDetail { get; set; } = new List<APIDataSourceParamObjectDetail>();

    }
}
