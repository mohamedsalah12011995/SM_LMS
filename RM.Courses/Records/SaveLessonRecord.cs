namespace RM.Courses.Records;

public record SaveLessonRecord(
    int? Id,
        int CourseSectionId,
        string TitleAr,
        string TitleEn,
        string? DescriptionAr,
        string? DescriptionEn,
        string? VideoUrl,
        string? ThumbnailUrl,
        int DurationMinutes,
        int SortOrder,
        bool IsPreview,
        int Status

);