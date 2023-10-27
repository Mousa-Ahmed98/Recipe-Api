using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Core.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Implementation
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        internal StoreContext _context;
        internal DbSet<TEntity> _dbSet;
        public BaseRepository(StoreContext context)
        {
            _context = context;
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null!,
            string includeProperties = "",
            bool tracked = false
        )
        {
            IQueryable<TEntity> query = tracked ? _dbSet.AsTracking() : _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            
            return await query.ToListAsync();
        }
        
        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _context.AddRangeAsync(entities);
        }

        public virtual async Task DeleteById(int id)
        {
            TEntity entityToDelete = await _dbSet.FindAsync(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            _context.Set<TEntity>().Remove(entityToDelete);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _context.RemoveRange(entities);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _context.Update(entityToUpdate);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.UpdateRange(entities);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
