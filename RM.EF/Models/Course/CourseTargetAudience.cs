
#nullable disable

using RM.Models;

public class CourseTargetAudience
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string TitleAr { get; set; } = string.Empty;

        public string TitleEn { get; set; } = string.Empty;

        public Course Course { get; set; } = null!;
    }

