using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing user data in the system.
    /// </summary>
    /// <remarks>
    /// This controller handles CRUD operations for user entities, including retrieving, creating,
    /// updating, and deleting users. All endpoints in this controller require authorization.
    /// The base route for this controller is "api/Users".
    /// </remarks>
    [Authorize]
    [Route("api/[controller]")] 
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetAllUsersAsync();
            return StatusCode(response.StatusCode ?? StatusCodes.Status200OK, response);
        }

        // GET: api/Users/guid
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var response = await _userService.GetUserByIdAsync(id);
            return StatusCode(response.StatusCode ?? StatusCodes.Status200OK, response);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            var response = await _userService.UpdateUserAsync(id, user);
            return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUser(User user)
        {
            var response = await _userService.CreateUserAsync(user);

            if (!response.Success)
                return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);

            if (response.Data == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApplicationResponse<User>
                {
                    Success = false,
                    Message = "No user data was returned.",
                    Data = default!, // Use default! to satisfy non-nullable reference type
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
                
            return CreatedAtAction(nameof(GetUser), new { id = response.Data.Id }, response);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var response = await _userService.DeleteUserAsync(id);
            return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);
        }
    }
}
