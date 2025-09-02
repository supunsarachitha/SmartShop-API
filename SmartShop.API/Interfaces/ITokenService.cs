namespace SmartShop.API.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Generates a JWT token for the specified username.
        /// </summary>
        /// <param name="username">The username to include in the token claims.</param>
        /// <returns>A signed JWT token string.</returns>
        string GenerateJwtToken(string username);
    }
}
