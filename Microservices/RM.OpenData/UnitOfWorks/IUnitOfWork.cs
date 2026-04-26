


using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.OpenData.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IMemoryCache MemoryCache { get; }

        IBaseRepository<Models.OpenData> OpenData { get; }
        IBaseRepository<OpenDataTemp> OpenDataTemp { get; }
        IBaseRepository<OpenDataRequest> OpenDataRequest { get; }
        IBaseRepository<OpenDataStatistics> OpenDataStatistics { get; }
        IBaseRepository<MajorLookup> MajorLookup { get; }
        IBaseRepository<User> User { get; }
        IBaseRepository<CronSettings> CronSettings { get; }
        IConfiguration Configuration { get; } 
        IDbContextTransaction BeginTransaction();
      
        int Complete();
        Task<int> CompleteAsync();
    }
}