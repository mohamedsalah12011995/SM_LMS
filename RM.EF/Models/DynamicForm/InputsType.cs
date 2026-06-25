
#nullable disable

using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("InputsType", Schema = "DynamicForm")]
    public class InputsType
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Type { get; set; }
        public virtual ICollection<FormInput> FormInputs { get; set; } = new List<FormInput>();
    }
}
