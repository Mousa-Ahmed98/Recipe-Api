using Core.Entities;
using Infrastructure.Common;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterRequest model);
        Task<AuthResponse> TokenRequestAsync(TokenRequest model);
    }
}
