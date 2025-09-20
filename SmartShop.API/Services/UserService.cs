using Microsoft.EntityFrameworkCore;
using SmartShop.API.Helpers;
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

                // Update last login timestamp using IDateTimeProvider
                user.LastLoginDate = _dateTimeProvider.UtcNow;
                _context.SaveChanges();

                return new UserAuthenticationResponse { IsAuthenticated = true, User = user };
            }
            else
            {
                return new UserAuthenticationResponse { IsAuthenticated = false };
            }
        }
         
        public async Task<ApplicationResponse<UserDto>> CreateUserAsync(User user)
        {
            try
            {
                if (user == null)
                {
                    return ResponseFactory.CreateErrorResponse<UserDto>(
                        "Failed to create user because the provided user object is null.",
                        "User",
                        "The input user object was null. Please provide a valid user.",
                        StatusCodes.Status400BadRequest);
                }

                // Check if a user with the same UserName or Email already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == user.UserName || u.Email == user.Email);

                if (existingUser != null)
                {
                    return ResponseFactory.CreateErrorResponse<UserDto>(
                        "Failed to create user because a user with the same UserName or Email already exists.",
                        "User",
                        "A user with the same UserName or Email already exists. Please use unique values.",
                        StatusCodes.Status400BadRequest);
                }

                user.CreatedDate = _dateTimeProvider.UtcNow;
                user.UpdatedDate = _dateTimeProvider.UtcNow;
                user.IsActive = true;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return ResponseFactory.CreateSuccessResponse(
                    MapUserToDto(user),
                    "User created successfully.",
                    StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<UserDto>(
                    "An unexpected error occurred while creating the user.",
                    "Exception",
                    $"Exception message: {ex.Message}",
                    StatusCodes.Status500InternalServerError);
            }
        }
         
        public async Task<ApplicationResponse<UserDto>> DeleteUserAsync(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return ResponseFactory.CreateErrorResponse<UserDto>(
                        "Unable to delete user because the user was not found.",
                        "Id",
                        $"No user exists with the specified ID: {id}.",
                        StatusCodes.Status404NotFound);
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                 
                return ResponseFactory.CreateSuccessResponse(
                    MapUserToDto(user),
                    "User deleted successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<UserDto>(
                    "An error occurred while deleting the user.",
                    "Exception",
                    $"Exception message: {ex.Message}",
                    StatusCodes.Status500InternalServerError);
            }
        }

        
        public async Task<ApplicationResponse<List<UserDto>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();

                var userDtos = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Name = u.Name,
                    RoleID = u.RoleId,
                    IsActive = u.IsActive,
                    LastLoginDate = u.LastLoginDate,
                    CreatedDate = u.CreatedDate,
                    UpdatedDate = u.UpdatedDate
                }).ToList();

                return ResponseFactory.CreateSuccessResponse(
                    userDtos,
                    "Users retrieved successfully.",
                    StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<List<UserDto>>(
                    "An error occurred while retrieving the list of users.",
                    "Exception",
                    $"Exception message: {ex.Message}",
                    StatusCodes.Status500InternalServerError);
            }
        }

        // Example change for GetUserByIdAsync method
        public async Task<ApplicationResponse<UserDto>> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return ResponseFactory.CreateErrorResponse<UserDto>(
                        "User retrieval failed because the user was not found.",
                        "Id",
                        $"No user exists with the specified ID: {id}.",
                        StatusCodes.Status404NotFound);
                }

                return ResponseFactory.CreateSuccessResponse(
                    MapUserToDto(user),
                    "User retrieved successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<UserDto>(
                    "An error occurred while retrieving the user.",
                    "Exception",
                    $"Exception message: {ex.Message}",
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<UserDto>> UpdateUserAsync(Guid id, User updatedUser)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return ResponseFactory.CreateErrorResponse<UserDto>(
                        "User update failed because the user was not found.",
                        "Id",
                        $"No user exists with the specified ID: {id}.",
                        StatusCodes.Status404NotFound);
                }

                // Validate and selectively update UserName
                if (!string.IsNullOrWhiteSpace(updatedUser.UserName))
                {
                    if (updatedUser.UserName.Length > 100)
                    {
                        return ResponseFactory.CreateErrorResponse<UserDto>(
                            "UserName update failed due to exceeding maximum length.",
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
                        return ResponseFactory.CreateErrorResponse<UserDto>(
                            "Email update failed due to exceeding maximum length.",
                            "Email",
                            "Email must be at most 256 characters.",
                            StatusCodes.Status400BadRequest);
                    }
                    user.Email = updatedUser.Email;
                }

                if (!string.IsNullOrWhiteSpace(updatedUser.Password))
                {
                    if (updatedUser.Password.Length < 8 ||
                        !updatedUser.Password.Any(char.IsDigit) ||
                        !updatedUser.Password.Any(char.IsLetter))
                    {
                        return ResponseFactory.CreateErrorResponse<UserDto>(
                            "Password update failed due to not meeting complexity requirements.",
                            "Password",
                            "Password must be at least 8 characters long and contain both letters and numbers.",
                            StatusCodes.Status400BadRequest);
                    }

                    // Hash password before storing
                    user.Password = AuthHelper.HashPassword(updatedUser.Password);
                }

                // Selectively update other fields
                if (!string.IsNullOrWhiteSpace(updatedUser.Name))
                    user.Name = updatedUser.Name;

                if (updatedUser.RoleId != null && updatedUser.RoleId.HasValue)
                    user.RoleId = updatedUser.RoleId;

                user.IsActive = updatedUser.IsActive;
                user.UpdatedDate = _dateTimeProvider.UtcNow;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                return ResponseFactory.CreateSuccessResponse(
                    MapUserToDto(user),
                    "User updated successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<UserDto>(
                    "An error occurred while updating the user.",
                    "Exception",
                    $"Exception message: {ex.Message}",
                    StatusCodes.Status500InternalServerError);
            }
        }
         

        private static bool VerifyPassword(string password, string hashedPassword)
        {
            // Verifies the password against the hashed password
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private static UserDto MapUserToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                RoleID = user.RoleId,
                IsActive = user.IsActive,
                LastLoginDate = user.LastLoginDate,
                CreatedDate = user.CreatedDate,
                UpdatedDate = user.UpdatedDate
            };
        }
    }
}
