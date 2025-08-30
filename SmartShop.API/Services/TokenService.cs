using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartShop.API.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string username)
        {
            try
            {
                var keyString = _configuration["Jwt:Key"];
                if (string.IsNullOrWhiteSpace(keyString))
                    throw new ArgumentException("JWT key is missing from configuration.");

                var keyBytes = Encoding.UTF8.GetBytes(keyString);
                if (keyBytes.Length < 32)
                    throw new ArgumentOutOfRangeException("Jwt:Key", "JWT key must be at least 256 bits (32 bytes) long.");

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var key = new SymmetricSecurityKey(keyBytes);
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException("JWT generation failed due to insufficient key length.", ex);
            }
            catch (ArgumentException ex)
            {
                // Log and rethrow or return a meaningful error
                throw new InvalidOperationException("JWT generation failed due to missing configuration.", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An unexpected error occurred during JWT generation.", ex);
            }
        }
    }


}
