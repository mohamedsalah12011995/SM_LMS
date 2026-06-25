namespace RM.Courses.Records
{
    public record SaveLearningOutcomeRecord(
         int? Id,
         int CourseId,
         string TitleAr,
         string TitleEn,
         int SortOrder
     );
}
