#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("APIDataSourceParamObjectDetails", Schema = "DynamicForm")]
    public class APIDataSourceParamObjectDetail
    {
        public int Id { get; set; }
        public int APIDataSourceId { get; set; }
        public string ParamName { get; set; }
        public string DisplayParamNameAr { get; set; }
        public string DisplayParamNameEn { get; set; }
        public string ParameterDataListMethodName { get; set; }
        public string ParameterControlTypeName { get; set; }

        public APIDataSource APIDataSource { get; set; }

    }
}
