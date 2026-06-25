namespace RM.Courses.Dtos
{
    public class CourseLessonDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int CourseSectionId { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? VideoUrl { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int DurationMinutes { get; set; }
        public int SortOrder { get; set; }
        public bool IsPreview { get; set; }
        public int Status { get; set; }
    }
}
