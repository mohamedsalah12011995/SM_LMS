namespace RM.Courses.Dtos
{
    public class CoursLessoneMaterialDto
    {
        public int Id { get; set; }
        public int CourseLessonId { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public int FileType { get; set; }
        public long FileSize { get; set; }
        public int SortOrder { get; set; }
    }
}
