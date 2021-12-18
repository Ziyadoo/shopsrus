using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRus.Domain.Core
{
    public abstract class RepositoryBase<TEntity> : RepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : IEntity<int>
    {
        
    }

    public abstract class RepositoryBase<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        public abstract Task<TEntity> InsertAsync(TEntity entity);
        public abstract Task<TEntity> UpdateAsync(TEntity entity);
        public abstract Task DeleteAsync(TEntity entity);
        public abstract Task<TEntity> GetAsync(TKey id);
        public abstract Task<IReadOnlyList<TEntity>> GetList();
        public abstract IQueryable<TEntity> GetQueryable();
    }
}