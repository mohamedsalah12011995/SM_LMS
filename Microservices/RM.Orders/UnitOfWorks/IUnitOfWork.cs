


using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Orders.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IBaseRepository<Models.Order> Orders { get; }
        IBaseRepository<Models.OrderActions> OrderActions { get; }
        IBaseRepository<Models.MajorLookup> MajorLookups { get; }
        IBaseRepository<Models.User> Users { get; }

        IConfiguration Configuration { get; } 
        IDbContextTransaction BeginTransaction();
      
        int Complete();
        Task<int> CompleteAsync();
    }
}