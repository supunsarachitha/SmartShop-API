using Microsoft.CodeAnalysis.Scripting;
using SmartShop.API.Interfaces;

namespace SmartShop.API.Services
{
    public class UserService : IUserService
    {
        private readonly SmartShopDbContext _context;

        public UserService(SmartShopDbContext context)
        {
            _context = context;
        }

        public bool Authenticate(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                return false;

            var user = _context.Users
                .FirstOrDefault(u => u.UserName == userName && u.IsActive);

            return user != null;
        }

    }
}
