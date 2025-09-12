using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SmartShop.API.Controllers;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;
using Xunit;

namespace SmartShop.UnitTests
{
    public class CustomersControllerTests
    {
        private readonly Mock<ILogger<CustomersController>> _loggerMock;
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _loggerMock = new Mock<ILogger<CustomersController>>();
            _customerServiceMock = new Mock<ICustomerService>();
            _controller = new CustomersController(_loggerMock.Object, _customerServiceMock.Object);
        }

        [Fact]
        public async Task GetCustomers_ReturnsOk_WithCustomers()
        {
            var customers = new List<Customer> { new Customer { Id = Guid.NewGuid(), Name = "Test" } };
            var response = new ApplicationResponse<List<Customer>> { Success = true, Data = customers, StatusCode = StatusCodes.Status200OK };
            _customerServiceMock.Setup(s => s.GetAllCustomersAsync()).ReturnsAsync(response);

            var result = await _controller.GetCustomers();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task GetCustomer_ReturnsOk_WithCustomer()
        {
            var customer = new Customer { Id = Guid.NewGuid(), Name = "Test" };
            var response = new ApplicationResponse<Customer> { Success = true, Data = customer, StatusCode = StatusCodes.Status200OK };
            _customerServiceMock.Setup(s => s.GetCustomerByIdAsync(customer.Id)).ReturnsAsync(response);

            var result = await _controller.GetCustomer(customer.Id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }


        [Fact]
        public async Task PutCustomer_ReturnsBadRequest_WhenFailed()
        {
            var customer = new Customer { Id = Guid.NewGuid(), Name = "Test" };
            var response = new ApplicationResponse<Customer> { Success = false, StatusCode = StatusCodes.Status400BadRequest };
            _customerServiceMock.Setup(s => s.UpdateCustomerAsync(customer.Id, customer)).ReturnsAsync(response);

            var result = await _controller.PutCustomer(customer.Id, customer);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task PutCustomer_ReturnsOk_WhenSuccess()
        {
            var customer = new Customer { Id = Guid.NewGuid(), Name = "Test" };
            var response = new ApplicationResponse<Customer> { Success = true, Data = customer, StatusCode = StatusCodes.Status200OK };
            _customerServiceMock.Setup(s => s.UpdateCustomerAsync(customer.Id, customer)).ReturnsAsync(response);

            var result = await _controller.PutCustomer(customer.Id, customer);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task PostCustomer_ReturnsCreatedAtAction_WhenSuccess()
        {
            var customer = new Customer { Id = Guid.NewGuid(), Name = "Test" };
            var response = new ApplicationResponse<Customer> { Success = true, Data = customer, StatusCode = StatusCodes.Status201Created };
            _customerServiceMock.Setup(s => s.CreateCustomerAsync(customer)).ReturnsAsync(response);

            var result = await _controller.PostCustomer(customer);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetCustomer), createdResult.ActionName);
            Assert.Equal(customer.Id, ((dynamic)createdResult.RouteValues)["id"]);
            Assert.Equal(response, createdResult.Value);
        }

        [Fact]
        public async Task PostCustomer_ReturnsBadRequest_WhenFailed()
        {
            var customer = new Customer { Id = Guid.NewGuid(), Name = "Test" };
            var response = new ApplicationResponse<Customer> { Success = false, StatusCode = StatusCodes.Status400BadRequest };
            _customerServiceMock.Setup(s => s.CreateCustomerAsync(customer)).ReturnsAsync(response);

            var result = await _controller.PostCustomer(customer);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsBadRequest_WhenFailed()
        {
            var id = Guid.NewGuid();
            var response = new ApplicationResponse<Customer> { Success = false, StatusCode = StatusCodes.Status400BadRequest };
            _customerServiceMock.Setup(s => s.DeleteCustomerAsync(id)).ReturnsAsync(response);

            var result = await _controller.DeleteCustomer(id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsOk_WhenSuccess()
        {
            var id = Guid.NewGuid();
            var response = new ApplicationResponse<Customer> { Success = true, StatusCode = StatusCodes.Status200OK };
            _customerServiceMock.Setup(s => s.DeleteCustomerAsync(id)).ReturnsAsync(response);

            var result = await _controller.DeleteCustomer(id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }
    }
}
