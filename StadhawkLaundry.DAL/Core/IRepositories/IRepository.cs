using StadhawkCoreApi;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Core.IRepositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<ApiResult<IEnumerable<TEntity>>> GetAll();

        Task<ApiResult<TEntity>> GetByID(dynamic Id);
        Task<ApiResult<IEnumerable<TEntity>>> Get<TEntity2>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity2>> order);
        Task<ApiResult<IEnumerable<TEntity>>> Get(Expression<Func<TEntity, bool>> predicate);

        Task<ApiResult<TEntity>> SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<ApiResult<IEnumerable<TType>>> GetSelectedDataAsync<TType>(Expression<Func<TEntity, TType>> select);
        Task<ApiResult<IEnumerable<TType>>> GetSelectedDataAsync<TType>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TType>> select);
        Task<ApiResultCode> Add(TEntity entity);

        ApiResultCode Update(TEntity entity);
        ApiResultCode UpdateAll(IEnumerable<TEntity> entities);
        Task<ApiResultCode> AddAll(IEnumerable<TEntity> entities);
        Task<ApiResultCode> Remove(Guid Id);
        Task<ApiResultCode> RemoveRange(IEnumerable<TEntity> entities);
        Task<ApiResult<bool>> Exists(Expression<Func<TEntity, bool>> predicate);
        Task<ApiResult<int>> Count(Expression<Func<TEntity, bool>> predicate);

        Task<ApiResult<IEnumerable<TEntity>>> GetPagedRecords(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, string>> orderBy, int pageNo, int pageSize);
        //Task<ApiResult<IEnumerable<TEntity>>> ExecWithStoreProcedure(string query, params object[] parameters);
    }
}
