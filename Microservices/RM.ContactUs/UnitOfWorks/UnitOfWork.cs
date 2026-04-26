using Microsoft.EntityFrameworkCore.Storage;
using MapsterMapper;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;
using Microsoft.Extensions.Caching.Memory;


namespace RM.ContactUs.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IMemoryCache MemoryCache { get; private set; }

        public IConfiguration Configuration { get; private set; } 
        public IBaseRepository<ContactU> ContactUs { get; private set; }
        public IBaseRepository<User> User { get; private set; }
        public IBaseRepository<Reference> References { get; private set; }
        public IBaseRepository<Feedback> Feedback { get; private set; }
        public IBaseRepository<Actions> Actions { get; private set; }

        public IBaseRepository<Entity> Entities { get; private set; }
        public IBaseRepository<CronSettings> CronSettings { get; private set; }

        public IBaseRepository<Eservice> Eservices { get; private set; }
        public IBaseRepository<GovService> GovServices { get; private set; }
        public IBaseRepository<MajorStatus> MajorStatus { get; private set; }
        public IBaseRepository<InteractionStatistic> InteractionStatistic { get; private set; }
        public IBaseRepository<Recommendations> Recommendations { get; private set; }

        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration , IMemoryCache memoryCache)
        {
            _context = context;
            Mappers = mapper;
            MemoryCache = memoryCache;
            Configuration = configuration;
            ContactUs = new BaseRepository<ContactU>(context);
            User=new BaseRepository<User>(context);
            References = new BaseRepository<Reference>(context);
            Feedback = new BaseRepository<Feedback>(context);
            Actions = new BaseRepository<Actions>(context);
            Eservices = new BaseRepository<Eservice>(context);
            GovServices = new BaseRepository<GovService>(context);
            Entities = new BaseRepository<Entity>(context);
            CronSettings = new BaseRepository<CronSettings>(context);
            MajorStatus = new BaseRepository<MajorStatus>(context);
            InteractionStatistic = new BaseRepository<InteractionStatistic>(context);
            Recommendations = new BaseRepository<Recommendations>(context);
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