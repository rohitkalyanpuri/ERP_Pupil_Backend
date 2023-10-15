using Pupil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace Pupil.DataLayer
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Delete(object id);

        void Delete(TEntity entityToDelete);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TResult> GetWithProjection<TResult>(
                Expression<Func<TEntity, TResult>> selector,
                Expression<Func<TEntity, bool>> filter = null,
                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetCacheable(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,

            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetSearchAsync(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null,
           params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetSearch(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int? skip = null, int? take = null,
          params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetOrderedSearchAsync(
          Expression<Func<TEntity, bool>> filter = null,
          List<OrderByColumns> orderByColumns = null, int? skip = null, int? take = null,
          params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetOrderedSearch(
         Expression<Func<TEntity, bool>> filter = null,
         List<OrderByColumns> orderByColumns = null, int? skip = null, int? take = null,
         params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TResult> GetOrderedSearchWithProjection<TResult>(
                Expression<Func<TEntity, TResult>> selector,
                Expression<Func<TEntity, bool>> filter = null,
                List<OrderByColumns> orderByColumns = null, int? skip = null, int? take = null,
                params Expression<Func<TEntity, object>>[] includeProperties);

        Task<int> GetSearchCountAsync(
          Expression<Func<TEntity, bool>> filter = null,
          params Expression<Func<TEntity, object>>[] includeProperties);

        int GetSearchCount(
         Expression<Func<TEntity, bool>> filter = null,
         params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetCacheableSearchAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,

            params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetAsNoTracking(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetCacheableAsNoTracking(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetAsNoTrackingAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetSearchAsNoTrackingAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetCacheableSearchAsNoTrackingAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,

            params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> GetCacheableAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,

            params Expression<Func<TEntity, object>>[] includeProperties);
        TEntity GetById(object id);

        Task<TEntity> GetByIdAsync(object id);

        //IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters);

        //Task<IEnumerable<TEntity>> GetWithRawSqlAsync(string query, params object[] parameters);

        void Insert(TEntity entity);

        void Update(TEntity entityToUpdate);

        void InsertRange(List<TEntity> entity);

        void DeleteRange(IEnumerable<TEntity> entitiesToDelete);

        IEnumerable<TEntity> GetWithProjectionAsNoTracking(
           Expression<Func<TEntity, bool>> filter = null,
           Expression<Func<TEntity, TEntity>> selector = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TResult>> GetWithProjectionAsync<TResult>(
           Expression<Func<TEntity, TResult>> selector,
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
