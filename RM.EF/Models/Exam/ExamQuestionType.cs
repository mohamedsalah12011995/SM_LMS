
#nullable disable

using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{

    [Table("QuestionType", Schema = "Exams")]
    public class ExamQuestionType
    {

        public int Id { get; set; }
        public string TextAr { get; set; }
        public string TextEn { get; set; }
        public string Type { get; set; }
        public bool? HasDataSource { get; set; }
    }

}
