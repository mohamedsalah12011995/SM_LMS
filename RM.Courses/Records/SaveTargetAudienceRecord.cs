namespace RM.Courses.Records
{
    public record SaveTargetAudienceRecord(
         int? Id,
         int CourseId,
         string TitleAr,
         string TitleEn
     );
}
