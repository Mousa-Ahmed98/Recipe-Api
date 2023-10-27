using Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.RealTime
{
    public class ConnectedUsersManager : IConnectedUsersManager
    {
        private readonly Dictionary<string, string> connectedUsers = new Dictionary<string, string>();

        public void AddUser(string userId, string connectionId)
        {
            connectedUsers[userId] = connectionId;
        }

        public void RemoveUser(string userId)
        {
            connectedUsers.Remove(userId);
        }

        public string? GetUserConnectionId(string userId)
        {
            connectedUsers.TryGetValue(userId, out var connectionId);
            
            return connectionId;
        }        
        
        public List<string> GetUsersConnectionIds(List<string> userIds)
        {
            var connectionIds = connectedUsers
                .Where(x => userIds.Contains(x.Key))
                .Select(x => x.Value)
                .ToList();
            
            return connectionIds;
        }
    }
}
