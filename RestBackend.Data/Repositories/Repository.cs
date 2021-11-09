using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestBackend.Core.Repositories;

namespace RestBackend.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            this.Context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (orderBy != null)
                return await orderBy(query).FirstOrDefaultAsync(filter);

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<IQueryable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null) query = query.Where(filter);

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (orderBy != null)
                return orderBy(query);

            return query;
        }

        public async Task<IQueryable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (orderBy != null)
                return orderBy(query);

            return query;
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public Task<int> Count(Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null)
                query = query.Where(filter);

            return query.CountAsync();
        }

        public Task<List<TEntity>> GetPagedAsync(
            int page,
            int limit,
            string orderBy,
            bool isAscending = true,
            Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            page = (page < 0) ? 1 : page;
            var startRow = (page - 1) * limit;

            IQueryable<TEntity> query = Context.Set<TEntity>();

            if (filter != null) query = query.Where(filter);

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (!string.IsNullOrEmpty(orderBy))
                return OrderBy(query, orderBy, isAscending).Skip(startRow).Take(limit).ToListAsync();

            return query.Skip(startRow).Take(limit).ToListAsync();
        }

        private static IQueryable<T> OrderBy<T>(IQueryable<T> source, string columnName, bool isAscending = true)
        {
            try
            {
                if (String.IsNullOrEmpty(columnName))
                    return source;

                ParameterExpression parameter = Expression.Parameter(source.ElementType, "");

                MemberExpression property = Expression.Property(parameter, columnName);
                LambdaExpression lambda = Expression.Lambda(property, parameter);

                string methodName = isAscending ? "OrderBy" : "OrderByDescending";

                Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                      new Type[] { source.ElementType, property.Type },
                                      source.Expression, Expression.Quote(lambda));

                return source.Provider.CreateQuery<T>(methodCallExpression);
            }
            catch (Exception e)
            {
                throw new Exception($"Ordering param '{columnName}' is not valid.", e);
            }
        }

    }
}