using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Entities.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IBaseRepository<Entity> Entity { get; }
        IBaseRepository<EntitiesType> EntitiesType { get; }
        IBaseRepository<Form> Forms { get; }
        IBaseRepository<FormsEntity> FormsEntity { get; }

        IBaseRepository<Reference> References { get; }
        IConfiguration Configuration { get; } 
        IBaseRepository<MajorLookup> MajorLookup { get; }
        IDbContextTransaction BeginTransaction();

        public IEnumerable<EntitiesLatestUpdate> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter);

        public ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; }
      
        int Complete();
        Task<int> CompleteAsync();
    }
}