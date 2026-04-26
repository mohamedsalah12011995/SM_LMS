using Microsoft.EntityFrameworkCore.Storage;
using MapsterMapper;
using RM.Core.Interfaces;
using RM.Models;
using RM.Core.Repositories;
using Microsoft.Extensions.Caching.Memory;


namespace RM.OpenData.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IMemoryCache MemoryCache { get; private set; }

        public IConfiguration Configuration { get; private set; }
        public IBaseRepository<Models.OpenData> OpenData { get; private set; }
        public IBaseRepository<OpenDataTemp> OpenDataTemp { get; private set; }
        public IBaseRepository<OpenDataRequest> OpenDataRequest { get; private set; }
        public IBaseRepository<OpenDataStatistics> OpenDataStatistics { get; private set; }
        public IBaseRepository<MajorLookup> MajorLookup { get; private set; }
        public IBaseRepository<User> User { get; private set; }
        public IBaseRepository<CronSettings> CronSettings { get; private set; }

        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _context = context;
            Mappers = mapper;
            MemoryCache = memoryCache;

            Configuration = configuration;
            OpenData = new BaseRepository<Models.OpenData>(context);
            OpenDataTemp = new BaseRepository<OpenDataTemp>(context);
            OpenDataRequest = new BaseRepository<OpenDataRequest>(context);
            OpenDataStatistics = new BaseRepository<OpenDataStatistics>(context);
            MajorLookup = new BaseRepository<MajorLookup>(context);
            User = new BaseRepository<User>(context);
            CronSettings = new BaseRepository<CronSettings>(context);
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