using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Core.Common;
using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Implementation
{
    public class RatingsRepository : BaseRepository<Rating>, IRatingsRepository
    {
        public RatingsRepository(StoreContext context) : base(context)
        {
        }

        public async Task<PaginatedList<Rating>> GetByRecipeId
            (int recipeId, int pageNumber, int pageSize)
        {
            var query = _context.Ratings
                .Where(r => r.RecipeId == recipeId)
                .Include(r => r.User);

            return await PaginatedList<Rating>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<Rating?> GetByUserId(string userId, int recipeId)
        {
            return await _context.Ratings
                .FirstOrDefaultAsync(r => r.UserId == userId && r.RecipeId == recipeId);
        }

        public async Task<bool> RatedAlready(string userId, int recipeId)
        {
            return await _context.Ratings
                .AnyAsync(r => r.UserId == userId && r.RecipeId == recipeId);
        }
    }
}
