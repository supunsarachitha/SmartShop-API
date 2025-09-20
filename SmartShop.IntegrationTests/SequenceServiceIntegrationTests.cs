using Microsoft.EntityFrameworkCore;
using SmartShop.API.Models;
using SmartShop.API.Services;

namespace SmartShop.IntegrationTests
{
    public class SequenceServiceTests
    {
        private DbContext CreateContext() => TestDbContextFactory.CreateContext();

        [Fact]
        public async Task GetNextSequenceAsync_InitializesSequence_WhenNotFound()
        {
            // Arrange
            var context = CreateContext();
            var service = new SequenceService(context);

            // Act
            var result = await service.GetNextSequenceAsync("Invoice", true);

            // Assert
            Assert.Equal("000001", result);
            var config = await context.Set<SequenceConfig>().FirstOrDefaultAsync(c => c.Key == "Invoice");
            Assert.NotNull(config);
            Assert.Equal(1, config.Value);
        }

        [Fact]
        public async Task GetNextSequenceAsync_IncrementsSequence_WhenIncrementTrue()
        {
            // Arrange
            var context = CreateContext();
            var config = new SequenceConfig
            {
                Id = Guid.NewGuid(),
                Key = "Invoice",
                Description = "Sequence for Invoice",
                Prefix = "",
                Length = 6,
                Value = 5
            };
            context.Add(config);
            await context.SaveChangesAsync();
            var service = new SequenceService(context);

            // Act
            var result = await service.GetNextSequenceAsync("Invoice", true);

            // Assert
            Assert.Equal("000006", result);
            var updated = await context.Set<SequenceConfig>().FirstOrDefaultAsync(c => c.Key == "Invoice");
            Assert.Equal(6, updated?.Value);
        }

        [Fact]
        public async Task GetNextSequenceAsync_DoesNotIncrement_WhenIncrementFalse()
        {
            // Arrange
            var context = CreateContext();
            var config = new SequenceConfig
            {
                Id = Guid.NewGuid(),
                Key = "Customer",
                Description = "Sequence for Customer",
                Prefix = "",
                Length = 6,
                Value = 10
            };
            context.Add(config);
            await context.SaveChangesAsync();
            var service = new SequenceService(context);

            // Act
            var result = await service.GetNextSequenceAsync("Customer", false);

            // Assert
            Assert.Equal("000010", result);
            var updated = await context.Set<SequenceConfig>().FirstOrDefaultAsync(c => c.Key == "Customer");
            Assert.Equal(10, updated?.Value);
        }

        [Fact]
        public async Task GetNextSequenceAsync_FormatsWithPrefix()
        {
            // Arrange
            var context = CreateContext();
            var config = new SequenceConfig
            {
                Id = Guid.NewGuid(),
                Key = "Product",
                Description = "Sequence for Product",
                Prefix = "PRD",
                Length = 4,
                Value = 99
            };
            context.Add(config);
            await context.SaveChangesAsync();
            var service = new SequenceService(context);

            // Act
            var result = await service.GetNextSequenceAsync("Product", true);

            // Assert
            Assert.Equal("PRD-0100", result);
        }
    }
}
