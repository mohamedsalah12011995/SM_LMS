namespace RM.Courses.Records;

public record SaveCourseCategoryRecord(
         int? Id,
        string NameAr,
        string NameEn,
        string? DescriptionAr,
        string? DescriptionEn,
        string? ImageUrl,
        int? ParentCategoryId,
        int ReferenceId

);