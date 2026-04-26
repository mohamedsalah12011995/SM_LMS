using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;

namespace RM.Statistics.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMemoryCache MemoryCache { get; private set; }

        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IBaseRepository<Rate> Rates { get; private set; }
        public IBaseRepository<Comment> Comments { get; private set; }

        public IBaseRepository<InteractionStatistic> InteractionStatistic { get; private set; }

        public IBaseRepository<GeneralNumbersResult> GeneralNumbersResult { get; private set; }
        public IBaseRepository<TotalVisitedByEntityResult> TotalVisitedByEntityResult { get; private set; }
        public IBaseRepository<StatisticsResult> StatisticsResult { get; private set; }
        public IBaseRepository<MajorLookup> MajorLookups { get; private set; }

        public IBaseRepository<Entity> Entities { get; private set; }
        public IBaseRepository<CronSettings> CronSettings { get; private set; }
        public IBaseRepository<InteractionStatisticsType> InteractionStatisticsTypes { get; private set; }

        public IBaseRepository<Advertisement> Advertisements { get; private set; }
        public IBaseRepository<Article> Articles { get; private set; }
        public IBaseRepository<Eservice> Eservices { get; private set; }
        public IBaseRepository<GovService> GovServices { get; private set; }
        public IBaseRepository<News> News { get; private set; }

        public IBaseRepository<Models.ScientificLetters> ScientificLetters { get; private set; }
        public IBaseRepository<Models.Partner> Partners { get; private set; }
        public IBaseRepository<Official> Officials { get; private set; }
        public IBaseRepository<Models.Multimedia> Multimedia { get; private set; }
        public IBaseRepository<JobAdvertisement> JobAdvertisement { get; private set; }
        public IBaseRepository<JobCareer> JobCareers { get; private set; }
        public IBaseRepository<Models.FAQ> FAQs { get; private set; }
        public IBaseRepository<Document> Documents { get; private set; }
        public IBaseRepository<Idea> Ideas { get; private set; }


        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            Rates = new BaseRepository<Rate>(context);
            Comments = new BaseRepository<Comment>(context);
            InteractionStatistic = new BaseRepository<InteractionStatistic>(context);
            GeneralNumbersResult = new BaseRepository<GeneralNumbersResult>(context);
            TotalVisitedByEntityResult = new BaseRepository<TotalVisitedByEntityResult>(context);
            StatisticsResult = new BaseRepository<StatisticsResult>(context);
            MemoryCache = memoryCache;
            Entities = new BaseRepository<Entity>(context);
            CronSettings = new BaseRepository<CronSettings>(context);
            MajorLookups = new BaseRepository<MajorLookup>(context);
            InteractionStatisticsTypes = new BaseRepository<InteractionStatisticsType>(context);

            Advertisements = new BaseRepository<Models.Advertisement>(context);
            Articles = new BaseRepository<Models.Article>(context);
            Eservices = new BaseRepository<Eservice>(context);
            GovServices = new BaseRepository<GovService>(context);
            News = new BaseRepository<News>(context);
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