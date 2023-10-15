
using System.Linq.Expressions;
using System.Text;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Pupil.Model;

namespace Pupil.DataLayer
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal DbContext Context;
        internal DbSet<TEntity> DbSet;

        public IQueryable<TEntity> DbSetQuerable => DbSet.AsQueryable();

        public GenericRepository(DbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        #region Private Methods
        #endregion

        //public virtual IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)
        //{
        //    return DbSet.SqlQuery(query, parameters).ToList();
        //}

        //public virtual async Task<IEnumerable<TEntity>> GetWithRawSqlAsync(string query, params object[] parameters)
        //{
        //    return await DbSet.SqlQuery(query, parameters).ToListAsync();
        //}

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public virtual IEnumerable<TResult> GetWithProjection<TResult>(
                Expression<Func<TEntity, TResult>> selector,
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;
            IQueryable<TResult> resultQuery = null;

            try
            {
                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                resultQuery = query.Select(selector);

                return resultQuery.ToList();
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private Exception HandleDbUpdateException(DbUpdateException dbu)
        {
            var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");

            try
            {
                foreach (var result in dbu.Entries)
                {
                    builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name);
                }
            }
            catch (Exception e)
            {
                builder.Append("Error parsing DbUpdateException: " + e.ToString());
            }

            string message = builder.ToString();
            return new Exception(message, dbu);

        }

        public virtual IEnumerable<TEntity> GetCacheable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,

            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;

                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                if (orderBy != null)
                {
                    return await orderBy(query).ToListAsync();
                }

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetCacheableAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,

            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;

                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                if (orderBy != null)
                {
                    return await orderBy(query).ToListAsync();
                }

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public virtual async Task<IEnumerable<TEntity>> GetSearchAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;

                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                if (orderBy != null)
                {
                    if (skip.HasValue && take.HasValue)
                    {
                        return await orderBy(query).Skip(skip.Value).Take(take.Value).ToListAsync();
                    }
                    return await orderBy(query).AsQueryable().ToListAsync();
                }

                return await query.ToListAsync();
                
            }
            catch (Exception)
            {
                throw;
            }
        }



        public virtual IEnumerable<TEntity> GetSearch(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null,
          params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;

                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                if (orderBy != null)
                {
                    if (skip.HasValue && take.HasValue)
                    {
                        return orderBy(query).Skip(skip.Value).Take(take.Value).ToList();
                    }
                    return orderBy(query).ToList();
                }

                return query.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetOrderedSearchAsync(
          Expression<Func<TEntity, bool>> filter = null,
          List<OrderByColumns> orderByColumns = null, int? skip = null, int? take = null,
          params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;

                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                if (orderByColumns != null)
                {
                    foreach (var item in orderByColumns)
                    {
                        if (!string.IsNullOrEmpty(item.OrderType) && item.OrderType.ToUpper() == "ASC")
                            query = OrderingHelper(query, item.ColumnName, false, false);
                        else if (!string.IsNullOrEmpty(item.OrderType) && item.OrderType.ToUpper() == "DESC")
                            query = OrderingHelper(query, item.ColumnName, true, false);

                    }
                    if (skip.HasValue && take.HasValue)
                    {
                        return await query.Skip(skip.Value).Take(take.Value).ToListAsync();

                    }

                }

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public virtual IEnumerable<TEntity> GetOrderedSearch(
         Expression<Func<TEntity, bool>> filter = null,
         List<OrderByColumns> orderByColumns = null, int? skip = null, int? take = null,
         params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;

                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                if (orderByColumns != null)
                {
                    foreach (var item in orderByColumns)
                    {
                        if (!string.IsNullOrEmpty(item.OrderType) && item.OrderType.ToUpper() == "ASC")
                            query = OrderingHelper(query, item.ColumnName, false, false);
                        else if (!string.IsNullOrEmpty(item.OrderType) && item.OrderType.ToUpper() == "DESC")
                            query = OrderingHelper(query, item.ColumnName, true, false);

                    }
                    if (skip.HasValue && take.HasValue)
                    {
                        return query.Skip(skip.Value).Take(take.Value).ToList();

                    }

                }

                return query.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public virtual IEnumerable<TResult> GetOrderedSearchWithProjection<TResult>(
                Expression<Func<TEntity, TResult>> selector,
                Expression<Func<TEntity, bool>> filter = null,
                List<OrderByColumns> orderByColumns = null, int? skip = null, int? take = null,
                params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;
            IQueryable<TResult> resultQuery = null;

            try
            {
                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                resultQuery = query.Select(selector);

                if (orderByColumns != null)
                {
                    foreach (var item in orderByColumns)
                    {
                        if (!string.IsNullOrEmpty(item.OrderType) && item.OrderType.ToUpper() == "ASC")
                            resultQuery = OrderingHelper(resultQuery, item.ColumnName, false, false);
                        else if (!string.IsNullOrEmpty(item.OrderType) && item.OrderType.ToUpper() == "DESC")
                            resultQuery = OrderingHelper(resultQuery, item.ColumnName, true, false);

                    }
                    if (skip.HasValue && take.HasValue)
                    {
                        return resultQuery.Skip(skip.Value).Take(take.Value).ToList();
                    }
                }


                return resultQuery.ToList();
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private static IOrderedQueryable<T> OrderingHelper<T>(IQueryable<T> source, string propertyName, bool descending, bool anotherLevel)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), string.Empty); // I don't care about some naming
            MemberExpression property = Expression.PropertyOrField(param, propertyName);
            LambdaExpression sort = Expression.Lambda(property, param);
            MethodCallExpression call = Expression.Call(
                typeof(Queryable),
                (!anotherLevel ? "OrderBy" : "ThenBy") + (descending ? "Descending" : string.Empty),
                new[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(sort));
            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(call);
        }


        public virtual async Task<int> GetSearchCountAsync(
          Expression<Func<TEntity, bool>> filter = null,
          params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            try
            {
                return await query.CountAsync();
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public virtual int GetSearchCount(
         Expression<Func<TEntity, bool>> filter = null,
         params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            try
            {
                return query.Count();
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public virtual async Task<IEnumerable<TEntity>> GetCacheableSearchAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,

            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;

                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                if (orderBy != null)
                {
                    var orderedQuery = orderBy(query);
                    return await orderBy(query).ToListAsync();
                }

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual IEnumerable<TEntity> GetAsNoTracking(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty).AsNoTracking());

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public virtual IEnumerable<TEntity> GetCacheableAsNoTracking(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty).AsNoTracking());

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;

                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty).AsNoTracking());

                if (orderBy != null)
                {
                    return await orderBy(query).ToListAsync();
                }

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /*public virtual async Task<IEnumerable<TEntity>> GetCacheableAsNoTrackingAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;

                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty).AsNoTracking());

                if (orderBy != null)
                {
                    return await orderBy(query).ToListAsync();
                }

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }*/

        public virtual async Task<IEnumerable<TEntity>> GetSearchAsNoTrackingAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;

                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty).AsNoTracking());

                if (orderBy != null)
                {
                    var orderedQuery = orderBy(query);
                    return await orderBy(query).ToListAsync();
                }

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetCacheableSearchAsNoTrackingAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,

            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            try
            {
                IQueryable<TEntity> query = DbSet;

                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty).AsNoTracking());

                if (orderBy != null)
                {
                    var orderedQuery = orderBy(query);
                    return await orderBy(query).ToListAsync();
                }

                return await query.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual TEntity GetById(object id)
        {
            return DbSet.Find(id);
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public virtual void InsertRange(List<TEntity> entity)
        {
            DbSet.AddRange(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = DbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entitiesToDelete)
        {
            entitiesToDelete.ToList().ForEach(e =>
            {
                if (Context.Entry(e).State == EntityState.Detached)
                {
                    DbSet.Attach(e);
                }
            });

            DbSet.RemoveRange(entitiesToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual IEnumerable<TEntity> GetWithProjectionAsNoTracking(
           Expression<Func<TEntity, bool>> filter = null,
           Expression<Func<TEntity, TEntity>> selector = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            if (filter != null)
            {
                query = query.AsExpandable().Where(filter);
            }

            if (selector != null)
            {
                query = query.Select(selector);
            }

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)).AsNoTracking();

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public virtual async Task<IEnumerable<TResult>> GetWithProjectionAsync<TResult>(
           Expression<Func<TEntity, TResult>> selector,
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;
            IQueryable<TResult> resultQuery = null;

            try
            {
                if (filter != null)
                {
                    query = query.AsExpandable().Where(filter);
                }

                query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                resultQuery = query.Select(selector);

                return await resultQuery.ToListAsync();
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = DbSet;

            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return await query.ToListAsync();
        }
    }
}
