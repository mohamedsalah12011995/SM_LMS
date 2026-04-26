using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;

namespace RM.Entities.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IBaseRepository<Entity> Entity { get; private set; }
        public IBaseRepository<EntitiesType>  EntitiesType { get; private set; }
        public IBaseRepository<Form> Forms { get; private set; }
        public IBaseRepository<FormsEntity> FormsEntity { get; private set; }
        public IBaseRepository<Reference> References { get; private set; }
        public IBaseRepository<MajorLookup> MajorLookup { get; private set; }
        public ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; private set; }

        public UnitOfWork(ExternalPortal_v2Context context, ILoggerFactory loggerFactory, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            References = new BaseRepository<Reference>(context);
            Entity = new BaseRepository<Entity>(context);
            EntitiesType = new BaseRepository<EntitiesType>(context);
            Forms = new BaseRepository<Form>(context);
            FormsEntity = new BaseRepository<FormsEntity>(context);
            MajorLookup = new BaseRepository<MajorLookup>(context);

            sp_SearchEngine_Result = new SearchEngineRepository<SearchEngine>(_context);

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