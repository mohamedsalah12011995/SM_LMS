using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.News.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IConfiguration Configuration { get; }
        IMemoryCache MemoryCache { get; }

        IBaseRepository<Reference> References { get; }
        IBaseRepository<PublishEntities> PublishEntities { get; }
        IBaseRepository<Models.News> News { get; }
        IBaseRepository<Tag> Tags { get; }

        IDbContextTransaction BeginTransaction();

        public IEnumerable<Models.EntitiesLatestUpdate> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter);


        int Complete();
        Task<int> CompleteAsync();
    }
}