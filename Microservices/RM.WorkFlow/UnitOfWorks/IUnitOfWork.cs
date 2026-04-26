using MapsterMapper;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.WorkFlow.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {

        IMapper Mappers { get; }
        IConfiguration Configuration { get; }
        IBaseRepository<Reference> References { get; }
        IBaseRepository<Entity> Entity { get; }
        IBaseRepository<Form> Forms { get; }
        IBaseRepository<FormInput> FormInputs { get; }
        IBaseRepository<FormValue> FormValue { get; }

        IBaseRepository<FormsEntity> FormsEntity { get; }
        IBaseRepository<FormValueDetails> FormValueDetails { get; }

        IBaseRepository<Engine> Engine { get; }
        IBaseRepository<WorkFlowActions> WorkFlowActions { get; }
        IBaseRepository<FormInputsActions> FormInputsActions { get; }
        IBaseRepository<FormValuesActions> FormValuesActions { get; }
        IBaseRepository<EngineActionJobRole> EnginesActionsJobRole { get; }
        IBaseRepository<EngineForms> EngineForms { get; }

        IBaseRepository<ReferencesMajor> ReferencesMajor { get; }
        IBaseRepository<MajorLookup> MajorLookup { get; }
        IBaseRepository<User> User { get; }
        IBaseRepository<JobApplication> JobApplication { get; }
        IBaseRepository<FormValueDataSource> FormValueDataSource { get; }

        IDbContextTransaction BeginTransaction();

        public IEnumerable<EntitiesLatestUpdate> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter);
        public ISearchEngineRepository<DynamicFormAdvanceSearch> sp_dynamicFormAdvanceSearch { get; }

        public ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}