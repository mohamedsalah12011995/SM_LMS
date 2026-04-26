using Microsoft.EntityFrameworkCore.Storage;
using MapsterMapper;
using RM.Models;
using RM.Core.Repositories;
using RM.Core.Interfaces;


namespace RM.Orders.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; } 
        public IBaseRepository<Models.Order> Orders { get; private set; }
        public IBaseRepository<Models.OrderActions> OrderActions { get; private set; }
        public IBaseRepository<Models.MajorLookup> MajorLookups { get; private set; }
        public IBaseRepository<Models.User> Users { get; private set; }
        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            Orders = new BaseRepository<Models.Order>(context);
            OrderActions = new BaseRepository<Models.OrderActions>(context);
            MajorLookups = new BaseRepository<Models.MajorLookup>(context);
            Users = new BaseRepository<Models.User>(context);
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