using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetById(int id);
        T Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(T entity);
    }
}
