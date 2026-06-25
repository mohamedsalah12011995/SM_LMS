using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Courses.Models;
using RM.EF.Models.Course;
using RM.Models;
using Course = RM.Models.Course;




namespace RM.Courses.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IBaseRepository<Course> Courses { get; }
        IBaseRepository<CourseLesson> CourseLessons { get; }
        IBaseRepository<CourseLessonMaterial> CourseLessonMaterials { get; }
        public IBaseRepository<CourseCategory> CourseCategorys { get; }
        public IBaseRepository<CourseInstructor> CourseInstructors { get; }
        public IBaseRepository<CourseTargetAudience> CourseTargetAudiences { get; }
        public IBaseRepository<CourseTagMapping> CourseTagMappings { get; }
        public IBaseRepository<CourseLearningOutcome> CourseLearningOutcomes { get; }
        public IBaseRepository<CoursePrerequisite> CoursePrerequisites { get;    }
        public IBaseRepository<CourseSection> CourseSections { get; }
        public IBaseRepository<CourseTag> CourseTags { get;  }
        public IBaseRepository<Instructor> Instructors { get; }
        IConfiguration Configuration { get; }
        IDbContextTransaction BeginTransaction();

        int Complete();
        Task<int> CompleteAsync();
    }
}