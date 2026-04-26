using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;

namespace CronJobService.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IMemoryCache MemoryCache { get; private set; }

        public IConfiguration Configuration { get; private set; }

        public IBaseRepository<MajorLookup> MajorLookups { get; private set; }


        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration, IMemoryCache memoryCache)
        {
            _context = context;
            MemoryCache = memoryCache;
            Configuration = configuration;

            MajorLookups = new BaseRepository<MajorLookup>(context);
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