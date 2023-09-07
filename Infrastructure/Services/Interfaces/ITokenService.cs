using Core.Entities;
using Infrastructure.Common;

namespace Infrastructure.Services.Interfaces
{
    public interface ITokenService
    {
        Task<RefreshTokenResponse?> GenerateAccessToken(User user);
    }
}
