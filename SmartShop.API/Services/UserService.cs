using Microsoft.CodeAnalysis.Scripting;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Services
{
    public class UserService : IUserService
    {
        private readonly SmartShopDbContext _context;

        public UserService(SmartShopDbContext context)
        {
            _context = context;
        }

        public UserAuthenticationResponse Authenticate(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                return new UserAuthenticationResponse { IsAuthenticated = false};

            var user = _context.Users.FirstOrDefault(u => u.UserName == userName && u.IsActive);

            if(user != null)
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
          
    }
}
