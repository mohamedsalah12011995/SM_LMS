#nullable disable

using RM.EF.Enums;
using RM.Models;


namespace RM.Courses.Models
{
    public class CourseLesson
    {
        public int Id { get; set; }

        public int CourseSectionId { get; set; }

        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;

        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }

        public string? VideoUrl { get; set; }

        public string? ThumbnailUrl { get; set; }

        public int DurationMinutes { get; set; }

        public int SortOrder { get; set; }

        public bool IsPreview { get; set; }

        public CourseStatus Status { get; set; }

        public CourseSection CourseSection { get; set; } = null!;

        public ICollection<CourseLessonMaterial> Materials { get; set; }
            = new List<CourseLessonMaterial>();
    }
}
