using Core.Entities;
using Core.Enums;
using Infrastructure.CustomModels;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementation
{
    public class NotificationsRepository : BaseRepository<Notification>, INotificationsRepository
    {
        public NotificationsRepository(StoreContext context) : base(context)
        {
        }
        public async Task<PaginatedList<Notification>> GetRecentNotifications(string userId, int pageNumber, int pageSize)
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
            var LastUnread = await _context.Notifications
                .Where(x => x.RecipeId == recipeId && x.Type == type && x.ReadAt == null)
                .FirstOrDefaultAsync();

            if(LastUnread != null)
            {
                LastUnread.CreatedAt = DateTime.Now;
                _context.Notifications.Update(LastUnread);
            }
            else
            {
                var newNotification = new Notification { 
                    RecipeId = recipeId, 
                    Type = type,
                    UserId = userId,
                };
                await _context.Notifications.AddAsync(newNotification);
            }

            await _context.SaveChangesAsync();
        }
    }
}
