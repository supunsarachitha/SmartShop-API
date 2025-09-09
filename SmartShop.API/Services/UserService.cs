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
                // Use VerifyPassword to check the password against the stored hash
                if (!VerifyPassword(password, user.Password))
                {
                    return new UserAuthenticationResponse { IsAuthenticated = false };
                }

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

                // Validate and selectively update UserName
                if (!string.IsNullOrWhiteSpace(updatedUser.UserName))
                {
                    if (updatedUser.UserName.Length > 100)
                    {
                        return ResponseFactory.CreateErrorResponse<User>(
                            "UserName exceeds maximum length.",
                            "UserName",
                            "UserName must be at most 100 characters.",
                            StatusCodes.Status400BadRequest);
                    }
                    user.UserName = updatedUser.UserName;
                }

                // Validate and selectively update Email
                if (!string.IsNullOrWhiteSpace(updatedUser.Email))
                {
                    if (updatedUser.Email.Length > 256)
                    {
                        return ResponseFactory.CreateErrorResponse<User>(
                            "Email exceeds maximum length.",
                            "Email",
                            "Email must be at most 256 characters.",
                            StatusCodes.Status400BadRequest);
                    }
                    user.Email = updatedUser.Email;
                }

                // Validate password (example: minimum length 8, contains number, etc.)
                if (!string.IsNullOrWhiteSpace(updatedUser.Password))
                {
                    if (updatedUser.Password.Length < 8 ||
                        !updatedUser.Password.Any(char.IsDigit) ||
                        !updatedUser.Password.Any(char.IsLetter))
                    {
                        return ResponseFactory.CreateErrorResponse<User>(
                            "Password does not meet complexity requirements.",
                            "Password",
                            "Password must be at least 8 characters long and contain both letters and numbers.",
                            StatusCodes.Status400BadRequest);
                    }

                    // Hash password before storing
                    user.Password = HashPassword(updatedUser.Password);
                }

                // Selectively update other fields
                if (!string.IsNullOrWhiteSpace(updatedUser.Name))
                    user.Name = updatedUser.Name;

                if (updatedUser.Role != null)
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
         
        private static string HashPassword(string password)
        {
            // BCrypt automatically generates a salt and hashes the password securely
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            // Verifies the password against the hashed password
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        } 
    }
}
