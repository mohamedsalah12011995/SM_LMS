

using System.ComponentModel.DataAnnotations;

namespace RM.Models
{
    public class DynamicFormAdvanceSearch
    {
        [Key]
        public int FormValueDetailId { get; set; }
        public int Id { get; set; }
        public int EntityId { get; set; }
        public int FormId { get; set; }
        public string CreatedByUser { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedByUser { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int FormValueId { get; set; }
        public int InputKey { get; set; }
        public string InputValue { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public string Property { get; set; }
        public int Type { get; set; }
    }
}
