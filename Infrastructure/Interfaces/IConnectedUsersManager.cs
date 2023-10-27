using System.Collections.Generic;

namespace Infrastructure.Interfaces
{
    public interface IConnectedUsersManager
    {
        void AddUser(string userId, string connectionId);
        void RemoveUser(string userId);
        string? GetUserConnectionId(string userId);
        List<string> GetUsersConnectionIds(List<string> userIds);
    }
}
