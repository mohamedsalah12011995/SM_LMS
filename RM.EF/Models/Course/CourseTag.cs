
using RM.Models;

namespace RM.EF.Models.Course
{
    public class CourseTag
    {
        public int Id { get; set; }

        public string NameAr { get; set; } = string.Empty;

        public string NameEn { get; set; } = string.Empty;

        public int? ReferenceId { get; set; }
        public virtual Reference Reference { get; set; }

        public ICollection<CourseTagMapping> CourseTagMappings { get; set; }
            = new List<CourseTagMapping>();
    }
}
