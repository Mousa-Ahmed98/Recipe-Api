using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

using Infrastructure.Interfaces;
using Application.DTOs.Response;
using Application.Hubs;
using Application.Interfaces.DomainServcies;

namespace Application.RealTime
{
    public class NotificationsSender : INotificationSender
    {
        private readonly IHubContext<NotificationsHub> _hubContext;
        private readonly IConnectedUsersManager _connectedUsers;
        private readonly ILogger<NotificationsSender> _logger;

        public NotificationsSender(
            IHubContext<NotificationsHub> hubContext,
            IConnectedUsersManager connectedUsers,
            ILogger<NotificationsSender> logger
            )
        {
            _hubContext = hubContext;
            _connectedUsers = connectedUsers;
            _logger = logger;
        }

        public async Task NotifyOne(string userId, NotificationResponse notification)
        {
            try
            {
                var connectionId = _connectedUsers.GetUserConnectionId(userId);

                if(connectionId == null ) { return; } // not connected return

                await _hubContext.Clients.Client(connectionId)
                    .SendAsync("getNotified", notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task NotifyMany(List<string> userIds, NotificationResponse notification)
        {
            try
            {
                var connectionIds = _connectedUsers.GetUsersConnectionIds(userIds);

                await _hubContext.Clients.Clients(connectionIds)
                    .SendAsync("getNotified", notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
