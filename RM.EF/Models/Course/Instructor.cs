
using RM.Models;

namespace RM.EF.Models.Course
{
    public class Instructor
    {
        public int Id { get; set; }

        public string FullNameAr { get; set; } = string.Empty;

        public string FullNameEn { get; set; } = string.Empty;

        public string? BioAr { get; set; }

        public string? BioEn { get; set; }

        public string? ImageUrl { get; set; }

        public int? ReferenceId { get; set; }
        public virtual Reference Reference { get; set; }

        public ICollection<CourseInstructor> CourseInstructors { get; set; }
            = new List<CourseInstructor>();
    }
}
