using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Entities;

using Infrastructure.Common;
using Infrastructure.CustomModels;
using Infrastructure.Data;
using Infrastructure.Exceptions.User;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Implementation
{
    public class UsersRepository : BaseRepository<ApplicationUser>, IUsersRepository
    {
        public UsersRepository(StoreContext context) : base(context)
        {
        }

        public async Task<PaginatedList<UserResponse>> GetUsers(string userId, int pageNumber, int pageSize)
        {
            var query = _context.Users.Select(x => new UserResponse
            {
                FirstName= x.FirstName,
                LastName= x.LastName,
                UserName= x.UserName,
                IsFollowed = _context.Follows
                    .Any(f => f.FollowerId == userId && f.FolloweeId == x.Id),
                Followers = _context.Follows
                    .Count(f => f.FolloweeId == x.Id),
                Following = _context.Follows
                    .Count(f => f.FollowerId == x.Id),
            });

            return await PaginatedList<UserResponse>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<UserResponse> GetByUsername(string userId, string username)
        {
            var user = await _context.Users
                .Select(x => new UserResponse
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    IsFollowed = _context.Follows
                        .Any(f => f.FollowerId == userId && f.FolloweeId == x.Id),
                    Followers = _context.Follows
                        .Count(f => f.FolloweeId == x.Id),
                    Following = _context.Follows
                        .Count(f => f.FollowerId == x.Id),
                })
                .FirstOrDefaultAsync(x => x.UserName == username);
            
            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            return user;
        }

        public async Task FollowUser(string userId, string username)
        {
            var followee = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
            
            if(followee == null)
            {
                throw new UserNotFoundException(username);
            }

            var follow = await _context.Follows
                .FirstOrDefaultAsync(x => x.FollowerId == userId && x.FolloweeId == followee.Id);

            if(follow != null)
            {
                throw new FollowAlreadyExistsException(username);
            }

            var newFollow = new Follow
            {
                FolloweeId = followee.Id,
                FollowerId = userId,
            };

            await _context.Follows.AddAsync(newFollow);
            await _context.SaveChangesAsync();
        }

        public async Task UnfollowUser(string userId, string username)
        {
            var followee = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (followee == null)
            {
                throw new UserNotFoundException(username);
            }

            var follow = await _context.Follows
                .FirstOrDefaultAsync(x => x.FollowerId == userId && x.FolloweeId == followee.Id);

            if (follow == null)
            {
                throw new FollowDoesntExistException(username);
            }

            _context.Follows.Remove(follow);
            await _context.SaveChangesAsync();
        }
    }
}
