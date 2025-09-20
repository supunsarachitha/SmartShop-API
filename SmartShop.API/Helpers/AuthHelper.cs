namespace SmartShop.API.Helpers
{
    public static class AuthHelper
    {
        public static string HashPassword(string password)
        {
            // BCrypt automatically generates a salt and hashes the password securely
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

    }
}
