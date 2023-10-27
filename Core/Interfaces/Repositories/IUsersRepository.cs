using System.Threading.Tasks;
using Core.Common;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IUsersRepository : IBaseRepository<ApplicationUser>
    {
        Task<PaginatedList<ApplicationUser>> GetUsers(string? userId, int pageNumber, int pageSize);
        Task<ApplicationUser?> GetByUsername(string? userId, string username);
        Task AddFollow(string userId, string followeeId);
        Task RemoveFollow(string userId, string followeeId);
        Task<bool> IsFollowedBy(string followerId, string followeeId);
    }
}
