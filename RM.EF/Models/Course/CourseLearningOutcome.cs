#nullable disable

using RM.Models;

public class CourseLearningOutcome
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public string TitleAr { get; set; } = string.Empty;

        public string TitleEn { get; set; } = string.Empty;

        public int SortOrder { get; set; }

        public Course Course { get; set; } = null!;
    }

