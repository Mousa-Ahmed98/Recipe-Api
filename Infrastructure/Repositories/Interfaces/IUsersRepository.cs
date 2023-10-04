using Core.Entities;
using Core.Interfaces;
using Infrastructure.Common;
using Infrastructure.CustomModels;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IUsersRepository : IBaseRepository<ApplicationUser>
    {
        Task<PaginatedList<UserResponse>> GetUsers(string userId, int pageNumber, int pageSize);
        Task<UserResponse> GetByUsername(string userId, string username);
        Task FollowUser(string userId, string username);
        Task UnfollowUser(string userId, string username);
    }
}
