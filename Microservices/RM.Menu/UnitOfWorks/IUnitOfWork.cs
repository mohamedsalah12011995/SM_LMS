using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Menu.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IConfiguration Configuration { get; }
        IBaseRepository<ReferencesMajor> ReferencesMajor { get; }

        IBaseRepository<Reference> References { get; }
        IBaseRepository<AdminMenu> AdminMenu { get; }
        IBaseRepository<MenuType> MenuTypes { get; }

        IBaseRepository<Models.Menu> Menu { get; }
        IBaseRepository<FormsEntity> FormsEntity { get; }
        IBaseRepository<User> User { get; }
        IBaseRepository<UsersEntity> UsersEntity { get; }

        IBaseRepository<UsersEntityReference> UsersEntityReference { get; }
        IBaseRepository<Entity> Entity { get; }


        IDbContextTransaction BeginTransaction();

        int Complete();
        Task<int> CompleteAsync();
    }
}