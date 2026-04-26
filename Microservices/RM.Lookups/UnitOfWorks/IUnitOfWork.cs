


using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Lookups.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IBaseRepository<Models.MajorLookup> MajorLookup { get; }
        IBaseRepository<Models.MajorLookupsType> MajorLookupsType { get; }
        IBaseRepository<Recommendations> Recommendations { get; }
        IBaseRepository<Entity> Entity { get; }
        IBaseRepository<CronSettings> CronSettings { get; }
        IBaseRepository<User> Users { get; }
        IBaseRepository<UsersEntity> UsersEntity { get; }
        IConfiguration Configuration { get; } 
        IDbContextTransaction BeginTransaction();
      
        int Complete();
        Task<int> CompleteAsync();
    }
}