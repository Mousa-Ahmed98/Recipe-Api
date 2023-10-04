using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Infrastructure.CustomModels;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface INotificationsRepository : IBaseRepository<Notification>
    {
        Task<PaginatedList<Notification>> GetRecentNotifications(string userId, int pageNumber, int pageSize);
        Task ReadRecentNotifications(string userId);
        Task AddNewNotification(string userId, int recipeId, NotificationType type);
    }
}
