using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using RM.Core.Interfaces;
using RM.Models;




namespace RM.Advertisements.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IMemoryCache MemoryCache { get; }
        IBaseRepository<Advertisement> Advertisements { get; }
        IBaseRepository<Reference> References { get; }
        IConfiguration Configuration { get; } 
        IBaseRepository<MajorLookup> MajorLookup { get; }
        IBaseRepository<PublishEntities> PublishEntities { get; }
        IDbContextTransaction BeginTransaction();

        public IEnumerable<EntitiesLatestUpdate> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter);

        public ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; }
      
        int Complete();
        Task<int> CompleteAsync();
    }
}