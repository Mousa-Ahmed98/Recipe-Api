using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using Infrastructure.Common;
using Infrastructure.Repositories.Implementation;

using Application.Interfaces;
using Application.UserSession;


namespace RecipeApi.Configurations
{
    public static class AuthenticationConfiguration
    {
        public static IServiceCollection ConfigureAuthentication
            (this IServiceCollection services, IConfiguration configuration)
        {
            var tokenConfigurationSection = configuration.GetSection("TokenConfiguration");

            // register TokenConfiguration options
            services.Configure<TokenConfiguration>(
                tokenConfigurationSection
            );

            var tokenConfig = tokenConfigurationSection.Get<TokenConfiguration>();
            var key = Encoding.UTF8.GetBytes(tokenConfig!.Secret);

            var tokenValidationParams = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidIssuer = tokenConfig.Issuer,
                ValidAudience = tokenConfig.Audience,
                ClockSkew = TimeSpan.Zero
            };

            // configure jwt authentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
            {
                opts.SaveToken = true;
                opts.RequireHttpsMetadata = false;
                opts.TokenValidationParameters = tokenValidationParams;
            });

            services.AddHttpContextAccessor();
            services.AddScoped<IUserSession, Session>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
