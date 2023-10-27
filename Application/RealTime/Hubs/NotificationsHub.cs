using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.SignalR;

using Infrastructure.Interfaces;
using Application.Interfaces;

namespace Application.Hubs
{
    public class NotificationsHub : Hub
    {
        private readonly IConnectedUsersManager _connectedUsersManager;
        private readonly IUserSession _userSession;
        public NotificationsHub(
            IConnectedUsersManager connectedUsersManager,
            IUserSession userSession
            )
        {
            _connectedUsersManager = connectedUsersManager;
            _userSession = userSession;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            if (_userSession.IsAuthenticated) { 
                _connectedUsersManager.AddUser(_userSession.UserId, Context.ConnectionId);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
            _connectedUsersManager.RemoveUser(_userSession.UserId);
        }
    }
}
