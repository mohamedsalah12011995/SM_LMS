#nullable disable

using System.ComponentModel.DataAnnotations.Schema;


namespace RM.Models
{
    [Table("FormValueDetails", Schema = "DynamicForm")]
    public class FormValueDetails
    {
        public int Id { get; set; }
        public int? InputKey { get; set; }
        public string InputValue { get; set; }
        public int? InputTypeId { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public decimal? NumericValue { get; set; }
        public bool? BooleanValue { get; set; }
        public int? FormValueId { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }

        public virtual FormValue FormValue { get; set; }
        public virtual FormInput FormInput { get; set; }
        public virtual InputsType InputType { get; set; }
    }
}
