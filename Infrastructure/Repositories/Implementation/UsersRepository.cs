using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Common;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Implementation
{
    public class UsersRepository : BaseRepository<ApplicationUser>, IUsersRepository
    {
        public UsersRepository(StoreContext context) : base(context)
        {
        }

        public async Task<PaginatedList<ApplicationUser>> GetUsers
            (string? userId, int pageNumber, int pageSize)
        {
            var query = _context.Users.Select(x => new ApplicationUser
            {
                FirstName= x.FirstName,
                LastName= x.LastName,
                UserName= x.UserName,
                Email = x.Email,
                IsFollowed = _context.Follows
                    .Any(f => f.FollowerId == userId && f.FolloweeId == x.Id),
                Followers = _context.Follows
                    .Count(f => f.FolloweeId == x.Id),
                Following = _context.Follows
                    .Count(f => f.FollowerId == x.Id),
            });

            return await PaginatedList<ApplicationUser>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<ApplicationUser?> GetByUsername(string? userId, string username)
        {
            var user = await _context.Users
                .Select(x => new ApplicationUser
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    Email = x.Email,
                    IsFollowed = _context.Follows
                        .Any(f => f.FollowerId == userId && f.FolloweeId == x.Id),
                    Followers = _context.Follows
                        .Count(f => f.FolloweeId == x.Id),
                    Following = _context.Follows
                        .Count(f => f.FollowerId == x.Id),
                })
                .FirstOrDefaultAsync(x => x.UserName == username);
            
            return user;
        }

        public async Task AddFollow(string followerId, string followeeId)
        {
            var newFollow = new Follow
            {
                FolloweeId = followerId,
                FollowerId = followeeId,
            };

            await _context.Follows.AddAsync(newFollow);
        }

        public async Task RemoveFollow(string followerId, string followeeId)
        {
            var follow = await _context.Follows
                .FirstAsync(x => x.FollowerId == followerId && x.FolloweeId == followeeId);

            _context.Follows.Remove(follow);
        }

        public async Task<bool> IsFollowedBy(string followerId, string followeeId)
        {
            return await _context.Follows
                .AnyAsync(x => x.FollowerId == followerId && x.FolloweeId == followeeId);
        }
    }
}
