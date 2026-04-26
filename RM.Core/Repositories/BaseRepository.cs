
using ClosedXML.Excel;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Pagination.EntityFrameworkCore.Extensions;
using RM.Core.Consts;
using RM.Core.Helpers;
using RM.Core.Interfaces;
using RM.Models;
using System.Data;
using System.Linq.Expressions;

namespace RM.Core.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public ExternalPortal_v2Context _context;

        public BaseRepository(ExternalPortal_v2Context context)
        {
            _context = context;
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> orderBy = null, string orderByDirection = "ASC", params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }
            return query.AsNoTracking();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public T Find(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return query.SingleOrDefault(criteria);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return await query.FirstOrDefaultAsync(criteria);
        }
        public async Task<T> FindWithMultiConditionsAsync(Expression<Func<T, bool>>[] criteria, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);
            if (criteria != null)
                foreach (var condition in criteria)
                    query = query.Where(condition);

            return await query.FirstOrDefaultAsync();
        }
        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.Where(criteria).ToList();
        }

        public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int skip, int take, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.Where(criteria).Skip(skip).Take(take).ToList();
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return query;
        }

        public async Task<IEnumerable<T>> FindAllAndOrderAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria)
                                .Skip(skip.Value).Take(take.Value);
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }
            return await query.ToListAsync();
        }

        //-------------- test -----------
        public async Task<IEnumerable<T>> FindAllAndOrderMultiConditionsAsync(Expression<Func<T, bool>>[] criteria, int? skip, int? take,
                Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            if (criteria != null)
            {
                foreach (var condition in criteria)
                    query = query.Where(condition);
            }

            query.Skip(skip.Value).Take(take.Value);
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }
            return await query.ToListAsync();
        }
        //-----------

        public IQueryable<T> FindAllWithAsNoTracking(Expression<Func<T, bool>> criteria)
        {
            if (criteria is not null)
                return _context.Set<T>().Where(criteria).AsNoTracking();
            else return _context.Set<T>().AsNoTracking();
        }
        public IEnumerable<T> GetFromProcdure(string ProcedureName, string ProcedureParams, params object[] Parameter)
        {
            return _context.Set<T>().FromSqlRaw(ProcedureName + ProcedureParams, Parameter);
        }
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            return await query.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int take, int skip, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            return await query.Where(criteria).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAllAsync(int skip, int take
                                        , params Expression<Func<T, object>>[] includs)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var includeProperty in includs)
                query = query.Include(includeProperty);

            return await query.Skip(skip).Take(take).ToListAsync();
        }
        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            return entity;
        }

        public IEnumerable<T> AddRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            return entities;
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }

        public async Task<T> UpdateAsync(T t, object key)
        {
            await Task.Run(() =>
            {
                _context.Entry(t).CurrentValues.SetValues(key);


            });
            return t;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public void Attach(T entity)
        {
            _context.Set<T>().Attach(entity);
        }

        public void AttachRange(IEnumerable<T> entities)
        {
            _context.Set<T>().AttachRange(entities);
        }

        public int Count()
        {
            return _context.Set<T>().Count();
        }

        public int Count(Expression<Func<T, bool>> criteria)
        {
            return _context.Set<T>().Count(criteria);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Set<T>().CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>().CountAsync(criteria);
        }

        public async Task<IList<string>> CheckField(Expression<Func<T, bool>> where, Expression<Func<T, string>> select)
        {
            return await _context.Set<T>().Where(where).Select(select).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take)
        {
            return await _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToListAsync();
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
        public byte[] GetFileArray(DataTable dataTable)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                dataTable.TableName = "Sheet1";
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return stream.ToArray();
                }
            }
        }

        public async Task<(ApplicationOperation.Pagination Pagination, IQueryable<T> Data)> FindAllByPagination(Expression<Func<T, bool>> criteria, ApplicationOperation.Pagination? Pagination, int DefaultPaginationCount, Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending,
        params Expression<Func<T, object>>[] includes)
        {
            int CurrentPageIndex = 1;
            IQueryable<T> query = _context.Set<T>().Where(criteria);
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            if (Pagination != null)
            {
                if (Pagination.CurrentPageIndex.HasValue)
                    CurrentPageIndex = Pagination.CurrentPageIndex.Value;
                if (Pagination.RecordPerPage.HasValue)
                    DefaultPaginationCount = Pagination.RecordPerPage.Value;

                var result = await query.AsPaginationAsync(CurrentPageIndex, DefaultPaginationCount);
                Pagination = new ApplicationOperation.Pagination { RecordPerPage = DefaultPaginationCount, TotalPagesCount = result.TotalPages, CurrentPageIndex = result.CurrentPage, TotalItemsCount = (int)result.TotalItems };
                return (Pagination: Pagination, Data: result.Results.AsQueryable().AsNoTracking());
                //.Results.ToList();
            }
            else
            {
                DefaultPaginationCount = query.Count();
                Pagination = new ApplicationOperation.Pagination { RecordPerPage = DefaultPaginationCount, TotalPagesCount = 1, CurrentPageIndex = 1, TotalItemsCount = DefaultPaginationCount };
                return (Pagination: Pagination, Data: query.AsNoTracking());
            }
        }

        public async Task<(ApplicationOperation.Pagination Pagination, IQueryable<T> Data)> FindAllByPaginationAsync(Expression<Func<T, bool>> criteria, ApplicationOperation.Pagination? Pagination, int DefaultPaginationCount, Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending,
              params Expression<Func<T, object>>[] includes)
        {
            int CurrentPageIndex = 1;
            IQueryable<T> query = _context.Set<T>().Where(criteria);
            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            if (Pagination != null)
            {
                if (Pagination.CurrentPageIndex.HasValue)
                    CurrentPageIndex = Pagination.CurrentPageIndex.Value;
                if (Pagination.RecordPerPage.HasValue)
                    DefaultPaginationCount = Pagination.RecordPerPage.Value;

                var result = await query.AsPaginationAsync(CurrentPageIndex, DefaultPaginationCount);
                Pagination = new ApplicationOperation.Pagination { RecordPerPage = DefaultPaginationCount, TotalPagesCount = result.TotalPages, CurrentPageIndex = result.CurrentPage, TotalItemsCount = (int)result.TotalItems };
                return (Pagination: Pagination, Data: result.Results.AsQueryable().AsNoTracking());
            }
            else
            {
                DefaultPaginationCount = query.Count();
                Pagination = new ApplicationOperation.Pagination { RecordPerPage = DefaultPaginationCount, TotalPagesCount = 1, CurrentPageIndex = 1, TotalItemsCount = DefaultPaginationCount };
                return (Pagination: Pagination, Data: query.AsNoTracking());
            }
        }


        public async Task<(ApplicationOperation.Pagination Pagination, IQueryable<T> Data)>
         FindAllByPaginationWithThenBy(Expression<Func<T, bool>> criteria, ApplicationOperation.Pagination? Pagination, int DefaultPaginationCount, Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending,
         Expression<Func<T, object>>[] thenByArray = null, string thenByDirection = ThenBy.Ascending,
        params Expression<Func<T, object>>[] includes)
        {
            int CurrentPageIndex = 1;
            IQueryable<T> query = _context.Set<T>().Where(criteria);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            if (thenByArray != null)
            {
                IOrderedQueryable<T> queryOrder = (IOrderedQueryable<T>)query;
                foreach (var thenBy in thenByArray)
                {
                    if (thenByDirection == ThenBy.Ascending)
                        query = queryOrder.ThenBy(thenBy);
                    else
                        query = queryOrder.ThenByDescending(thenBy);
                }
            }

            if (Pagination != null)
            {
                if (Pagination.CurrentPageIndex.HasValue)
                    CurrentPageIndex = Pagination.CurrentPageIndex.Value;
                if (Pagination.RecordPerPage.HasValue)
                    DefaultPaginationCount = Pagination.RecordPerPage.Value;

                var result = await query.AsPaginationAsync(CurrentPageIndex, DefaultPaginationCount);
                Pagination = new ApplicationOperation.Pagination { RecordPerPage = DefaultPaginationCount, TotalPagesCount = result.TotalPages, CurrentPageIndex = result.CurrentPage, TotalItemsCount = (int)result.TotalItems };
                return (Pagination: Pagination, Data: result.Results.AsQueryable().AsNoTracking());
            }
            else
            {
                DefaultPaginationCount = query.Count();
                Pagination = new ApplicationOperation.Pagination { RecordPerPage = DefaultPaginationCount, TotalPagesCount = 1, CurrentPageIndex = 1, TotalItemsCount = DefaultPaginationCount };
                return (Pagination: Pagination, Data: query.AsNoTracking());
            }
        }

        public (ApplicationOperation.Pagination Pagination, IQueryable<T> Data) GetAllWithPaggination(IQueryable<T> query, ApplicationOperation.Pagination? Pagination, int DefaultPaginationCount)
        {
            int CurrentPageIndex = 1;
            int NumberOfRecord = query.Count();
            var paginationItem = new ApplicationOperation.Pagination();

            if (Pagination != null)
            {
                if (Pagination.CurrentPageIndex.HasValue)
                    CurrentPageIndex = Pagination.CurrentPageIndex.Value;
                if (Pagination.RecordPerPage.HasValue)
                    DefaultPaginationCount = Pagination.RecordPerPage.Value;
                else DefaultPaginationCount = NumberOfRecord > 0 ? NumberOfRecord : DefaultPaginationCount;
            }
            else
            {
                DefaultPaginationCount = NumberOfRecord > 0 ? NumberOfRecord : DefaultPaginationCount; ;
            }

            paginationItem.TotalPagesCount = Math.Ceiling((float)NumberOfRecord / (float)DefaultPaginationCount);
            paginationItem.CurrentPageIndex = CurrentPageIndex;
            paginationItem.TotalItemsCount = NumberOfRecord;
            paginationItem.RecordPerPage = DefaultPaginationCount;
            return (paginationItem, query.Skip((CurrentPageIndex - 1) * DefaultPaginationCount).Take(DefaultPaginationCount).AsNoTracking());
        }

        public int ExecuteUpdate(Expression<Func<T, bool>> criteria, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setExpression)
        {
            return _context.Set<T>().Where(criteria).ExecuteUpdate(setExpression);

        }

        public async Task<int> ExecuteUpdateAsync(Expression<Func<T, bool>> criteria, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> setExpression)
        {

            return await _context.Set<T>().Where(criteria).ExecuteUpdateAsync(setExpression);
        }

        public int ExecuteDelete(Expression<Func<T, bool>> criteria)
        {
            return _context.Set<T>()
                 .Where(criteria)
                 .ExecuteDelete();
        }

        public async Task<int> ExecuteDeleteAsync(Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>()
                 .Where(criteria)
                 .ExecuteDeleteAsync();
        }
        public async Task BulkInsertAsync(IEnumerable<T> entities)
        {
            await _context.BulkInsertAsync(entities);
        }
        public async Task BulkUpdateAsync(IEnumerable<T> entities)
        {
            await _context.BulkUpdateAsync(entities);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


    }
}