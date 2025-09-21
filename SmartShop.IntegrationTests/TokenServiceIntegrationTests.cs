using Microsoft.Extensions.Configuration;
using SmartShop.API.Models;
using SmartShop.API.Services;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SmartShop.IntegrationTests
{
    public class TokenServiceIntegrationTests
    {
        private static IConfiguration? _configuration;

        private static IConfiguration GetConfiguration()
        {
            if (_configuration == null)
            {
                _configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(new[]
                    {
                        new KeyValuePair<string, string?>("Jwt:Key", "your-256-bit-secret-key-should-be-at-least-32-characters-long"),
                        new KeyValuePair<string, string?>("Jwt:Issuer", "your-test-issuer"),
                        new KeyValuePair<string, string?>("Jwt:Audience", "your-test-audience")
                    })
                    .Build();
            }
            return _configuration;
        }

        private TokenService CreateService()
        {
            return new TokenService(GetConfiguration());
        }

        [Fact]
        public async Task GenerateJwtToken_ShouldReturnToken_ForValidUser()
        {
            var service = CreateService();
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "tokenuser",
                Email = "tokenuser@example.com",
                Password = "securepassword"
            };

            var token = await Task.FromResult(service.GenerateJwtToken(user.UserName));

            Assert.False(string.IsNullOrEmpty(token));
        }

        [Fact]
        public async Task ValidateToken_ShouldReturnTrue_ForValidToken()
        {
            var service = CreateService();
            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = "validuser",
                Email = "validuser@example.com",
                Password = "validpassword"
            };

            var token = await Task.FromResult(service.GenerateJwtToken(user.UserName));
            var isValid = await ValidateTokenAsync(service, token);

            Assert.True(isValid);
        }

        [Fact]
        public async Task ValidateToken_ShouldReturnFalse_ForInvalidToken()
        {
            var service = CreateService();
            var invalidToken = "this.is.not.a.valid.token";

            var isValid = await ValidateTokenAsync(service, invalidToken);

            Assert.False(isValid);
        }

        [Fact]
        public void GenerateJwtToken_ShouldThrowException_ForNullUser()
        {
            var service = CreateService();
            string? username = null;

            Assert.Throws<ArgumentNullException>(() => service.GenerateJwtToken(username!));
        }

        // Helper method to simulate ValidateTokenAsync
        private async Task<bool> ValidateTokenAsync(TokenService service, string token)
        {
            try
            {
                // Get key from appsettings.json via configuration
                var configuration = GetConfiguration();
                var keyString = configuration["Jwt:Key"];
                if (string.IsNullOrEmpty(keyString))
                    throw new InvalidOperationException("JWT key not found in appsettings.json.");

                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var key = System.Text.Encoding.ASCII.GetBytes(keyString);
                tokenHandler.ValidateToken(token, new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }
    }
}
