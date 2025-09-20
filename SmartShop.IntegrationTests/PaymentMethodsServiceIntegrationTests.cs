using SmartShop.API.Helpers;
using SmartShop.API.Models;
using SmartShop.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShop.IntegrationTests
{
    public class PaymentMethodsServiceIntegrationTests
    {
        private SmartShopDbContext CreateContext() => TestDbContextFactory.CreateContext();

        DateTimeProvider GetDateTimeProvider()
        {
            return new DateTimeProvider();
        }

        private PaymentMethodsService GetService(SmartShopDbContext context)
        {
            return new PaymentMethodsService(context, GetDateTimeProvider());
        }

        [Fact]
        public async Task CreatePaymentMethod_ShouldAddPaymentMethod()
        {
            using var context = CreateContext();
            var service = GetService(context);

            var paymentMethod = new PaymentMethod
            {
                Id = Guid.NewGuid(),
                Name = "Credit Card",
                Description = "Visa/Mastercard",
                Type = 1,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };

            var response = await service.CreatePaymentMethodAsync(paymentMethod);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal("Credit Card", response.Data.Name);
        }

        [Fact]
        public async Task GetPaymentMethodById_ShouldReturnPaymentMethod()
        {
            using var context = CreateContext();
            var service = GetService(context);

            var paymentMethod = new PaymentMethod
            {
                Id = Guid.NewGuid(),
                Name = "PayPal",
                Description = "Online Payment",
                Type = 2,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };

            context.PaymentMethods.Add(paymentMethod);
            await context.SaveChangesAsync();

            var response = await service.GetPaymentMethodByIdAsync(paymentMethod.Id);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal("PayPal", response.Data.Name);
        }

        [Fact]
        public async Task UpdatePaymentMethod_ShouldUpdatePaymentMethod()
        {
            using var context = CreateContext();
            var service = GetService(context);

            var paymentMethod = new PaymentMethod
            {
                Id = Guid.NewGuid(),
                Name = "Bank Transfer",
                Description = "Wire Transfer",
                Type = 3,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };

            context.PaymentMethods.Add(paymentMethod);
            await context.SaveChangesAsync();

            paymentMethod.Name = "Updated Bank Transfer";
            var response = await service.UpdatePaymentMethodAsync(paymentMethod.Id, paymentMethod);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal("Updated Bank Transfer", response.Data.Name);
        }

        [Fact]
        public async Task DeletePaymentMethod_ShouldRemovePaymentMethod()
        {
            using var context = CreateContext();
            var service = GetService(context);

            var paymentMethod = new PaymentMethod
            {
                Id = Guid.NewGuid(),
                Name = "Cash",
                Description = "Physical Money",
                Type = 4,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };

            context.PaymentMethods.Add(paymentMethod);
            await context.SaveChangesAsync();

            var response = await service.DeletePaymentMethodAsync(paymentMethod.Id);

            Assert.True(response.Success);

            var deleted = context.PaymentMethods.Find(paymentMethod.Id);
            Assert.Null(deleted);
        }

        [Fact]
        public async Task GetAllPaymentMethods_ShouldReturnAllPaymentMethods()
        {
            using var context = CreateContext();
            var service = GetService(context);

            context.PaymentMethods.Add(new PaymentMethod
            {
                Id = Guid.NewGuid(),
                Name = "Method1",
                Description = "Desc1",
                Type = 1,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            });
            context.PaymentMethods.Add(new PaymentMethod
            {
                Id = Guid.NewGuid(),
                Name = "Method2",
                Description = "Desc2",
                Type = 2,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            });
            await context.SaveChangesAsync();

            var response = await service.GetAllPaymentMethodsAsync();

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.True(response.Data.Count >= 2);
        }

        [Fact]
        public async Task GetPaymentMethodById_ShouldReturnError_WhenNotFound()
        {
            using var context = CreateContext();
            var service = GetService(context);

            var nonExistentId = Guid.NewGuid();
            var response = await service.GetPaymentMethodByIdAsync(nonExistentId);

            Assert.False(response.Success);
            Assert.Null(response.Data);
            Assert.NotNull(response.Message);
        }

        [Fact]
        public async Task UpdatePaymentMethod_ShouldReturnError_WhenNotFound()
        {
            using var context = CreateContext();
            var service = GetService(context);

            var nonExistentId = Guid.NewGuid();
            var paymentMethod = new PaymentMethod
            {
                Id = nonExistentId,
                Name = "NonExistent",
                Description = "Should not exist",
                Type = 99,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };

            var response = await service.UpdatePaymentMethodAsync(nonExistentId, paymentMethod);

            Assert.False(response.Success);
            Assert.Null(response.Data);
            Assert.NotNull(response.Message);
        }

        [Fact]
        public async Task DeletePaymentMethod_ShouldReturnError_WhenNotFound()
        {
            using var context = CreateContext();
            var service = GetService(context);

            var nonExistentId = Guid.NewGuid();
            var response = await service.DeletePaymentMethodAsync(nonExistentId);

            Assert.False(response.Success);
            Assert.Null(response.Data);
            Assert.NotNull(response.Message);
        }

        [Fact]
        public async Task CreatePaymentMethod_ShouldNotAllowDuplicateId()
        {
            using var context = CreateContext();
            var service = GetService(context);

            var id = Guid.NewGuid();
            var paymentMethod1 = new PaymentMethod
            {
                Id = id,
                Name = "UniqueMethod",
                Description = "First",
                Type = 1,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };
            var paymentMethod2 = new PaymentMethod
            {
                Id = id,
                Name = "DuplicateMethod",
                Description = "Second",
                Type = 2,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };

            var response1 = await service.CreatePaymentMethodAsync(paymentMethod1);
            var response2 = await service.CreatePaymentMethodAsync(paymentMethod2);

            Assert.True(response1.Success);
            Assert.False(response2.Success);
            Assert.Null(response2.Data);
        }

        [Fact]
        public async Task CreatePaymentMethod_ShouldReturnError_WhenNameIsNull()
        {
            using var context = CreateContext();
            var service = GetService(context);

            var paymentMethod = new PaymentMethod
            {
                Id = Guid.NewGuid(),
                Name = null,
                Description = "No name",
                Type = 1,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };

            var response = await service.CreatePaymentMethodAsync(paymentMethod);

            Assert.False(response.Success);
            Assert.Null(response.Data);
            Assert.NotNull(response.Message);
        }

        [Fact]
        public async Task CreatePaymentMethodAsync_ShouldCreatePaymentMethod()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var paymentMethod = new PaymentMethod
            {
                Id = Guid.NewGuid(),
                Name = "Mobile Wallet",
                Description = "Apple Pay/Google Pay",
                Type = 5,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };

            var response = await service.CreatePaymentMethodAsync(paymentMethod);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(paymentMethod.Name, response.Data.Name);
        }

        [Fact]
        public async Task GetAllPaymentMethodsAsync_ShouldReturnPaymentMethods()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var paymentMethod = new PaymentMethod
            {
                Id = Guid.NewGuid(),
                Name = "Crypto",
                Description = "Bitcoin/Ethereum",
                Type = 6,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };
            context.PaymentMethods.Add(paymentMethod);
            await context.SaveChangesAsync();

            var response = await service.GetAllPaymentMethodsAsync();

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Contains(response.Data, pm => pm.Id == paymentMethod.Id);
        }

        [Fact]
        public async Task GetPaymentMethodByIdAsync_ShouldReturnPaymentMethod()
        {
            using var context = CreateContext();
            var service = GetService(context);
            var paymentMethod = new PaymentMethod
            {
                Id = Guid.NewGuid(),
                Name = "Voucher",
                Description = "Gift Voucher",
                Type = 7,
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };
            context.PaymentMethods.Add(paymentMethod);
            await context.SaveChangesAsync();

            var response = await service.GetPaymentMethodByIdAsync(paymentMethod.Id);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(paymentMethod.Id, response.Data.Id);
        }

        
    }
}
