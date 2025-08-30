using System.ComponentModel.DataAnnotations;

namespace SmartShop.API.Models
{ 
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}