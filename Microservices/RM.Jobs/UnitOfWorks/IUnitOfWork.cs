using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;


namespace RM.Jobs.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IBaseRepository<JobAdvertisement> JobAdvertisement { get; }
        IBaseRepository<JobCareer> JobCareers { get; }
        IBaseRepository<JobApplication> JobApplication { get; }
        IBaseRepository<JobLookUp> JobLookUp { get; }
        IBaseRepository<Exam> Exams { get; }
        IBaseRepository<ExamQuestion> ExamQuestions { get; }
        IBaseRepository<ExamDataSource> ExamDataSources { get; }
        IBaseRepository<ExamAnswerAction> ExamAnswerActions { get; }
        IBaseRepository<ExamQuestionAnswer> ExamQuestionAnswers { get; }
        IBaseRepository<ExamQuestionType> ExamQuestionTypes { get; }
        IBaseRepository<JobApplicationExams> JobApplicationExams { get; }
        IBaseRepository<MajorLookup> MajorLookup { get; }
        IBaseRepository<MajorLookupsType> MajorLookupsType { get; }

        IConfiguration Configuration { get; } 
        IDbContextTransaction BeginTransaction();
      
        int Complete();
        Task<int> CompleteAsync();
    }
}