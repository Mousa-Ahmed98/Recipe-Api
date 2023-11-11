using Core.Common;
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IRatingsRepository : IBaseRepository<Rating>
    {
        Task<PaginatedList<Rating>> GetByRecipeId(int recipeId, int pageNumber, int pageSize);
        Task<bool> RatedAlready(string userId, int recipId); 
        Task<Rating?> GetByUserId(string userId, int recipeId);
    }
}
