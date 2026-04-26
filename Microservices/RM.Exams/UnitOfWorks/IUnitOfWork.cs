using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Exams.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Exam> Exams { get; }
        IBaseRepository<ExamQuestion> ExamQuestions { get; }
        IBaseRepository<ExamDataSource> ExamDataSources { get; }
        IBaseRepository<ExamAnswerAction> ExamAnswerActions { get; }
        IBaseRepository<ExamQuestionAnswer> ExamQuestionAnswers { get; }
        IBaseRepository<ExamQuestionType> ExamQuestionTypes { get; }
        IBaseRepository<JobApplicationExams> JobApplicationExams { get; }
        IBaseRepository<MajorLookup> MajorLookup { get; }
        IBaseRepository<User> User { get; }
        IBaseRepository<TrainingCourse> TrainingCourses { get; }
        IBaseRepository<TrainingCourseSchedule> TrainingCourseSchedule { get; }
        IBaseRepository<TrainingCourseType> TrainingCourseType { get; }
        IBaseRepository<Reference> References { get; }
        IBaseRepository<InternalCourseTrainees> InternalCourseTrainees { get; }
        IBaseRepository<ExternalCourseTraniees> ExternalCourseTraniees { get; }
        IBaseRepository<InternalCourseExams> InternalCourseExams { get; }
        IBaseRepository<ExternalCourseExams> ExternalCourseExams { get; }
        IBaseRepository<CourseAdvertisement> CourseAdvertisement { get; }
        IBaseRepository<AdvertisementsCourses> AdvertisementsCourses { get; }
        IBaseRepository<ExamExternalTranieesAnswerAction> ExamExternalTranieesAnswerAction { get; }
        IBaseRepository<ExamExternalTranieesQuestionAnswer> ExamExternalTranieesQuestionAnswer { get; }
        IBaseRepository<ExamInternalTranieesAnswerAction> ExamInternalTranieesAnswerAction { get; }
        IBaseRepository<ExamInternalTranieesQuestionAnswer> ExamInternalTranieesQuestionAnswer { get; }
        IBaseRepository<Certificate> Certificates { get; }
        IBaseRepository<CertificateThemes> CertificateThemes { get; }
        IBaseRepository<UserApplicationExam> UserApplicationExam { get; }
        IBaseRepository<ExamUserApplicationAnswerAction> ExamUserApplicationAnswerAction { get; }
        IBaseRepository<ExamUserApplicationQuestionAnswer> ExamUserApplicationQuestionAnswer { get; }
        IBaseRepository<CertificateLog> CertificateLog { get; }

        ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; }

        IConfiguration Configuration { get; }
        IMapper Mappers { get; }

        IDbContextTransaction BeginTransaction();
        IEnumerable<EntitiesLatestUpdate> GetFromProcdure(string procedureName, string procedureParams, params object[] parameter);
        Task<int> CompleteAsync();
        int Complete();
    }
}
