#nullable disable

using RM.Courses.Models;
using RM.Models;

public class CourseSection
{
    public int Id { get; set; }

    public int CourseId { get; set; }

    public string TitleAr { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;

    public string? DescriptionAr { get; set; }
    public string? DescriptionEn { get; set; }

    public int SortOrder { get; set; }

    public Course Course { get; set; } = null!;

    public ICollection<CourseLesson> CourseLessons { get; set; }
        = new List<CourseLesson>();
}
