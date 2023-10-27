using System.Threading.Tasks;
using Core.Common;
using Application.DTOs.Response;
using Application.DTOs.Request;

namespace Application.Interfaces.DomainServices
{
    public interface INotificationsService
    {
        Task<PaginatedList<NotificationResponse>> GetRecentNotifications(PaginatedRequest request);
        Task ReadRecentNotifications();
    }
}