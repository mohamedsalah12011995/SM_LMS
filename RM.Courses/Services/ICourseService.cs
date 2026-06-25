using RM.Courses.Dtos;
using RM.Courses.Dtos.OperationOutput;
using RM.Courses.Records;

namespace RM.Courses.Services
{
    public interface ICourseService
    {
        Task<OperationOutput> GetCourseseList(CourseDto RequestedData);
        Task<OperationOutput> GetCourseseDetails(CourseDto RequestedData);
        Task<OperationOutput> SaveCourse(CourseDto RequestedData);
        Task<OperationOutput> UpdateCourseById(CourseDto RequestedData);
        Task<OperationOutput> ToggleCourseStatus(CourseDto RequestedData);

        Task<OperationOutput> DeleteCourse(CourseDto RequestedData);
        OperationOutput ModelActions(CourseDto RequestedData);

        Task<OperationOutput> GetAllCategories(CourseCategoryDto RequestedData);
        Task<OperationOutput> SaveCategories(SaveCourseCategoryRecord RequestedData);

        Task<OperationOutput> GetAllInstructors(InstructorDto RequestedData);
        Task<OperationOutput> SaveInstructor(SaveInstructorRecord RequestedData);

        Task<OperationOutput> GetAllTags(CourseTagDto RequestedData);
        Task<OperationOutput> SaveTag(SaveTagRecord RequestedData);

        Task<OperationOutput> SaveCourseLearningOutcomes(CourseDto RequestedData);
        Task<OperationOutput> SaveCoursePrerequisites(CourseDto RequestedData);
        Task<OperationOutput> SaveCourseTargetAudiences(CourseDto RequestedData);



    }
}
