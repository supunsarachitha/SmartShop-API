using System.ComponentModel.DataAnnotations;

namespace SmartShop.API.Models
{
    public class SequenceConfig
    {
        [Key]
        public Guid Id { get; set; }
        public string Key { get; set; } = null!; 
        public string Description { get; set; } = null!; 
        public string Prefix { get; set; } = null!;
        public int Length { get; set; }
        public int Value { get; set; }
    }
}
