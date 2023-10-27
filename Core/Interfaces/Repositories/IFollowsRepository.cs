using System.Collections.Generic;
using System.Threading.Tasks;

using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IFollowsRepository : IBaseRepository<Follow>
    {
        Task<List<ApplicationUser>> GetFollowersByUserId(string userId);
    }
}
