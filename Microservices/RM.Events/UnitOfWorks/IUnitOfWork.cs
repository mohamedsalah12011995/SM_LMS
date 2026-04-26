using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Events.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IBaseRepository<EidFiterRequest> EidFiterRequests { get; }
        IBaseRepository<MajorLookup> MajorLookup { get; }
        IBaseRepository<User> User { get; }

        IConfiguration Configuration { get; } 
        IDbContextTransaction BeginTransaction();

        public IEnumerable<EntitiesLatestUpdate> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter);

        public ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; }
      
        int Complete();
        Task<int> CompleteAsync();
    }
}