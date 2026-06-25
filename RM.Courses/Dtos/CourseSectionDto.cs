namespace RM.Courses.Dtos
{
    public class CourseSectionDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
    }
}
