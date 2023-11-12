using Core.Common;
using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ICommentsRepository : IBaseRepository<Comment>
    {
        Task<PaginatedList<Comment>> GetComments(int recipeId, int pageNumber, int pageSize);
    }
}
