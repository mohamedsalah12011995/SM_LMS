

namespace RM.Core.Interfaces
{
    public interface ISearchEngineRepository<T> where T : class
    {
        IQueryable<T> QueryProcedure(string ProcedureName, List<Tuple<string, object>> Params);
    }
}
