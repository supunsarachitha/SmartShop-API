using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SmartShop.API.Controllers;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;
using SmartShop.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartShop.UnitTests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly Mock<ILogger<ProductsController>> _mockLogger;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _mockLogger = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_mockLogger.Object, _mockProductService.Object);
        }

        [Fact]
        public async Task GetProduct_ReturnsOk_WhenProductExists()
        {
            var productId = Guid.NewGuid();
            var expectedProduct = new Product { Id = productId, Name = "Test Product" };
            var serviceResponse = new ApplicationResponse<Product>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = expectedProduct
            };

            _mockProductService
                .Setup(s => s.GetProductByIdAsync(productId))
                .ReturnsAsync(serviceResponse);

            var result = await _controller.GetProduct(productId);

            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedResponse = Assert.IsType<ApplicationResponse<Product>>(okResult.Value);
            Assert.Equal(expectedProduct.Id, returnedResponse.Data.Id);
        }

        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductIsNull()
        {
            var productId = Guid.NewGuid();
            var serviceResponse = new ApplicationResponse<Product>
            {
                Success = false,
                StatusCode = StatusCodes.Status404NotFound,
                Data = null
            };

            _mockProductService
                .Setup(s => s.GetProductByIdAsync(productId))
                .ReturnsAsync(serviceResponse);

            var result = await _controller.GetProduct(productId);

            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetProducts_ReturnsOk_WithProductList()
        {
            var products = new List<Product>
                {
                    new Product { Id = Guid.NewGuid(), Name = "Product 1" },
                    new Product { Id = Guid.NewGuid(), Name = "Product 2" }
                };
            var serviceResponse = new ApplicationResponse<List<Product>>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = products
            };

            _mockProductService
                .Setup(s => s.GetAllProductsAsync())
                .ReturnsAsync(serviceResponse);

            var result = await _controller.GetProducts();

            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedResponse = Assert.IsType<ApplicationResponse<List<Product>>>(okResult.Value);
            Assert.Equal(2, returnedResponse.Data.Count);
        }

        [Fact]
        public async Task PostProduct_ReturnsCreated_WhenProductIsCreated()
        {
            var product = new Product { Id = Guid.NewGuid(), Name = "New Product" };
            var serviceResponse = new ApplicationResponse<Product>
            {
                Success = true,
                StatusCode = StatusCodes.Status201Created,
                Data = product
            };

            _mockProductService
                .Setup(s => s.CreateProductAsync(product))
                .ReturnsAsync(serviceResponse);

            var result = await _controller.PostProduct(product);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            var returnedResponse = Assert.IsType<ApplicationResponse<Product>>(createdResult.Value);
            Assert.Equal(product.Id, returnedResponse.Data.Id);
        }

        [Fact]
        public async Task PutProduct_ReturnsOk_WhenProductIsUpdated()
        {
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Updated Product" };
            var serviceResponse = new ApplicationResponse<Product>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = product
            };

            _mockProductService
                .Setup(s => s.UpdateProductAsync(productId, product))
                .ReturnsAsync(serviceResponse);

            var result = await _controller.PutProduct(productId, product);

            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedResponse = Assert.IsType<ApplicationResponse<Product>>(okResult.Value);
            Assert.Equal(productId, returnedResponse.Data.Id);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsOk_WhenProductIsDeleted()
        {
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Deleted Product" };
            var serviceResponse = new ApplicationResponse<Product>
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = product
            };

            _mockProductService
                .Setup(s => s.DeleteProductAsync(productId))
                .ReturnsAsync(serviceResponse);

            var result = await _controller.DeleteProduct(productId);

            var okResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedResponse = Assert.IsType<ApplicationResponse<Product>>(okResult.Value);
            Assert.Equal(productId, returnedResponse.Data.Id);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            var productId = Guid.NewGuid();
            var serviceResponse = new ApplicationResponse<Product>
            {
                Success = false,
                StatusCode = StatusCodes.Status404NotFound,
                Data = null
            };

            _mockProductService
                .Setup(s => s.DeleteProductAsync(productId))
                .ReturnsAsync(serviceResponse);

            var result = await _controller.DeleteProduct(productId);

            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }
    }

}


