namespace RM.Courses.Records
{
    public record SaveInstructorRecord(
        int? Id,
        string FullNameAr,
        string FullNameEn,
        string? BioAr,
        string? BioEn,
        string? ImageUrl,
        int ReferenceId
    );
}
