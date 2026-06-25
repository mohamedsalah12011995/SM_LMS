#nullable disable

using RM.Courses.Models;
using RM.EF.Enums;

namespace RM.Models;

public class CourseLessonMaterial
{
    public int Id { get; set; }

    public int CourseLessonId { get; set; }

    public string TitleAr { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;

    public string FileUrl { get; set; } = string.Empty;

    public MaterialType FileType { get; set; }

    public long FileSize { get; set; }

    public int SortOrder { get; set; }


    public CourseLesson CourseLesson { get; set; } = null!;
}