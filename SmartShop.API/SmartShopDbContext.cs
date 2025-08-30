using Microsoft.EntityFrameworkCore;
using SmartShop.API.Models;

public class SmartShopDbContext : DbContext
{
    public SmartShopDbContext(DbContextOptions<SmartShopDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceItem> InvoiceItems { get; set; }
    public DbSet<PaymentMethod> PaymentMethod { get; set; }

    public DbSet<User> Users { get; set; } = default!;
}