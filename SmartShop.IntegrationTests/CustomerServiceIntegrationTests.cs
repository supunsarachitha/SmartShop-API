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
    public class CustomerServiceIntegrationTests
    {
        private SmartShopDbContext CreateContext() => TestDbContextFactory.CreateContext();

        DateTimeProvider GetDateTimeProvider()
        {
            return new DateTimeProvider();
        }

        private CustomerService GetService(SmartShopDbContext context)
        {
            return new CustomerService(context, GetDateTimeProvider());
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldCreateCustomer()
        {
            // Arrange
            using var context = CreateContext();
            var service = GetService(context);
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "John Doe",
                Email = "john@example.com",
                Phone = "1234567890",
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };

            // Act
            var response = await service.CreateCustomerAsync(customer);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(customer.Name, response.Data.Name);
        }

        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnCustomers()
        {
            // Arrange
            using var context = CreateContext();
            var service = GetService(context);
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Jane Doe",
                Email = "jane@example.com",
                Phone = "0987654321",
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            // Act
            var response = await service.GetAllCustomersAsync();

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Contains(response.Data, c => c.Id == customer.Id);
        }

        [Fact]
        public async Task GetCustomerByIdAsync_ShouldReturnCustomer()
        {
            // Arrange
            using var context = CreateContext();
            var service = GetService(context);
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Alice Smith",
                Email = "alice@example.com",
                Phone = "5555555555",
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            // Act
            var response = await service.GetCustomerByIdAsync(customer.Id);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(customer.Id, response.Data.Id);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldUpdateCustomer()
        {
            // Arrange
            using var context = CreateContext();
            var service = GetService(context);
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Bob Marley",
                Email = "bob@example.com",
                Phone = "1112223333",
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            var updatedCustomer = new Customer
            {
                Id = customer.Id,
                Name = "Bob M.",
                Email = "bob.m@example.com",
                Phone = "9998887777",
                CreatedDate = customer.CreatedDate,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };

            // Act
            var response = await service.UpdateCustomerAsync(customer.Id, updatedCustomer);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(updatedCustomer.Name, response.Data.Name);
            Assert.Equal(updatedCustomer.Email, response.Data.Email);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldReturnError_WhenCustomerNotFound()
        {
            // Arrange
            using var context = CreateContext();
            var service = GetService(context);
            var nonExistentId = Guid.NewGuid();
            var customer = new Customer
            {
                Id = nonExistentId,
                Name = "Ghost",
                Email = "ghost@example.com",
                Phone = "0000000000",
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };

            // Act
            var response = await service.UpdateCustomerAsync(nonExistentId, customer);

            // Assert
            Assert.False(response.Success);
            Assert.Null(response.Data);
            Assert.NotNull(response.Message);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldDeleteCustomer()
        {
            // Arrange
            using var context = CreateContext();
            var service = GetService(context);
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Name = "Delete Me",
                Email = "deleteme@example.com",
                Phone = "4445556666",
                CreatedDate = GetDateTimeProvider().UtcNow,
                UpdatedDate = GetDateTimeProvider().UtcNow
            };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            // Act
            var response = await service.DeleteCustomerAsync(customer.Id);

            // Assert
            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.Equal(customer.Id, response.Data.Id);

            // Verify customer is removed from DB
            var dbCustomer = await context.Customers.FindAsync(customer.Id);
            Assert.Null(dbCustomer);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldReturnError_WhenCustomerNotFound()
        {
            // Arrange
            using var context = CreateContext();
            var service = GetService(context);
            var nonExistentId = Guid.NewGuid();

            // Act
            var response = await service.DeleteCustomerAsync(nonExistentId);

            // Assert
            Assert.False(response.Success);
            Assert.Null(response.Data);
            Assert.NotNull(response.Message);
        }
    }
}
