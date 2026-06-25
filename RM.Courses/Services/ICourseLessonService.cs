using RM.Courses.Dtos;
using RM.Courses.Dtos.OperationOutput;

namespace RM.Courses.Services
{
    public interface ICourseLessonService
    {
        // --- الأقسام (Course Sections) ---
        Task<OperationOutput> GetSectionsList(CourseLessonDto RequestedData);
        Task<OperationOutput> SaveSection(CourseLessonDto RequestedData);
        Task<OperationOutput> UpdateSectionById(CourseLessonDto RequestedData);
        Task<OperationOutput> DeleteSection(CourseLessonDto RequestedData);

        // --- الدروس (Course Lessons) ---
        Task<OperationOutput> GetLessonsList(   CourseLessonDto RequestedData);
        Task<OperationOutput> GetLessonDetails(CourseLessonDto RequestedData);
        Task<OperationOutput> SaveLesson(CourseLessonDto RequestedData);
        Task<OperationOutput> UpdateLessonById(CourseLessonDto RequestedData);
        Task<OperationOutput> DeleteLesson(CourseLessonDto RequestedData);
    }
}
