using System.ComponentModel.DataAnnotations.Schema;

namespace RM.Models
{
    [Table("FormValueViewStatistic", Schema = "DynamicForm")]

    public class FormValueViewStatistic
    {
        public int Id { get; set; }
        public int? FormValueId { get; set; }
        public int? EntityId { get; set; }
        public string TextValue { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int? UserReferenceId { get; set; }
        public string UserReferenceNameAr { get; set; }
        public string UserReferenceNameEn { get; set; }
        public DateTime? ViewDate { get; set; }
    }
}
