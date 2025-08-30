using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartShop.API.Models
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Invoice")]
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }

        [ForeignKey("PaymentMethod")]
        public Guid PaymentMethodId { get; set; }
        public int Status { get; set; }
        public DateTime Date { get; set; }

        public required PaymentMethod PaymentMethod { get; set; }
    }
}