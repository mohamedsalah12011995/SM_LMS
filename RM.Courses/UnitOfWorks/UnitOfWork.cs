using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Courses.Models;
using RM.EF.Models.Course;
using RM.Models;


namespace RM.Courses.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IBaseRepository<Course> Courses { get; private set; }
        public IBaseRepository<CourseLesson> CourseLessons { get; private set; }
        public IBaseRepository<CourseLessonMaterial> CourseLessonMaterials { get; private set; }
        public IBaseRepository<CourseCategory> CourseCategorys { get; private set; }
        public IBaseRepository<CourseInstructor> CourseInstructors { get; private set; }
        public IBaseRepository<CourseTargetAudience> CourseTargetAudiences { get; private set; }
        public IBaseRepository<CourseTagMapping> CourseTagMappings { get; private set; }
        public IBaseRepository<CourseLearningOutcome> CourseLearningOutcomes { get; private set; }
        public IBaseRepository<CoursePrerequisite> CoursePrerequisites { get; private set; }
        public IBaseRepository<CourseSection> CourseSections { get; private set; }
        public IBaseRepository<CourseTag> CourseTags { get; private set; }
        public IBaseRepository<Instructor> Instructors { get; private set; }
        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            Courses = new BaseRepository<Course>(context);
            CourseLessons = new BaseRepository<CourseLesson>(context);
            CourseLessonMaterials = new BaseRepository<CourseLessonMaterial>(context);
            CourseCategorys = new BaseRepository<CourseCategory>(context);
            CourseInstructors = new BaseRepository<CourseInstructor>(context);
            CourseTargetAudiences = new BaseRepository<CourseTargetAudience>(context);
            CourseTagMappings = new BaseRepository<CourseTagMapping>(context);
            CourseLearningOutcomes = new BaseRepository<CourseLearningOutcome>(context);
            CoursePrerequisites = new BaseRepository<CoursePrerequisite>(context);
            CourseSections = new BaseRepository<CourseSection>(context);
            CourseTags = new BaseRepository<CourseTag>(context);
            Instructors = new BaseRepository<Instructor>(context);

        }


        public void Dispose()
        {
            _context.Dispose();
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}