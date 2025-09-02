using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Interfaces
{
    public interface IUserService
    {
        UserAuthenticationResponse Authenticate(string userName, string password); 
    }
}
