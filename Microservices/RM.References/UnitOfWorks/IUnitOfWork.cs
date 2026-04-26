using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.References.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IConfiguration Configuration { get; }
        IBaseRepository<Menu> Menu { get; }
        IBaseRepository<Entity> Entity { get; }

        IBaseRepository<ReferencesMajor> ReferencesMajor { get; }
        IBaseRepository<Reference> References { get; }
        IBaseRepository<ReferenceContent> ReferenceContent { get; }
        IDbContextTransaction BeginTransaction();
        public ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}