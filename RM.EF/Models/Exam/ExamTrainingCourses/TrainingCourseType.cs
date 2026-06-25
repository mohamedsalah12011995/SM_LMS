#nullable disable

using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("TrainingCourseTypes", Schema = "ExamTraining")]

    public class TrainingCourseType
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
