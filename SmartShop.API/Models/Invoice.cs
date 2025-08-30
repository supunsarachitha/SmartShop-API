using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SmartShop.API.Models
{
    public class Invoice
    {
        [Key]
        public Guid Id { get; set; } 
        public Guid CustomerId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty; 
        public decimal Total { get; set; }
        public int Status { get; set; } 
        public DateTime InvoiceDate { get; set; }
        public List<InvoiceItem> Items { get; set; } = new();
        public List<Payment> Payments { get; set; } = new();
    }
}