using Core.Entities;
using Infrastructure.Common;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterModel model);
        Task<AuthModel> TokenRequestAsync(TokenRequestModel model);
    }
}
