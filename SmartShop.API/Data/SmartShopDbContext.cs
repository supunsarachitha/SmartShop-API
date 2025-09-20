using Microsoft.EntityFrameworkCore;
using SmartShop.API.Models;

namespace SmartShop.API.Data
{
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
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<UserRole> UserRoles { get; set; } = default!;
        public DbSet<SequenceConfig> SequenceConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Use fixed DateTime for seeding to avoid changing values on each migration
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var systemAdminRoleId = new Guid("0b4cf409-41a3-448c-9dfd-393906be62a1");
            var storeAdminRoleId = new Guid("e2a1f7b2-7c4a-4b7e-8e2d-1a2b3c4d5e6f");

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole
                {
                    Id = systemAdminRoleId,
                    Name = "SysAdmin",
                    Description = "System administrator with full access",
                    CreatedDate = seedDate,
                    UpdatedDate = seedDate
                },
                new UserRole
                {
                    Id = storeAdminRoleId,
                    Name = "StoreAdmin",
                    Description = "Store administrator with store-level access",
                    CreatedDate = seedDate,
                    UpdatedDate = seedDate
                }
            );

            var adminUserId = new Guid("a1b2c3d4-e5f6-4789-abcd-1234567890ab");

            // Replace with your actual hash
            //admin@123
            var staticHashedPassword = "$2a$12$cfShxSPzeorednTNM/ro2eT75uJJRB4vbQAcFA6d/RsiItoFtlr8W";

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = adminUserId,
                UserName = "admin",
                Email = "admin@smartshop.local",
                Password = staticHashedPassword,
                Name = "System Administrator",
                RoleId = systemAdminRoleId,
                IsActive = true,
                CreatedDate = seedDate,
                UpdatedDate = seedDate
            });
        }
    }
}