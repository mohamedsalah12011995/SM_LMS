using Microsoft.EntityFrameworkCore.Storage;
using MapsterMapper;
using RM.Core.Interfaces;
using RM.Models;
using RM.Core.Repositories;



namespace RM.Lookups.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; } 
        public IBaseRepository<Models.MajorLookup> MajorLookup { get; private set; }
        public IBaseRepository<Models.MajorLookupsType> MajorLookupsType { get; private set; }
        public IBaseRepository<Recommendations> Recommendations { get; private set; }
        public IBaseRepository<CronSettings> CronSettings { get; private set; }
        public IBaseRepository<Entity> Entity { get; private set; }
        public IBaseRepository<User> Users { get; private set; }
        public IBaseRepository<UsersEntity> UsersEntity { get; private set; }

        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            MajorLookup = new BaseRepository<Models.MajorLookup>(context);
            MajorLookupsType = new BaseRepository<Models.MajorLookupsType>(context);
            Recommendations = new BaseRepository<Recommendations>(context);
            Entity = new BaseRepository<Entity>(context);
            CronSettings = new BaseRepository<CronSettings>(context);
            Users = new BaseRepository<User>(context);
            UsersEntity = new BaseRepository<UsersEntity>(context);
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