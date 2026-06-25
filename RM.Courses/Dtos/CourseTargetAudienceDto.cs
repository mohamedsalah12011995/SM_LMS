namespace RM.Courses.Dtos
{
    public class CourseTargetAudienceDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
    }
}
