using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartShop.API.Models;
using SmartShop.API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartShop.API.Controllers
{
 
    public class AuthController : Controller
    {
        private readonly TokenService _tokenService;

        public AuthController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (user.UserName == "admin" && user.Password == "password")
            {
                var token = _tokenService.GenerateJwtToken(user.UserName);

                return Ok(new { token });
            }
            return Unauthorized();
        } 
    }
}
