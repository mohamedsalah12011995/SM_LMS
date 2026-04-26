using Microsoft.EntityFrameworkCore.Storage;
using MapsterMapper;
using RM.Core.Interfaces;
using RM.Models;
using RM.Core.Repositories;
using RM.GovServices.Dtos;

namespace RM.GovServices.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; } 
        public Lazy<IBaseRepository<Models.GovService>> _GovServices { get; private set; }
        public Lazy<IBaseRepository<Models.Eservice>> _Eservices { get; private set; }
        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;

            _Eservices = new Lazy<IBaseRepository<Eservice>>(() => new BaseRepository<Eservice>(_context));
            _GovServices = new Lazy<IBaseRepository<GovService>>(() => new BaseRepository<GovService>(_context));

        }

        public IBaseRepository<Eservice> Eservices => _Eservices.Value;
        public IBaseRepository<GovService> GovServices => _GovServices.Value;

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