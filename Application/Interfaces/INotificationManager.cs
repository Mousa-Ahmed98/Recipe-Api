using Application.DTOs.Response;
using Core.CustomModels;
using Core.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    /// <summary>
    /// The whole purpose of the notification manager is to make the notifing
    /// indepenent from the request. By that we can manage this any way we like!
    /// </summary>
    public interface INotificationManager
    {
        Task CreateOne(string userId, RecipeSummary recipe, NotificationType type);
        Task CreateMany(List<string> usersIds, RecipeSummary recipe, NotificationType type);
    }
}
