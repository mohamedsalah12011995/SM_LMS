


using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Volunteers.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        IMapper Mappers { get; }
        IBaseRepository<Models.Volunteer> Volunteers { get; }
        IBaseRepository<Models.MajorLookup> MajorLookups { get; }
        IConfiguration Configuration { get; } 
        IDbContextTransaction BeginTransaction();
      
        int Complete();
        Task<int> CompleteAsync();
    }
}