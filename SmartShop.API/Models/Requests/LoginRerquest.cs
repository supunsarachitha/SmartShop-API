using System.ComponentModel.DataAnnotations;

namespace SmartShop.API.Models.Requests
{
    public class LoginRequest
    {
        [Required] 
        public required string UserName { get; set; }

        [Required] 
        public required string Password { get; set; }
    }

}
