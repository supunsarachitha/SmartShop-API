using Microsoft.EntityFrameworkCore;

public class SmartShopDbContext : DbContext
{
    public SmartShopDbContext(DbContextOptions<SmartShopDbContext> options)
        : base(options)
    {
    }

    // Define your DbSets here, e.g.:
    // public DbSet<Product> Products { get; set; }
}