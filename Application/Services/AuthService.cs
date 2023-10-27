using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Core.CustomModels;
using Core.Entities;
using Core.Constants;
using Infrastructure.Common;

using Application.Interfaces;

namespace Infrastructure.Repositories.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenConfiguration _tokenConfiguration;
        public AuthService(
            UserManager<ApplicationUser> userManager,
            IOptions<TokenConfiguration> tokenConfiguration
            )
        {
            _userManager = userManager;
            _tokenConfiguration = tokenConfiguration.Value;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest model)
        {
            if (await _userManager.FindByNameAsync(model.UserName) != null)
                return new RegisterResponse { Message = "Username is already used." };
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new RegisterResponse { Message = "Email is already registered." };
            
            var user = new ApplicationUser 
            { 
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                StringBuilder errors = new StringBuilder();
                foreach (var item in result.Errors)
                {
                    errors.Append("\n");
                    errors.Append(item.Description);
                }
                return new RegisterResponse {Message = errors.ToString() };
            }

            await _userManager.AddToRoleAsync(user, Roles.User);

            return new RegisterResponse
            {
                Success = true,
                Message = "Signed Up successfully",
                User = new UserResponse
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                }
            };
        }

        public async Task<AuthResponse> TokenRequestAsync(TokenRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new AuthResponse
                {
                    Message = "Incorrect email or password."
                };
            }

            var jwtSecToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            var res = new AuthResponse()
            {
                IsAuthenticated = true,
                Message = "Success! Access granted.",
                
                UserId = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = roles.FirstOrDefault() ?? "",
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecToken),
                Expiration = jwtSecToken.ValidTo
            };

            return res;
        }

        public async Task<SecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("role", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("username", user.UserName),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var key = Encoding.ASCII.GetBytes(_tokenConfiguration.Secret);
            SigningCredentials credentials = new SigningCredentials(
                new SymmetricSecurityKey(key), _tokenConfiguration.Algorithm
                );
            
            var expiryDate = DateTime.UtcNow
                .AddMinutes(_tokenConfiguration.DurationInMinutes);

            JwtSecurityToken token = new (
                issuer: _tokenConfiguration.Issuer,
                audience: _tokenConfiguration.Audience,
                expires: expiryDate,
                claims: claims,
                signingCredentials: credentials
            );

            return token;
        }
    }
}
