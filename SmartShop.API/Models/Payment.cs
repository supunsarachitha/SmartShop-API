using System;
using System.ComponentModel.DataAnnotations;

namespace SmartShop.API.Models
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public Guid PaymentMethodId { get; set; }
        public int Status { get; set; }
        public DateTime Date { get; set; }
    }
}