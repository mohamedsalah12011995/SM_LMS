namespace RM.Courses.Dtos
{
    public class InstructorDto
    {
        public int Id { get; set; }
        public string FullNameAr { get; set; } = string.Empty;
        public string FullNameEn { get; set; } = string.Empty;
        public string? BioAr { get; set; }
        public string? BioEn { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }

        public int ReferenceId { get; set; }
    }
}
