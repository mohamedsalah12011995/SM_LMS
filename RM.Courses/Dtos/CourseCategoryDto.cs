namespace RM.Courses.Dtos
{
    public class CourseCategoryDto
    {
        public int Id { get; set; }
        public string NameAr { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? ImageUrl { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }

        public int? ReferenceId { get; set; }


    }
}
