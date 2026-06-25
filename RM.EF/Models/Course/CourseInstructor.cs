#nullable disable

using RM.EF.Models.Course;
using RM.Models;

public class CourseInstructor
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int InstructorId { get; set; }

        public bool IsPrimaryInstructor { get; set; }

        public Course Course { get; set; } = null!;

        public Instructor Instructor { get; set; } = null!;
    }
