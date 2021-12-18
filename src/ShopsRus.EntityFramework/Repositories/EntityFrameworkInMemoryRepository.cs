using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopsRus.Domain.Core;

namespace ShopsRus.EntityFramework.Repositories
{
    public class EntityFrameworkInMemoryRepository<TEntity, TKey>: RepositoryBase<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        private readonly ShopsRusDbContext _dbContext;

        public EntityFrameworkInMemoryRepository(ShopsRusDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbSet<TEntity> DbSet => _dbContext?.Set<TEntity>() ?? throw new Exception("DbSet Error!");

        public override async Task<TEntity> InsertAsync(TEntity entity)
        {
            if (entity is IHaveCreationTime time)
            {
                time.CreationTime = DateTime.Now;
            }

            DbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public override async Task DeleteAsync(TEntity entity)
        {
            DbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public override async Task<TEntity> GetAsync(TKey id)
        {
            return await DbSet.FindAsync(id);
        }

        public override async Task<IReadOnlyList<TEntity>> GetList()
        {
            return await DbSet.ToListAsync();
        }

        public override IQueryable<TEntity> GetQueryable()
        {
            return DbSet.AsQueryable();
        }
    }
    
    public class EntityFrameworkInMemoryRepository<TEntity>: EntityFrameworkInMemoryRepository<TEntity, int>, IRepository<TEntity> 
        where TEntity : class, IEntity<int>
    {
        public EntityFrameworkInMemoryRepository(ShopsRusDbContext dbContext)
            : base(dbContext)
        {
        }

        public override async Task<TEntity> InsertAsync(TEntity entity)
        {
            if (entity.Id == default)
            {
                if (! await GetQueryable().AnyAsync())
                {
                    entity.Id = 1;
                }
                else
                {
                    var max = (await GetQueryable().MaxAsync(x => x.Id));
                    entity.Id = max + 1;
                }
            }
            
            return await base.InsertAsync(entity);
        }
    }
}