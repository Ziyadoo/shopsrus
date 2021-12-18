using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopsRus.Domain.Core
{
    public interface IRepository<TEntity, TKey>
    where TEntity: IEntity<TKey>
    {
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<TEntity> GetAsync(TKey id);
        Task<IReadOnlyList<TEntity>> GetList();
        IQueryable<TEntity> GetQueryable();
    }

    public interface IRepository<TEntity> : IRepository<TEntity, int> where TEntity : IEntity<int>
    {
        
    }
}