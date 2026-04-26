
using Microsoft.EntityFrameworkCore.Query;
using RM.Core.Consts;
using RM.Core.Helpers;
using System.Data;
using System.Linq.Expressions;


namespace RM.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        IQueryable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();

        IQueryable<T> GetAll(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending
        , params Expression<Func<T, object>>[] includes);

        T Find(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);
        Task<T> FindAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);

        //--------------- test ------------
        Task<T> FindWithMultiConditionsAsync(Expression<Func<T, bool>>[] criteria, params Expression<Func<T, object>>[] includes);

        //-------------------------
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int take, int skip, params Expression<Func<T, object>>[] includes);
        IQueryable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);
        //-------------- test ----------
        Task<IEnumerable<T>> FindAllAndOrderMultiConditionsAsync(Expression<Func<T, bool>>[] criteria, int? skip, int? take,
             Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, params Expression<Func<T, object>>[] includes);

        // --------------------
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take, params Expression<Func<T, object>>[] includes);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);
        Task<IEnumerable<T>> FindAllAndOrderAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, params Expression<Func<T, object>>[] includes);

        IQueryable<T> FindAllWithAsNoTracking(Expression<Func<T, bool>> criteria = null);

        Task<IEnumerable<T>> FindAllAsync(int skip, int take, params Expression<Func<T, object>>[] includes);

        IEnumerable<T> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter);

        T Add(T entity);
        Task<T> AddAsync(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        T Update(T entity);
        Task<T> UpdateAsync(T t, object key);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Attach(T entity);
        void AttachRange(IEnumerable<T> entities);
        int Count();
        int Count(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> criteria);
        Task<IList<string>> CheckField(Expression<Func<T, bool>> where, Expression<Func<T, string>> select);
        IQueryable<T> QueryProcedure(string ProcedureName, List<Tuple<string, object>> Params);
        byte[] GetFileArray(DataTable dataTable);

        public Task<(ApplicationOperation.Pagination? Pagination, IQueryable<T> Data)> FindAllByPagination(Expression<Func<T, bool>> criteria, ApplicationOperation.Pagination? Pagination, int DefaultPaginationCount, Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending,
         params Expression<Func<T, object>>[] includes);

        public Task<(ApplicationOperation.Pagination? Pagination, IQueryable<T> Data)> FindAllByPaginationAsync(Expression<Func<T, bool>> criteria, ApplicationOperation.Pagination? Pagination, int DefaultPaginationCount, Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending,
        params Expression<Func<T, object>>[] includes);


        public Task<(ApplicationOperation.Pagination? Pagination, IQueryable<T> Data)> FindAllByPaginationWithThenBy(Expression<Func<T, bool>> criteria, ApplicationOperation.Pagination? Pagination, int DefaultPaginationCount, Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending,
        Expression<Func<T, object>>[] thenByArray = null, string thenByDirection = ThenBy.Ascending,
        params Expression<Func<T, object>>[] includes);

        public (ApplicationOperation.Pagination Pagination, IQueryable<T> Data) GetAllWithPaggination(IQueryable<T> query, ApplicationOperation.Pagination? Pagination, int DefaultPaginationCount);

        int ExecuteUpdate(Expression<Func<T, bool>> criteria, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setExpression);
        Task<int> ExecuteUpdateAsync(Expression<Func<T, bool>> criteria, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setExpression);
        int ExecuteDelete(Expression<Func<T, bool>> criteria);
        Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> criteria);

        Task BulkInsertAsync(IEnumerable<T> entities);
        Task BulkUpdateAsync(IEnumerable<T> entities);
        Task<int> SaveChangesAsync();

    }
}