using System.Collections.Generic;

namespace SmartShop.API.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public List<InvoiceItem> Items { get; set; } = new();
        public decimal Total { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}