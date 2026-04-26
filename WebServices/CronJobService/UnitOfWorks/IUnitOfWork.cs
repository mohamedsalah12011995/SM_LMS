

using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Models;

namespace CronJobService.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMemoryCache MemoryCache { get; }
        IConfiguration Configuration { get; }

        IBaseRepository<MajorLookup> MajorLookups { get; }
        IDbContextTransaction BeginTransaction();

        int Complete();
        Task<int> CompleteAsync();
    }
}