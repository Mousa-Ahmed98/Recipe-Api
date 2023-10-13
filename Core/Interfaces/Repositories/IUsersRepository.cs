using System.Threading.Tasks;

using Core.Common;
using Core.CustomModels;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IUsersRepository : IBaseRepository<ApplicationUser>
    {
        Task<PaginatedList<UserResponse>> GetUsers(string userId, int pageNumber, int pageSize);
        Task<UserResponse> GetByUsername(string userId, string username);
        Task FollowUser(string userId, string username);
        Task UnfollowUser(string userId, string username);
    }
}
