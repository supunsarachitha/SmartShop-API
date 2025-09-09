using Microsoft.EntityFrameworkCore;
using SmartShop.API.Common;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Services
{

    public class UserService : IUserService
    {
        private readonly SmartShopDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UserService(SmartShopDbContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public UserAuthenticationResponse Authenticate(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                return new UserAuthenticationResponse { IsAuthenticated = false };

            var user = _context.Users.FirstOrDefault(u => u.UserName == userName && u.IsActive);

            if (user != null)
            {
                // Update last login timestamp
                user.LastLoginDate = DateTime.UtcNow;
                _context.SaveChanges();

                return new UserAuthenticationResponse { IsAuthenticated = true, User = user };
            }
            else
            {
                return new UserAuthenticationResponse { IsAuthenticated = false };
            }
        }

        public async Task<ApplicationResponse<User>> CreateUserAsync(User user)
        {
            try
            {
                if (user == null)
                {
                    return ResponseFactory.CreateErrorResponse<User>(
                        "User object is null.",
                        "User",
                        "User object is null.",
                        StatusCodes.Status400BadRequest);
                }

                user.Id = Guid.NewGuid();
                user.CreatedDate = _dateTimeProvider.UtcNow;
                user.UpdatedDate = _dateTimeProvider.UtcNow;
                user.IsActive = true;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return ResponseFactory.CreateSuccessResponse(
                    user,
                    "User created successfully.",
                    StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<User>(
                    "User creation failed.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<User>> DeleteUserAsync(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return ResponseFactory.CreateErrorResponse<User>(
                        "User not found.",
                        "Id",
                        "No user found with the specified ID.",
                        StatusCodes.Status404NotFound);
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return ResponseFactory.CreateSuccessResponse(
                    user,
                    "User deleted successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<User>(
                    "User deletion failed.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<List<User>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return ResponseFactory.CreateSuccessResponse(
                    users,
                    "Users retrieved successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<List<User>>(
                    "Failed to retrieve users.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<User>> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return ResponseFactory.CreateErrorResponse<User>(
                        "User not found.",
                        "Id",
                        "No user found with the specified ID.",
                        StatusCodes.Status404NotFound);
                }

                return ResponseFactory.CreateSuccessResponse(
                    user,
                    "User retrieved successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<User>(
                    "Failed to retrieve user.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<User>> UpdateUserAsync(Guid id, User updatedUser)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return ResponseFactory.CreateErrorResponse<User>(
                        "User not found.",
                        "Id",
                        "No user found with the specified ID.",
                        StatusCodes.Status404NotFound);
                }

                user.UserName = updatedUser.UserName;
                user.Email = updatedUser.Email;
                user.Password = updatedUser.Password;
                user.Name = updatedUser.Name;
                user.Role = updatedUser.Role;
                user.IsActive = updatedUser.IsActive;
                user.UpdatedDate = _dateTimeProvider.UtcNow;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return ResponseFactory.CreateSuccessResponse(
                    user,
                    "User updated successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<User>(
                    "User update failed.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<bool>> UserExistsAsync(Guid id)
        {
            try
            {
                var exists = await _context.Users.AnyAsync(u => u.Id == id);
                return ResponseFactory.CreateSuccessResponse(
                    exists,
                    exists ? "User exists." : "User does not exist.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<bool>(
                    "Failed to check user existence.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
