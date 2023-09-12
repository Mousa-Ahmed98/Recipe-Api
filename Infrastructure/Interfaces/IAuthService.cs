using Core.Entities;
using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResponse> RegisterAsync(RegisterModel model);
        Task<AuthModel> TokenRequestAsync(TokenRequestModel model);
    }
}
