using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;

namespace RM.References.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IBaseRepository<Menu> Menu { get; private set; }
        public IBaseRepository<Reference> References { get; private set; }
        public IBaseRepository<ReferencesMajor> ReferencesMajor { get; private set; }
        public IBaseRepository<ReferenceContent> ReferenceContent { get; private set; }
        public IBaseRepository<Entity> Entity { get; private set; }

        public ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; private set; }



        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            Menu = new BaseRepository<Menu>(context);
            Entity = new BaseRepository<Entity>(context);
            References = new BaseRepository<Reference>(context);
            ReferencesMajor = new BaseRepository<ReferencesMajor>(_context);
            ReferenceContent = new BaseRepository<ReferenceContent>(context);
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