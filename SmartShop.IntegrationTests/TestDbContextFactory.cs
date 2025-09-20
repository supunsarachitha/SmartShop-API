using Microsoft.EntityFrameworkCore;

// This factory provides methods to create in-memory DbContext instances for integration testing.
namespace SmartShop.IntegrationTests
{
    public static class TestDbContextFactory
    {
        public static SmartShopDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<SmartShopDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new SmartShopDbContext(options);
        }
    }
}


 