using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Multimedia.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IConfiguration Configuration { get; }

        IBaseRepository<Models.Multimedia> Multimedia { get; }
        IBaseRepository<Attachment> Attachment { get; }

        IDbContextTransaction BeginTransaction();

        int Complete();
        Task<int> CompleteAsync();
    }
}