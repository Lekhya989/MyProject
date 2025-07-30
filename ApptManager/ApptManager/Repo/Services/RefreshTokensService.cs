using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;

namespace ApptManager.Services
{
    public class RefreshTokenService
    {
        private readonly IConfiguration _config;

        public RefreshTokenService(IConfiguration config)
        {
            _config = config;
        }

        // Generate a new refresh token
        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        public string? GetRefreshTokenFromRequest(HttpRequest request)
        {
            return request.Cookies.TryGetValue("refreshToken", out var token) ? token : null;
        }
    }
}
