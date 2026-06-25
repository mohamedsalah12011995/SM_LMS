#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("QuestionType", Schema = "Survey")]
    public partial class SurveyQuestionType
    {

        public int Id { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string Type { get; set; }
        public bool? HasDataSource { get; set; }
        public string? Icon { get; set; }

    }
}
