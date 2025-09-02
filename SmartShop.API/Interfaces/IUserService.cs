using SmartShop.API.Models;

namespace SmartShop.API.Interfaces
{
    public interface IUserService
    { 
        bool Authenticate(string userName, string password);
    }
}
