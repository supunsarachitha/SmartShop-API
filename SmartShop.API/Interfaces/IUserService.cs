using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Interfaces
{
    public interface IUserService
    {
        UserAuthenticationResponse Authenticate(string userName, string password);

        Task<ApplicationResponse<List<User>>> GetAllUsersAsync();
        Task<ApplicationResponse<User>> GetUserByIdAsync(Guid id);
        Task<ApplicationResponse<User>> CreateUserAsync(User user);
        Task<ApplicationResponse<User>> UpdateUserAsync(Guid id, User user);
        Task<ApplicationResponse<User>> DeleteUserAsync(Guid id); 
    }
}
