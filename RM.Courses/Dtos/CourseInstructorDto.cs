namespace RM.Courses.Dtos
{
    public class CourseInstructorDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int InstructorId { get; set; }
        public bool IsPrimary { get; set; } 
    }
}
