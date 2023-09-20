using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Infrastructure.Repositories.Implementation
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        internal StoreContext _context;
        internal DbSet<TEntity> _dbSet;
        internal string _userId = null!;
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

        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            _context.AddRange(entities);
        }

        public async virtual void DeleteById(int id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
            await SaveChangesAsync();
        }

        public async virtual void Delete(TEntity entityToDelete)
        {
            _context.Set<TEntity>().Remove(entityToDelete);
            _context.SaveChanges();
        }

        public async void DeleteRange(IEnumerable<TEntity> entities)
        {
            _context.RemoveRange(entities);
            await SaveChangesAsync();
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _context.Update(entityToUpdate);
            _context.SaveChanges();
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.UpdateRange(entities);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void SetUserId(string userId)
        {
            _userId = userId;
        }

        public void ThrowIfUserIdNull()
        {
            if (_userId == null) { throw new ArgumentNullException("UserId Was not set!"); }
        }
    }
}
