using Microsoft.EntityFrameworkCore.Storage;
using MapsterMapper;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;


namespace RM.ExternalSites.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; } 
        public IBaseRepository<ExternalSite> ExternalSite { get; private set; }
        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            ExternalSite = new BaseRepository<ExternalSite>(context);

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