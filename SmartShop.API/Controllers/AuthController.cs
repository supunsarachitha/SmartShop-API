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
        private readonly IUserService _userService;
        public AuthController(ITokenService tokenService, ILogger<AuthController> logger, IUserService userService)
        {
            _tokenService = tokenService;
            _logger = logger;
            _userService = userService;
        }
         
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            bool isAuthenticated = _userService.Authenticate(request.UserName, request.Password);

            if (isAuthenticated)
            {
                var token = _tokenService.GenerateJwtToken(request.UserName);

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
