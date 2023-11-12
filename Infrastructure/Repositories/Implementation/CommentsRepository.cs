using Core.Common;
using System.Linq;

using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementation
{
    public class CommentsRepository : BaseRepository<Comment>, ICommentsRepository
    {
        public CommentsRepository(StoreContext context) : base(context)
        {
           
        }
        public async Task<PaginatedList<Comment>> GetComments(int recipeId, int pageNumber, int pageSize)
        {
            var query = _context.Comments
                .Include(x => x.User)
                .Include(x => x.Replies)
                    .ThenInclude(x => x.User)
                .Where(x => x.RecipeId == recipeId);

            return await PaginatedList<Comment>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}
