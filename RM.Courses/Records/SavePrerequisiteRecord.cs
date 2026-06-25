namespace RM.Courses.Records
{
    public record SavePrerequisiteRecord(
         int? Id,
         int CourseId,
         string TitleAr,
         string TitleEn
     );
}
