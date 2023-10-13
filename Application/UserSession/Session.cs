using Application.Interfaces;
using Core.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;

namespace Application.UserSession
{
    public class Session : IUserSession
    {
        private readonly ClaimsPrincipal _user;
        public string UserId { get; }
        public string Username { get; }
        public string Email { get; }
        public bool IsAuthenticated { get; }
        public bool IsAdmin { get; }

        public Session(IHttpContextAccessor context)
        {
            _user =  context.HttpContext.User ?? throw new ArgumentNullException(nameof(context));

            IsAuthenticated = _user.Identity?.IsAuthenticated ?? false;

            if (IsAuthenticated)
            {
                UserId = GetClaimValue(ClaimTypes.NameIdentifier);
                Email = GetClaimValue(ClaimTypes.Email);
                Username = GetClaimValue("username");
                IsAdmin = _user.IsInRole(Roles.Admin);
            }
        }

        private string GetClaimValue(string claimType)
        {
            return _user.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
        }
    }
}
