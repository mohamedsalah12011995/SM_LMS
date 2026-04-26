using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;

namespace RM.Surveys.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IMemoryCache MemoryCache { get; private set; }

        public IConfiguration Configuration { get; private set; }
        public IBaseRepository<Reference> References { get; private set; }

        public IBaseRepository<Survey> Surveys { get; private set; }
        public IBaseRepository<SurveyResult> SurveyResult { get; private set; }
        public IBaseRepository<SurveyQuestion> SurveyQuestions { get; private set; }
        public IBaseRepository<SurveyDataSource> SurveyDataSources { get; private set; }
        public IBaseRepository<SurveyAnswerAction> SurveyAnswerActions { get; private set; }
        public IBaseRepository<SurveyQuestionAnswer> SurveyQuestionAnswers { get; private set; }
        public IBaseRepository<SurveyQuestionType> SurveyQuestionTypes { get; private set; }
        public IBaseRepository<SurveyTheme> SurveyThemes { get; private set; }
        public IBaseRepository<PublishEntities> PublishEntities { get; private set; }
        public IBaseRepository<MajorLookup> MajorLookups { get; private set; }
        public IBaseRepository<QuestionsRecommendations> QuestionsRecommendations { get; private set; }
        public IBaseRepository<Recommendations> SurveyRecommendations { get; private set; }
        public IBaseRepository<CronSettings> CronSettings { get; private set; }
        public IBaseRepository<User> Users { get; private set; }


        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            References = new BaseRepository<Reference>(context);
            Surveys = new BaseRepository<Survey>(context);
            SurveyResult = new BaseRepository<SurveyResult>(context);
            SurveyQuestions = new BaseRepository<SurveyQuestion>(context);
            SurveyDataSources = new BaseRepository<SurveyDataSource>(context);
            SurveyAnswerActions = new BaseRepository<SurveyAnswerAction>(context);
            SurveyQuestionAnswers = new BaseRepository<SurveyQuestionAnswer>(context);
            SurveyQuestionTypes = new BaseRepository<SurveyQuestionType>(context);
            SurveyThemes = new BaseRepository<SurveyTheme>(context);
            PublishEntities = new BaseRepository<PublishEntities>(context);
            MajorLookups = new BaseRepository<MajorLookup>(context);
            QuestionsRecommendations = new BaseRepository<QuestionsRecommendations>(context);
            CronSettings = new BaseRepository<CronSettings>(context);
            SurveyRecommendations = new BaseRepository<Recommendations>(context);
            MemoryCache = memoryCache;
            Users = new BaseRepository<User>(context);

        }


        public void Dispose()
        {
            _context.Dispose();
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
        public IEnumerable<EntitiesLatestUpdate> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter)
        {
            return _context.Set<EntitiesLatestUpdate>().FromSqlRaw(ProcedureName + ProcedureParams, Parameter);
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