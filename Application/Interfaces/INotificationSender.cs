using Application.DTOs.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface INotificationSender
    {
        Task NotifyOne(string userId, NotificationResponse notification);
        Task NotifyMany(List<string> userIds, NotificationResponse notification);
    }
}
