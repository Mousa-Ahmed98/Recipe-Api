using System;
using System.Text;
using System.Threading.Tasks;
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

                // When using the WebSocket protocol with SignalR, it doesnt include Bearer token as a header
                // instead, it is appended to the request URL as an 'access_token' query parameter.
                // here we handle this by getting the token off the request's query 
                // and set its value to the context.
                // this helped <3 https://stackoverflow.com/a/57460785 
                opts.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/notifications")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddHttpContextAccessor();
            services.AddScoped<IUserSession, Session>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
