#nullable disable

using RM.EF.Models.Course;
using RM.Models;

public class CourseTagMapping
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int TagId { get; set; }

        public Course Course { get; set; } = null!;

        public CourseTag Tag { get; set; } = null!;
    }

