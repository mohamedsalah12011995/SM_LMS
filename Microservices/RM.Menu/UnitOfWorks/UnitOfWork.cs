using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;

namespace RM.Menu.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public Lazy<IBaseRepository<ReferencesMajor>> _ReferencesMajor { get; private set; }
        public Lazy<IBaseRepository<Reference>> _References { get; private set; }
        public Lazy<IBaseRepository<AdminMenu>> _AdminMenu { get; private set; }
        public Lazy<IBaseRepository<MenuType>> _MenuTypes { get; private set; }
        public Lazy<IBaseRepository<Models.Menu>> _Menu { get; private set; }
        public Lazy<IBaseRepository<User>> _User { get; private set; }
        public Lazy<IBaseRepository<UsersEntity>> _UsersEntity { get; private set; }
        public Lazy<IBaseRepository<UsersEntityReference>> _UsersEntityReference { get; private set; }
        public Lazy<IBaseRepository<FormsEntity>> _FormsEntity { get; private set; }
        public Lazy<IBaseRepository<Entity>> _Entity { get; private set; }


        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;

            _ReferencesMajor = new Lazy<IBaseRepository<ReferencesMajor>>(() => new BaseRepository<ReferencesMajor>(_context));
            _References = new Lazy<IBaseRepository<Reference>>(() => new BaseRepository<Reference>(_context));
            _MenuTypes = new Lazy<IBaseRepository<MenuType>>(() => new BaseRepository<MenuType>(context));
            _Menu = new Lazy<IBaseRepository<Models.Menu>>(() => new BaseRepository<Models.Menu>(context));
            _AdminMenu = new Lazy<IBaseRepository<AdminMenu>>(() => new BaseRepository<AdminMenu>(context));
            _User = new Lazy<IBaseRepository<User>>(() => new BaseRepository<User>(context));
            _UsersEntity = new Lazy<IBaseRepository<UsersEntity>>(() => new BaseRepository<UsersEntity>(context));
            _UsersEntityReference = new Lazy<IBaseRepository<UsersEntityReference>>(() => new BaseRepository<UsersEntityReference>(context));
            _FormsEntity = new Lazy<IBaseRepository<FormsEntity>>(() => new BaseRepository<FormsEntity>(context));
            _Entity = new Lazy<IBaseRepository<Entity>>(() => new BaseRepository<Entity>(context));

        }

        public IBaseRepository<ReferencesMajor> ReferencesMajor => _ReferencesMajor.Value;
        public IBaseRepository<Reference> References => _References.Value;
        public IBaseRepository<MenuType> MenuTypes => _MenuTypes.Value;
        public IBaseRepository<Models.Menu> Menu => _Menu.Value;
        public IBaseRepository<AdminMenu> AdminMenu => _AdminMenu.Value;
        public IBaseRepository<User> User => _User.Value;
        public IBaseRepository<UsersEntity> UsersEntity => _UsersEntity.Value;
        public IBaseRepository<UsersEntityReference> UsersEntityReference => _UsersEntityReference.Value;
        public IBaseRepository<FormsEntity> FormsEntity => _FormsEntity.Value;
        public IBaseRepository<Entity> Entity => _Entity.Value;


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