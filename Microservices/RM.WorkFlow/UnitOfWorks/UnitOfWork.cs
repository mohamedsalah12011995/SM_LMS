using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RM.Core.Interfaces;
using RM.Core.Repositories;
using RM.Models;

namespace RM.WorkFlow.UnitOfWorks
{

    public class UnitOfWork : IUnitOfWork
    {
        private ExternalPortal_v2Context _context;
        public IMapper Mappers { get; private set; }
        public IConfiguration Configuration { get; private set; }

        public IBaseRepository<Reference> References { get; private set; }
        public IBaseRepository<ReferencesMajor> ReferencesMajor { get; private set; }
        public IBaseRepository<Entity> Entity { get; private set; }

        public IBaseRepository<Form> Forms { get; private set; }
        public IBaseRepository<FormInput> FormInputs { get; private set; }
        public IBaseRepository<FormValue> FormValue { get; private set; }

        public IBaseRepository<FormsEntity> FormsEntity { get; private set; }
        public IBaseRepository<FormValueDetails> FormValueDetails { get; private set; }

        public IBaseRepository<Engine> Engine { get; private set; }
        public IBaseRepository<WorkFlowActions> WorkFlowActions { get; private set; }
        public IBaseRepository<FormInputsActions> FormInputsActions { get; private set; }
        public IBaseRepository<FormValuesActions> FormValuesActions { get; private set; }
        public IBaseRepository<EngineActionJobRole> EnginesActionsJobRole { get; private set; }
        public IBaseRepository<EngineForms> EngineForms { get; private set; }
        public IBaseRepository<JobApplication> JobApplication { get; private set; }
        public IBaseRepository<DynamicFormAdvanceSearch> DynamicFormAdvanceSearch { get; private set; }

        public ISearchEngineRepository<DynamicFormAdvanceSearch> sp_dynamicFormAdvanceSearch { get; private set; }


        public IBaseRepository<MajorLookup> MajorLookup { get; private set; }
        public IBaseRepository<User> User { get; private set; }
        public IBaseRepository<FormValueDataSource> FormValueDataSource { get; private set; }

        public ISearchEngineRepository<SearchEngine> sp_SearchEngine_Result { get; private set; }

        public UnitOfWork(ExternalPortal_v2Context context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            Mappers = mapper;
            Configuration = configuration;
            References = new BaseRepository<Reference>(context);
            ReferencesMajor = new BaseRepository<ReferencesMajor>(_context);
            User = new BaseRepository<User>(_context);
            Entity = new BaseRepository<Entity>(context);
            Forms = new BaseRepository<Form>(context);
            FormInputs = new BaseRepository<FormInput>(context);
            FormValue = new BaseRepository<FormValue>(context);

            FormsEntity = new BaseRepository<FormsEntity>(context);
            FormValueDetails = new BaseRepository<FormValueDetails>(context);
            DynamicFormAdvanceSearch = new BaseRepository<DynamicFormAdvanceSearch>(context);
            Engine = new BaseRepository<Engine>(context);
            WorkFlowActions = new BaseRepository<WorkFlowActions>(context);
            FormInputsActions = new BaseRepository<FormInputsActions>(context);
            FormValuesActions = new BaseRepository<FormValuesActions>(context);
            EnginesActionsJobRole = new BaseRepository<EngineActionJobRole>(context);
            EngineForms = new BaseRepository<EngineForms>(context);
            MajorLookup = new BaseRepository<MajorLookup>(context);
            FormValueDataSource = new BaseRepository<FormValueDataSource>(context);
            JobApplication = new BaseRepository<JobApplication>(context);
            sp_dynamicFormAdvanceSearch = new SearchEngineRepository<DynamicFormAdvanceSearch>(_context);
            sp_SearchEngine_Result = new SearchEngineRepository<SearchEngine>(_context);

        }


        public void Dispose()
        {
            _context.Dispose();
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
        public IEnumerable<EntitiesLatestUpdate> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter)
        {
            return _context.Set<EntitiesLatestUpdate>().FromSqlRaw(ProcedureName + ProcedureParams, Parameter);
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }
    }
}