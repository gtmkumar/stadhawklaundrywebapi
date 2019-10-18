using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using StadhawkCoreApi;
using StadhawkCoreApi.Logger;
using StadhawkLaundry.BAL.Core.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace StadhawkLaundry.BAL.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            this.Context = context;
        }

        public virtual async Task<ApiResultCode> Add(TEntity entity)
        {
            try
            {
                await Context.Set<TEntity>().AddAsync(entity);
                return new ApiResultCode(ApiResultType.Success, 1, "Added successfully");
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Error During Saving");
            }
        }

        public virtual async Task<ApiResultCode> AddAll(IEnumerable<TEntity> entities)
        {
            try
            {
                await Context.Set<TEntity>().AddRangeAsync(entities);
                return new ApiResultCode(ApiResultType.Success, 1, "All items added successfully");
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Error During List Saving");
            }
        }

        public virtual async Task<ApiResultCode> Remove(dynamic Id)
        {
            try
            {
                var result = await GetByID(Id);
                if (result.HasSuccess)
                {
                    Context.Set<TEntity>().Remove(result.UserObject);
                    return new ApiResultCode(ApiResultType.Success, 1, "Deleted successfully");

                }
                return new ApiResultCode(ApiResultType.Error, 0, "Error during delete");
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Error during item delete");
            }
        }

        public virtual async Task<ApiResultCode> RemoveRange(IEnumerable<TEntity> entities)
        {
            try
            {
                Context.Set<TEntity>().RemoveRange(entities);
                await Context.SaveChangesAsync();
                return new ApiResultCode(ApiResultType.Success, 1, "Deleted successfully");
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Error during item delete");
            }

        }

        public virtual ApiResultCode Update(TEntity entity)
        {
            try
            {
                Context.Set<TEntity>().Attach(entity);
                Context.Entry(entity).State = EntityState.Modified;
                return new ApiResultCode(ApiResultType.Success, 1, "Item updated successfully");
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Error during item update");

            }
        }

        public virtual ApiResultCode UpdateAll(IEnumerable<TEntity> entities)
        {
            try
            {
                Context.Set<TEntity>().AttachRange(entities);
                return new ApiResultCode(ApiResultType.Success, 1, "Items updated successfully");
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator");
            }
        }

        public virtual async Task<ApiResult<IEnumerable<TEntity>>> Get<TEntity2>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TEntity2>> order)
        {
            try
            {
                var result = await Context.Set<TEntity>().AsNoTracking().Where(predicate).OrderBy(order).ToListAsync();
                if (result == null)
                    return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));

                return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "No data in given request"));
            }
        }

        public virtual async Task<ApiResult<IEnumerable<TEntity>>> GetAll()
        {
            try
            {
                var result = await Context.Set<TEntity>().ToListAsync();
                if (result == null)
                    return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.Error, 1, "No data in given request"));

                return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }

        public virtual async Task<ApiResult<TEntity>> GetByID(dynamic Id)
        {
            try
            {
                var result = await Context.Set<TEntity>().FindAsync(Id);
                if (result == null)
                    return new ApiResult<TEntity>(new ApiResultCode(ApiResultType.Error, 0, "No data fount in ginven request"));

                return new ApiResult<TEntity>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<TEntity>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }

        public virtual async Task<ApiResult<TEntity>> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var result = await Context.Set<TEntity>().SingleOrDefaultAsync(predicate);
                if (result == null)
                    return new ApiResult<TEntity>(new ApiResultCode(ApiResultType.Error, 0, "Data not found"));

                return new ApiResult<TEntity>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<TEntity>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "Please contact system administrator"));
            }
        }

        public virtual async Task<ApiResult<IEnumerable<TEntity>>> Get(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var result = await Context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();
                if (result == null)
                    return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));

                return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "No data in given request"));
            }
        }

        public virtual async Task<ApiResult<IEnumerable<TType>>> GetSelectedDataAsync<TType>(Expression<Func<TEntity, TType>> select)
        {
            try
            {
                var result = await Context.Set<TEntity>().Select(select).ToListAsync();

                if (result == null)
                    return new ApiResult<IEnumerable<TType>>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));

                return new ApiResult<IEnumerable<TType>>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<TType>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "No data in given request"));
            }
        }

        public virtual async Task<ApiResult<IEnumerable<TType>>> GetSelectedDataAsync<TType>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TType>> select)
        {
            try
            {
                var result = await Context.Set<TEntity>().Where(predicate).Select(select).ToListAsync();

                if (result == null)
                    return new ApiResult<IEnumerable<TType>>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));

                return new ApiResult<IEnumerable<TType>>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<TType>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "No data in given request"));
            }
        }

        public virtual async Task<ApiResult<bool>> Exists(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var result = await Context.Set<TEntity>().AnyAsync(predicate);
                return new ApiResult<bool>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<bool>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "No data in given request"));
            }
        }

        public virtual async Task<ApiResult<int>> Count(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var result = await Context.Set<TEntity>().Where(predicate).CountAsync();

                if (result <= 0)
                    return new ApiResult<int>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));

                return new ApiResult<int>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<int>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "No data in given request"));
            }
        }

        public virtual async Task<ApiResult<IEnumerable<TEntity>>> GetPagedRecords(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, string>> orderBy, int pageNo, int pageSize)
        {
            try
            {
                var result = await Context.Set<TEntity>().Where(predicate).OrderBy(orderBy).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

                if (result == null)
                    return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));

                return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "No data in given request"));
            }
        }

        public async Task<ApiResult<TType>> GetSelectedAsync<TType>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TType>> select)
        {
            try
            {
                var result = await Context.Set<TEntity>().Where(predicate).Select(select).FirstOrDefaultAsync();

                if (result == null)
                    return new ApiResult<TType>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));

                return new ApiResult<TType>(new ApiResultCode(ApiResultType.Success), result);
            }
            catch (Exception ex)
            {
                ErrorTrace.Logger(LogArea.BusinessTier, ex);
                return new ApiResult<TType>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "No data in given request"));
            }
        }

        //public virtual async Task<ApiResult<IEnumerable<TEntity>>> ExecWithStoreProcedure(string query, params object[] parameters)
        //{
        //    try
        //    {
        //        var result = await Context.Database.ExecuteSqlCommandAsync<TEntity>(query, parameters).ToListAsync();

        //        if (result == null)
        //            return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.Error, 0, "No data in given request"));

        //        return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.Success), result);
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorTrace.Logger(LogArea.BusinessTier, ex);
        //        return new ApiResult<IEnumerable<TEntity>>(new ApiResultCode(ApiResultType.ExceptionDuringOpration, 3, "No data in given request"));
        //    }
        //}
    }
}
