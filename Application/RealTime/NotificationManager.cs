using System.Collections.Generic;
using System.Threading.Tasks;

using Core.Entities;
using Core.Enums;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Application.DTOs.Response;
using System;
using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Core.CustomModels;

namespace Application.RealTime
{
    public class NotificationManager : INotificationManager
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly INotificationSender _notificationSender;
        public NotificationManager(
            INotificationSender notificationSender,
            IServiceProvider serviceProvider)
        {
            _notificationSender = notificationSender;
            _serviceProvider = serviceProvider;
        }

        public async Task CreateMany
            (List<string> usersIds, RecipeSummary recipe, NotificationType type)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StoreContext>();

            // save notifications
            foreach (var id in usersIds)
            {
                await context.Notifications.AddAsync(new Notification()
                {
                    UserId = id,
                    RecipeId = recipe.Id,
                    Type = type
                });
            }

            await context.SaveChangesAsync();
            context.Dispose();

            // notify connected users in realtime 
            await _notificationSender.NotifyMany(
                userIds: usersIds,
                notification: new NotificationResponse(){
                    Type = type,
                    Recipe = recipe, 
                });
        }

        public async Task CreateOne
            (string userId, RecipeSummary recipe, NotificationType type)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<StoreContext>();

            // save a notification
            await context.Notifications.AddAsync(new Notification()
            {
                UserId = userId,
                RecipeId = recipe.Id,
                Type = NotificationType.NewPost
            });

            await context.SaveChangesAsync();
            context.Dispose();

            // notify connected user in realtime 
            await _notificationSender.NotifyOne(
                userId: userId,
                notification: new NotificationResponse()
                {
                    Recipe = recipe,
                    Type = type
                });
        }
    }
}
