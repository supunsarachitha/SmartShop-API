using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Requests;
using SmartShop.API.Models.Responses;
using SmartShop.API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        public AuthController(ITokenService tokenService, ILogger<AuthController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest user)
        {
            if (user.UserName == "admin" && user.Password == "password")
            {
                var token = _tokenService.GenerateJwtToken(user.UserName);

                var response = new ApplicationResponse<object>
                {
                    Success = true,
                    Message = "Login successful",
                    Data = new { token },
                    Errors = null,
                    StatusCode = 200
                };

                return Ok(response);
            }

            var errorResponse = new ApplicationResponse<object>
            {
                Success = false,
                Message = "Invalid username or password",
                Data = null,
                Errors = new List<ErrorDetail>
                {
                    new ErrorDetail { Field = "UserName", Message = "Invalid credentials" }
                },
                StatusCode = 401
            };

            return Unauthorized(errorResponse);
        }
    }
}
