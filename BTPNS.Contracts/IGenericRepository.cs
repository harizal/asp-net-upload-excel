using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BTPNS.Contracts
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        void Delete(object id);

    void Delete(TEntity entityToDelete);

    void DeleteRange(IEnumerable<TEntity> entitiesToDelete);

    IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);

    IQueryable<TEntity> GetAsQueryable(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null);
    IQueryable<TEntity> GetAsQueryableWithPagination(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int skip = 0, int take = 10);

    TEntity GetByID(object id);

    TEntity GetByID(params object[] keyValues);

    void Insert(TEntity entity);

    void InsertRange(IEnumerable<TEntity> entities);

    void Update(TEntity entityToUpdate);

    void UpdateWithRelatedEntities(TEntity entity);

    object SetObjectStateToDetached(object obj);

    object SetObjectStateToAdded(object obj);

    bool Exists(
         Expression<Func<TEntity, bool>> filter = null);

    void DeleteRange(Expression<Func<TEntity, bool>> filter = null);

    int Count(Expression<Func<TEntity, bool>> filter = null);
    int Count();
}
}