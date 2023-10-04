using Application.Interfaces;
using Core;
using Core.Entities;
using Infrastructure.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterModel model)
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

        public async Task<AuthModel> TokenRequestAsync(TokenRequestModel model)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(model.Email);

            if(user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Incorrect email or password.";
                return authModel;
            }
            var jwtSecToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecToken);
            authModel.Message = "Success! Access granted.";
            authModel.UserId = user.Id;
            authModel.Email = user.Email;
            authModel.FirstName = user.FirstName;
            authModel.LastName = user.LastName;
            authModel.UserName = user.UserName;
            authModel.Expiration = jwtSecToken.ValidTo;
            authModel.Role = roles.FirstOrDefault() ?? "";


            return authModel;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
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
                new Claim("username", user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }
            .Union(userClaims)
            .Union(roleClaims);


            var authSecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("anscna@@#as321AaAcEEE37$%**$#2ffr@#fvf^(gkoeLAD8"));
            SigningCredentials credentials = new SigningCredentials(authSecretKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://localhost:3000",
                audience: "https://localhost:4200",
                expires: DateTime.Now.AddHours(3),
                claims: claims,
                signingCredentials: credentials
                );

            return token;
        }
    }
}
