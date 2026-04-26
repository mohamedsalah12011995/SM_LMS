using Microsoft.EntityFrameworkCore.Storage;
using MapsterMapper;
using RM.Core.Interfaces;
using RM.Models;
using RM.Core.Repositories;
using Microsoft.Extensions.Caching.Memory;
using static RM.Core.Helpers.Enums;



namespace RM.Feedbacks.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IMemoryCache MemoryCache { get; private set; }

        public IConfiguration Configuration { get; private set; } 
        public IBaseRepository<Models.Feedbacks> Feedbacks { get; private set; }
        public IBaseRepository<Models.FeedbacksDataSource> FeedbacksDataSources { get; private set; }
        public IBaseRepository<Models.FeedbacksAnswerAction> FeedbacksAnswerActions { get; private set; }
        public IBaseRepository<Models.FeedbacksAnswer> FeedbacksAnswers { get; private set; }

        public IBaseRepository<Recommendations> Recommendations { get; private set; }


        public IBaseRepository<Advertisement> Advertisements { get; private set; }
        public IBaseRepository<Article> Articles { get; private set; }
        public IBaseRepository<Eservice> Eservices { get; private set; }
        public IBaseRepository<GovService> GovServices { get; private set; }
        public IBaseRepository<News> News { get; private set; }
        public IBaseRepository<Entity> Entities { get; private set; }
        public IBaseRepository<CronSettings> CronSettings { get; private set; }

        public IBaseRepository<Models.ScientificLetters> ScientificLetters { get; private set; }
        public IBaseRepository<Models.Partner> Partners { get; private set; }
        public IBaseRepository<Official> Officials { get; private set; }
        public IBaseRepository<Models.Multimedia> Multimedia { get; private set; }
        public IBaseRepository<JobAdvertisement> JobAdvertisement { get; private set; }
        public IBaseRepository<JobCareer> JobCareers { get; private set; }
        public IBaseRepository<Models.FAQ> FAQs { get; private set; }
        public IBaseRepository<Document> Documents { get; private set; }
        public IBaseRepository<Idea> Ideas { get; private set; }

        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration , IMemoryCache memoryCache)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            Feedbacks = new BaseRepository<Models.Feedbacks>(context);
            FeedbacksDataSources = new BaseRepository<Models.FeedbacksDataSource>(context);
            FeedbacksAnswerActions = new BaseRepository<Models.FeedbacksAnswerAction>(context);
            FeedbacksAnswers = new BaseRepository<Models.FeedbacksAnswer>(context);
            Recommendations = new BaseRepository<Models.Recommendations>(context);
            Advertisements = new BaseRepository<Models.Advertisement>(context);
            Articles = new BaseRepository<Models.Article>(context);
            Eservices = new BaseRepository<Eservice>(context);
            GovServices = new BaseRepository<GovService>(context);
            News = new BaseRepository<News>(context);
            Entities = new BaseRepository<Entity>(context);
            CronSettings = new BaseRepository<CronSettings>(context);
            MemoryCache = memoryCache;

            ScientificLetters = new BaseRepository<ScientificLetters>(context);
            Partners = new BaseRepository<Models.Partner>(context);
            Officials = new BaseRepository<Official>(context);
            Multimedia = new BaseRepository<Multimedia>(context);
            JobAdvertisement = new BaseRepository<JobAdvertisement>(context);
            JobCareers = new BaseRepository<JobCareer>(context);
            FAQs = new BaseRepository<FAQ>(context);
            Documents = new BaseRepository<Document>(context);
            Ideas = new BaseRepository<Idea>(context);

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