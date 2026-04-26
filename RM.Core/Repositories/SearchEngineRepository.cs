using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RM.Core.Interfaces;
using RM.Models;

namespace RM.Core.Repositories
{
    public class SearchEngineRepository<T> : ISearchEngineRepository<T> where T : class
    {
        protected ExternalPortal_v2Context _context;
        public SearchEngineRepository(ExternalPortal_v2Context context)
        {
            _context = context;
        }
        public IQueryable<T> QueryProcedure(string ProcedureName, List<Tuple<string, object>> Params)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            string ExecuteProcedure = ProcedureName;

            foreach (var item in Params)
            {
                param.Add(new SqlParameter("@" + item.Item1, item.Item2));
                if (Params.IndexOf(item) > 0) ExecuteProcedure += ",";
                ExecuteProcedure += " @" + item.Item1;
            }
            DbSet<T> query = _context.Set<T>();
            IQueryable<T> Result = query.FromSqlRaw(ExecuteProcedure, param.ToArray()).AsQueryable();
            return Result;
        }
    }
}
