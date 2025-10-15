using Microsoft.AspNetCore.Mvc;
using SmartShop.API.Interfaces;
using SmartShop.API.Models.Requests;
using SmartShop.API.Models.Responses;

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
            if (request == null)
            {
                return BadRequest(new ApplicationResponse<object>
                {
                    Success = false,
                    Message = "Invalid request",
                    Data = null,
                    Errors = new List<ErrorDetail>
                    {
                        new ErrorDetail { Field = "Request", Message = "Request body is required" }
                    },
                    StatusCode = 400
                }); 
            }

            UserAuthenticationResponse auth = _userService.Authenticate(request.UserName, request.Password);

            if (auth != null && auth.IsAuthenticated && auth.User != null)
            {
                auth.Token = _tokenService.GenerateJwtToken(auth.User.UserName); 

                var response = new ApplicationResponse<object>
                {
                    Success = true,
                    Message = "Login Successful",
                    Data = auth,
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
