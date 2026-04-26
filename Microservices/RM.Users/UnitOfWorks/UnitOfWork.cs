using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;
using DocumentFormat.OpenXml.Office2013.Excel;
using static RM.Core.Helpers.Enums;


namespace RM.Users.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; }
        public IBaseRepository<User> User { get; private set; }
        public IBaseRepository<Entity> Entity { get; private set; }
        public IBaseRepository<Reference> References { get; private set; }
        public IBaseRepository<ReferencesMajor> ReferencesMajor { get; private set; }

        public IBaseRepository<UsersEntity> UsersEntity { get; private set; }
        public IBaseRepository<UsersEntityReference> UsersEntityReference { get; private set; }
        public IBaseRepository<Role> Role { get; private set; }
        public IBaseRepository<Models.LoginWay> LoginWay { get; private set; }
        public IBaseRepository<AdminMenu> AdminMenus { get; private set; }
        public IBaseRepository<Menu> Menus { get; private set; }

        public ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; private set; }

        public UnitOfWork(ExternalPortal_v2Context context, ILoggerFactory loggerFactory, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            Entity = new BaseRepository<Entity>(context);
            User = new BaseRepository<User>(context);
 
            References = new BaseRepository<Reference>(context);
            ReferencesMajor = new BaseRepository<ReferencesMajor>(_context);

            UsersEntity = new BaseRepository<UsersEntity>(context);
            UsersEntityReference = new BaseRepository<UsersEntityReference>(context);
            Role = new BaseRepository<Role>(context);
            LoginWay = new BaseRepository<Models.LoginWay>(context);

            Menus = new BaseRepository<Menu>(context);
            AdminMenus = new BaseRepository<AdminMenu>(context);

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