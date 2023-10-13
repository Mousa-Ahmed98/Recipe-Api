using System.Threading.Tasks;

using Core.Common;
using Core.Entities;
using Core.Enums;

namespace Core.Interfaces.Repositories
{
    public interface INotificationsRepository : IBaseRepository<Notification>
    {
        Task<PaginatedList<Notification>> GetRecentNotifications(string userId, int pageNumber, int pageSize);
        Task ReadRecentNotifications(string userId);
        Task AddNewNotification(string userId, int recipeId, NotificationType type);
    }
}
