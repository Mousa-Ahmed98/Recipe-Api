using Core.Entities;
using Infrastructure.Common;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services.implementation
{
    public class JwtService : ITokenService
    {
        private readonly TokenConfiguration _tokenConfiguration;

        public JwtService(
            IOptions<TokenConfiguration> tokenConfiguration
            )
        {
            _tokenConfiguration = tokenConfiguration.Value;
        }

        public async Task<RefreshTokenResponse> GenerateAccessToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenConfiguration.Secret);
            var claims = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Role, user.Role),
                new("username", user.Username)
            });

            var expiryDate = DateTime.UtcNow
                .AddMinutes(_tokenConfiguration.DurationInMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _tokenConfiguration.Audience,
                Issuer = _tokenConfiguration.Issuer,
                Subject = claims,
                Expires = expiryDate,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    _tokenConfiguration.Algorithm
                    ),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new RefreshTokenResponse
            {
                Token = tokenHandler.WriteToken(token),
                ExpDate = expiryDate
            };
        }
    }
}
