using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SmartShop.API.Helpers;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;
using SmartShop.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SmartShop.IntegrationTests
{
    public class ProductServiceIntegrationTests
    {
        
        private SmartShopDbContext CreateContext() => TestDbContextFactory.CreateContext();
          
        DateTimeProvider GetDateTimeProvider()
        {
            // Use the actual DateTimeProvider implementation from SmartShop.API.Helpers
            return new DateTimeProvider();
        }

        private ProductService GetService(SmartShopDbContext context)
        {
            return new ProductService(context, GetDateTimeProvider());
        }

        [Fact]
        public async Task CreateProductAsync_ShouldCreateProduct()
        {
            var context = CreateContext();
            var service = GetService(context);

            var product = new Product
            {
                Name = "Test Product",
                Price = 10.5m,
                Stock = 100
            };

            var response = await service.CreateProductAsync(product);

            Assert.True(response.Success);
            Assert.Equal("Test Product", response.Data.Name);
            Assert.Equal(10.5m, response.Data.Price);
            Assert.Equal(100, response.Data.Stock);
            Assert.NotEqual(Guid.Empty, response.Data.Id);
            Assert.Equal(StatusCodes.Status201Created, response.StatusCode);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnProducts()
        {
            var context = CreateContext();
            var service = GetService(context);

            context.Products.Add(new Product { Id = Guid.NewGuid(), Name = "A", Price = 1, Stock = 1, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow });
            context.Products.Add(new Product { Id = Guid.NewGuid(), Name = "B", Price = 2, Stock = 2, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow });
            await context.SaveChangesAsync();

            var response = await service.GetAllProductsAsync();

            Assert.True(response.Success);
            Assert.Equal(2, response.Data.Count);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenExists()
        {
            var context = CreateContext();
            var service = GetService(context);

            var product = new Product { Id = Guid.NewGuid(), Name = "X", Price = 5, Stock = 5, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var response = await service.GetProductByIdAsync(product.Id);

            Assert.True(response.Success);
            Assert.Equal("X", response.Data.Name);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnError_WhenNotFound()
        {
            var context = CreateContext();
            var service = GetService(context);

            var response = await service.GetProductByIdAsync(Guid.NewGuid());

            Assert.False(response.Success);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct_WhenExists()
        {
            var context = CreateContext();
            var service = GetService(context);

            var product = new Product { Id = Guid.NewGuid(), Name = "Old", Price = 1, Stock = 1, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var updated = new Product { Id = product.Id, Name = "New", Price = 2, Stock = 2 };

            var response = await service.UpdateProductAsync(product.Id, updated);

            Assert.True(response.Success);
            Assert.Equal("New", response.Data.Name);
            Assert.Equal(2, response.Data.Price);
            Assert.Equal(2, response.Data.Stock);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldReturnError_WhenIdMismatch()
        {
            var context = CreateContext();
            var service = GetService(context);

            var product = new Product { Id = Guid.NewGuid(), Name = "Mismatch", Price = 1, Stock = 1 };

            var response = await service.UpdateProductAsync(Guid.NewGuid(), product);

            Assert.False(response.Success);
            Assert.Equal(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldDeleteProduct_WhenExists()
        {
            var context = CreateContext();
            var service = GetService(context);

            var product = new Product { Id = Guid.NewGuid(), Name = "ToDelete", Price = 1, Stock = 1, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var response = await service.DeleteProductAsync(product.Id);

            Assert.True(response.Success);
            Assert.Equal("ToDelete", response.Data.Name);
            Assert.Equal(StatusCodes.Status200OK, response.StatusCode);

            var exists = await context.Products.FindAsync(product.Id);
            Assert.Null(exists);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldReturnError_WhenNotFound()
        {
            var context = CreateContext();
            var service = GetService(context);

            var response = await service.DeleteProductAsync(Guid.NewGuid());

            Assert.False(response.Success);
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
        }
    }
}
