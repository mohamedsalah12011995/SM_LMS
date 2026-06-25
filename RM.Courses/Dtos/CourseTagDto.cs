namespace RM.Courses.Dtos
{
    public class CourseTagDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public int? ReferenceId { get; set; }
    }
}
