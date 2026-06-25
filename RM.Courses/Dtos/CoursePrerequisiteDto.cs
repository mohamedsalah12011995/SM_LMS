namespace RM.Courses.Dtos
{
    public class CoursePrerequisiteDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
    }
}
