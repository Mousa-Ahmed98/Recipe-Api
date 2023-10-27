using System.Threading.Tasks;

using Core.Common;
using Core.CustomModels;
using Application.DTOs.Request;

namespace Application.Interfaces.DomainServices
{
    public interface IUserService
    {
        Task FollowUser(string username);
        Task UnfollowUser(string username);
        Task<UserResponse> GetByUsername(string username);
        Task<PaginatedList<UserResponse>> GetUsers(PaginatedRequest request);
    }
}