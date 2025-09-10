using System.ComponentModel.DataAnnotations;

namespace SmartShop.API.Models
{
    public class Setting
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public DataType DataType { get; set; } = DataType.Text;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}