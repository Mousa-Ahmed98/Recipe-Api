using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null!,
            string includeProperties = "",
            bool tracked = false
            );
        
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        void Update(TEntity entityToUpdate);
        void UpdateRange(IEnumerable<TEntity> entities);

        Task DeleteById(int id);
        void Delete(TEntity entityToDelete);
        void DeleteRange(IEnumerable<TEntity> entities);

        Task<int> SaveChangesAsync();
    }
}
