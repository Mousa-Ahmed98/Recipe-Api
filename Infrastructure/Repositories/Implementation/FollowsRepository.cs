using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Implementation
{
    public class FollowsRepository : BaseRepository<Follow>, IFollowsRepository
    {
        public FollowsRepository(StoreContext context) : base(context)
        {
        }

        public async Task<List<ApplicationUser>> GetFollowersByUserId(string userId)
        {
            return await _context.Follows
                .Where(x => x.FolloweeId == userId)
                .Join(_context.Users,
                    f => f.FollowerId,
                    u => u.Id,
                    (f, u) => u
                ).ToListAsync();
        }
    }
}
