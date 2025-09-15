using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Interfaces
{
    public interface IUserService
    {
        UserAuthenticationResponse Authenticate(string userName, string password);

        Task<ApplicationResponse<List<UserDto>>> GetAllUsersAsync();
        Task<ApplicationResponse<UserDto>> GetUserByIdAsync(Guid id);
        Task<ApplicationResponse<UserDto>> CreateUserAsync(User user);
        Task<ApplicationResponse<UserDto>> UpdateUserAsync(Guid id, User user);
        Task<ApplicationResponse<UserDto>> DeleteUserAsync(Guid id); 
    }
}
