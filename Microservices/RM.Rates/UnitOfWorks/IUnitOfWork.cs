using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Rates.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IConfiguration Configuration { get; }
        IBaseRepository<Rate> Rates { get; }

        IDbContextTransaction BeginTransaction();

        int Complete();
        Task<int> CompleteAsync();
    }
}