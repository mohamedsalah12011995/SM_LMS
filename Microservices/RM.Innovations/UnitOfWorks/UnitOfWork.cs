using Microsoft.EntityFrameworkCore.Storage;
using MapsterMapper;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;
using Microsoft.Extensions.Caching.Memory;


namespace RM.Innovations.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMemoryCache MemoryCache { get; private set; }

        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; } 
        public Lazy<IBaseRepository<Idea>> _Ideas { get; private set; }
        public Lazy<IBaseRepository<IdeaAction>> _IdeaActions { get; private set; }
        public Lazy<IBaseRepository<IdeaComment>> _IdeaComments { get; private set; }
        public Lazy<IBaseRepository<IdeasCompetentAuthority>> _IdeasCompetentAuthority { get; private set; }
        public Lazy<IBaseRepository<User>> _Users { get; private set; }
        public Lazy<IBaseRepository<Reference>> _References { get; private set; }
        public Lazy<IBaseRepository<MajorLookup>> _MajorLookups { get; private set; }
        public Lazy<IBaseRepository<Entity>> _Entities { get; private set; }
        public Lazy<IBaseRepository<CronSettings>> _CronSettings { get; private set; }

        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            MemoryCache = memoryCache;

            _Ideas = new Lazy<IBaseRepository<Idea>>(() => new BaseRepository<Idea>(context));
            _IdeaActions = new Lazy<IBaseRepository<IdeaAction>>(() => new BaseRepository<IdeaAction>(context));
            _IdeaComments = new Lazy<IBaseRepository<IdeaComment>>(() => new BaseRepository<IdeaComment>(context));
            _IdeasCompetentAuthority = new Lazy<IBaseRepository<IdeasCompetentAuthority>>(() => new BaseRepository<IdeasCompetentAuthority>(context));
            _Users = new Lazy<IBaseRepository<User>>(() => new BaseRepository<User>(context));
            _References = new Lazy<IBaseRepository<Reference>>(() => new BaseRepository<Reference>(_context));
            _MajorLookups = new Lazy<IBaseRepository<MajorLookup>>(() => new BaseRepository<MajorLookup>(context));
            _CronSettings = new Lazy<IBaseRepository<CronSettings>>(() => new BaseRepository<CronSettings>(context));
            _Entities = new Lazy<IBaseRepository<Entity>>(() => new BaseRepository<Entity>(context));
        }

        public IBaseRepository<Idea> Ideas => _Ideas.Value;
        public IBaseRepository<IdeaAction> IdeaActions => _IdeaActions.Value;
        public IBaseRepository<IdeaComment> IdeaComments => _IdeaComments.Value;
        public IBaseRepository<IdeasCompetentAuthority> IdeasCompetentAuthority => _IdeasCompetentAuthority.Value;
        public IBaseRepository<User> Users => _Users.Value;
        public IBaseRepository<Reference> References => _References.Value;
        public IBaseRepository<MajorLookup> MajorLookups => _MajorLookups.Value;
        public IBaseRepository<CronSettings> CronSettings => _CronSettings.Value;
        public IBaseRepository<Entity> Entities => _Entities.Value;



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