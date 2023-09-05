using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        T Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
        IQueryable<T> Get(Expression<Func<T, bool>> filter = null,
                             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                             string includeProperties = "");
    }
}
