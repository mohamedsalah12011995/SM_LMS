using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;
using Microsoft.Extensions.Caching.Memory;


namespace RM.Advertisements.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IMemoryCache MemoryCache { get; private set; }
        public IConfiguration Configuration { get; private set; } 
        public Lazy<IBaseRepository<Advertisement>> _Advertisements { get; private set; }
        public Lazy<IBaseRepository<Reference>> _References { get; private set; }
        public Lazy<IBaseRepository<MajorLookup>> _MajorLookup { get; private set; }
        public Lazy<IBaseRepository<PublishEntities>> _PublishEntities { get; private set; }
        public ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; private set; }

        public UnitOfWork(ExternalPortal_v2Context context, ILoggerFactory loggerFactory, IMapper mapper, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            MemoryCache = memoryCache;

            _Advertisements = new Lazy<IBaseRepository<Advertisement>>(() => new BaseRepository<Advertisement>(_context));
            _References = new Lazy<IBaseRepository<Reference>>(() => new BaseRepository<Reference>(_context));
            _MajorLookup = new Lazy<IBaseRepository<MajorLookup>>(() => new BaseRepository<MajorLookup>(_context));
            _PublishEntities = new Lazy<IBaseRepository<PublishEntities>>(() => new BaseRepository<PublishEntities>(_context));

            sp_SearchEngine_Result = new SearchEngineRepository<SearchEngine>(_context);

        }

        public IBaseRepository<Advertisement> Advertisements => _Advertisements.Value;
        public IBaseRepository<Reference> References => _References.Value;
        public IBaseRepository<MajorLookup> MajorLookup => _MajorLookup.Value;
        public IBaseRepository<PublishEntities> PublishEntities => _PublishEntities.Value;


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