using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Regulations.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IConfiguration Configuration { get; }
        IBaseRepository<TermsAndRegulation> TermsAndRegulation { get; }

        IDbContextTransaction BeginTransaction();

        int Complete();
        Task<int> CompleteAsync();
    }
}