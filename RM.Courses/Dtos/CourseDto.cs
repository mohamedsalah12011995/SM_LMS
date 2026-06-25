namespace RM.Courses.Dtos
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? ImageUrl { get; set; }
        public string? IntroVideoUrl { get; set; }
        public int CategoryId { get; set; }
        public int Level { get; set; }
        public string LanguageCode { get; set; } = "ar";
        public int EstimatedDurationMinutes { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public bool AllowCertificate { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }

        public int EntityId { get; set; }
        public int ReferenceId { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }

        public int? SortOrder { get; set; }
        public int? CourseId { get; set; }

    }
}
