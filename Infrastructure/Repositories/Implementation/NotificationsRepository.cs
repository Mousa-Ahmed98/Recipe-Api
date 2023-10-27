using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Core.Entities;
using Core.Enums;
using Core.Common;
using Core.Interfaces.Repositories;

using Infrastructure.Data;

namespace Infrastructure.Repositories.Implementation
{
    public class NotificationsRepository : BaseRepository<Notification>, INotificationsRepository
    {
        public NotificationsRepository(StoreContext context) : base(context)
        {
        }
        public async Task<PaginatedList<Notification>> GetRecentNotifications
            (string userId, int pageNumber, int pageSize)
        {
            var query = _context.Notifications
                .Where(x => x.UserId.Equals(userId))
                .Include(x => x.Recipe)
                .OrderByDescending(x => x.CreatedAt);
            
            return await PaginatedList<Notification>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task ReadRecentNotifications(string userId)
        {
            await _context.Notifications
                .Where(x => x.UserId.Equals(userId) && x.ReadAt.Equals(null))
                .ExecuteUpdateAsync(x => 
                    x.SetProperty(p => p.ReadAt, DateTime.Now)
                    );
        }

        public async Task AddNewNotification(string userId, int recipeId, NotificationType type)
        {
            // just to prevent adding redundant and dublicate notifications 
            // we can later add a number field to Notification entity
            // and increase it as the user receives new comments or ratings.

            if (type == NotificationType.Comment || type == NotificationType.Rating)
            {
                var LastUnread = await _context.Notifications
                    .Where(x => x.RecipeId == recipeId && x.Type == type && x.ReadAt == null)
                    .FirstOrDefaultAsync();

                if(LastUnread != null)
                {
                    LastUnread.CreatedAt = DateTime.Now;
                    _context.Notifications.Update(LastUnread);
                    
                    return;
                }
            }
            else
            {
                _context.Notifications.Add( 
                    new Notification
                    {
                        RecipeId = recipeId,
                        Type = type,
                        UserId = userId,
                    }
                );
            }
        }
    }
}
