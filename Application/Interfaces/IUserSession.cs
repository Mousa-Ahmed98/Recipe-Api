using System;

namespace Application.Interfaces
{
    public interface IUserSession
    {
        string UserId { get; }
        string Username { get; }
        string Email { get; }
        bool IsAuthenticated { get; }
        bool IsAdmin { get; }
    }
}
