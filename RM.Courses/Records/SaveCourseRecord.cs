namespace RM.Courses.Records;

public record SaveCourseRecord(
    int? Id,
        string Code,
        string TitleAr,
        string TitleEn,
        string? DescriptionAr,
        string? DescriptionEn,
        string? ImageUrl,
        string? IntroVideoUrl,
        int CategoryId,
        int Level,
        string LanguageCode,
        int EstimatedDurationMinutes,
        int Status,
        int EntityId,
        int ReferenceId,
        int? CreatedBy,
        int? UpdatedBy
);