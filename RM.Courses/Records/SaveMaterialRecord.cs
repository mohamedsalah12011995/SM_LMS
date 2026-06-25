namespace RM.Courses.Records;

public record SaveMaterialRecord(
   int? Id,
        int CourseLessonId,
        string TitleAr,
        string TitleEn,
        string FileUrl,
        int FileType,
        long FileSize,
        int SortOrder
);