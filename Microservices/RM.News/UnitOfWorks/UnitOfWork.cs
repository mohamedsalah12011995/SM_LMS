using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;

namespace RM.News.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IMemoryCache MemoryCache { get; private set; }

        public Lazy<IBaseRepository<Reference>> _References { get; private set; }
        public Lazy<IBaseRepository<Models.News>> _News { get; private set; }
        public Lazy<IBaseRepository<Tag>> _Tags { get; private set; }
        public Lazy<IBaseRepository<PublishEntities>> _PublishEntities { get; private set; }

        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            MemoryCache = memoryCache;

            _References = new Lazy<IBaseRepository<Reference>>(() => new BaseRepository<Reference>(_context));
            _News = new Lazy<IBaseRepository<Models.News>>(() => new BaseRepository<Models.News>(context));
            _Tags = new Lazy<IBaseRepository<Tag>>(() => new BaseRepository<Tag>(context));
            _PublishEntities = new Lazy<IBaseRepository<PublishEntities>>(() => new BaseRepository<PublishEntities>(context));
        }

        public IBaseRepository<Reference> References => _References.Value;
        public IBaseRepository<Models.News> News => _News.Value;
        public IBaseRepository<Tag> Tags => _Tags.Value;
        public IBaseRepository<PublishEntities> PublishEntities => _PublishEntities.Value;


        public IEnumerable<EntitiesLatestUpdate> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter)
        {
            return _context.Set<EntitiesLatestUpdate>().FromSqlRaw(ProcedureName + ProcedureParams, Parameter);
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