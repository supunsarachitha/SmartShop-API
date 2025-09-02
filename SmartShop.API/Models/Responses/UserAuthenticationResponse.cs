namespace SmartShop.API.Models.Responses
{
    public class UserAuthenticationResponse
    {
        public bool IsAuthenticated { get; set; }
        public User? User { get; set; }

    }
}
