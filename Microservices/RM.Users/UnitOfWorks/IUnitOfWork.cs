using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;


namespace RM.Users.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IConfiguration Configuration { get; }
        IBaseRepository<Entity> Entity { get; }
        IBaseRepository<User> User { get; }
        IBaseRepository<ReferencesMajor> ReferencesMajor { get; }
        IBaseRepository<Reference> References { get; }
        IBaseRepository<AdminMenu> AdminMenus { get; }
        IBaseRepository<Menu> Menus { get; }

        IBaseRepository<Role> Role { get; }
        IBaseRepository<UsersEntity> UsersEntity { get; }
        IBaseRepository<UsersEntityReference> UsersEntityReference { get; }
        IBaseRepository<LoginWay> LoginWay { get; }

        IDbContextTransaction BeginTransaction();

        public IEnumerable<EntitiesLatestUpdate> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter);

        public ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; }
      
        int Complete();
        Task<int> CompleteAsync();
    }
}