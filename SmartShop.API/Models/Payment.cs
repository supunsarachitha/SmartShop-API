using System;

namespace SmartShop.API.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public int InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}