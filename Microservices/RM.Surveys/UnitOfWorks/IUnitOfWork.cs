using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Surveys.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IMemoryCache MemoryCache { get; }
        IConfiguration Configuration { get; }
        IBaseRepository<Reference> References { get; }
        IBaseRepository<Survey> Surveys { get; }
        IBaseRepository<SurveyResult> SurveyResult { get; }

        IBaseRepository<SurveyQuestion> SurveyQuestions { get; }
        IBaseRepository<SurveyDataSource> SurveyDataSources { get; }
        IBaseRepository<SurveyAnswerAction> SurveyAnswerActions { get; }
        IBaseRepository<SurveyQuestionAnswer> SurveyQuestionAnswers { get; }
        IBaseRepository<SurveyQuestionType> SurveyQuestionTypes { get; }
        IBaseRepository<SurveyTheme> SurveyThemes { get; }
        IBaseRepository<PublishEntities> PublishEntities { get; }
        IBaseRepository<MajorLookup> MajorLookups { get; }
        IBaseRepository<QuestionsRecommendations> QuestionsRecommendations { get; }
        IBaseRepository<Recommendations> SurveyRecommendations { get; }
        IBaseRepository<CronSettings> CronSettings { get; }
        IBaseRepository<User> Users { get; }
        IDbContextTransaction BeginTransaction();

        public IEnumerable<EntitiesLatestUpdate> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter);


        int Complete();
        Task<int> CompleteAsync();
    }
}