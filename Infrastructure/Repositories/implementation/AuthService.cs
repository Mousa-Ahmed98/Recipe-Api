using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByNameAsync(model.UserName) != null)
                return new AuthModel { Message = "Username is already used." };
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new AuthModel { Message = "Email is already registered." };
            
            var user = new ApplicationUser 
            { 
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
                return new AuthModel { Message = errors.ToString() };
            }

            await _userManager.AddToRoleAsync(user, model.Role);
            var jwtSecToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            return new AuthModel
            {
                Id = user.Id,
                Email = user.Email,
                Expiration = jwtSecToken.ValidTo,
                IsAuthenticated = true,
                Role = roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecToken),
                UserName = user.UserName,
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
            authModel.Id = user.Id;
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.Expiration = jwtSecToken.ValidTo;
            authModel.Role = roles.ToList();


            return authModel;
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
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
