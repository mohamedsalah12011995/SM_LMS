namespace RM.Courses.Records
{
    public record SaveSectionRecord(
         int? Id,
         int CourseId,
         string TitleAr,
         string TitleEn,
         int SortOrder
     );
}
