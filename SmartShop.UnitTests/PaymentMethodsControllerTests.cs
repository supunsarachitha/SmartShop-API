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
    public class PaymentMethodsControllerTests
    {
        private readonly Mock<ILogger<PaymentMethodsController>> _loggerMock;
        private readonly Mock<IPaymentMethods> _serviceMock;
        private readonly PaymentMethodsController _controller;

        public PaymentMethodsControllerTests()
        {
            _loggerMock = new Mock<ILogger<PaymentMethodsController>>();
            _serviceMock = new Mock<IPaymentMethods>();
            _controller = new PaymentMethodsController(_loggerMock.Object, _serviceMock.Object);
        }

        [Fact]
        public async Task GetPaymentMethods_ReturnsOk()
        {
            var response = new ApplicationResponse<List<PaymentMethod>>
            {
                Success = true,
                Data = new List<PaymentMethod>(),
                StatusCode = StatusCodes.Status200OK
            };
            _serviceMock.Setup(s => s.GetAllPaymentMethodsAsync()).ReturnsAsync(response);

            var result = await _controller.GetPaymentMethods();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task GetPaymentMethod_ReturnsOk()
        {
            var id = Guid.NewGuid();
            var paymentMethod = new PaymentMethod { Id = id, Name = "Test", Description = "Desc", Type = 1 };
            var response = new ApplicationResponse<PaymentMethod>
            {
                Success = true,
                Data = paymentMethod,
                StatusCode = StatusCodes.Status200OK
            };
            _serviceMock.Setup(s => s.GetPaymentMethodByIdAsync(id)).ReturnsAsync(response);

            var result = await _controller.GetPaymentMethod(id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task PutPaymentMethod_ReturnsBadRequestOnFailure()
        {
            var id = Guid.NewGuid();
            var paymentMethod = new PaymentMethod { Id = id, Name = "Test", Description = "Desc", Type = 1 };
            var response = new ApplicationResponse<PaymentMethod>
            {
                Success = false,
                Message = "Failed",
                StatusCode = StatusCodes.Status400BadRequest
            };
            _serviceMock.Setup(s => s.UpdatePaymentMethodAsync(id, paymentMethod)).ReturnsAsync(response);

            var result = await _controller.PutPaymentMethod(id, paymentMethod);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task PutPaymentMethod_ReturnsOkOnSuccess()
        {
            var id = Guid.NewGuid();
            var paymentMethod = new PaymentMethod { Id = id, Name = "Test", Description = "Desc", Type = 1 };
            var response = new ApplicationResponse<PaymentMethod>
            {
                Success = true,
                Data = paymentMethod,
                StatusCode = StatusCodes.Status200OK
            };
            _serviceMock.Setup(s => s.UpdatePaymentMethodAsync(id, paymentMethod)).ReturnsAsync(response);

            var result = await _controller.PutPaymentMethod(id, paymentMethod);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task PostPaymentMethod_ReturnsCreatedOnSuccess()
        {
            var id = Guid.NewGuid();
            var paymentMethod = new PaymentMethod { Id = id, Name = "Test", Description = "Desc", Type = 1 };
            var response = new ApplicationResponse<PaymentMethod>
            {
                Success = true,
                Data = paymentMethod,
                StatusCode = StatusCodes.Status201Created
            };
            _serviceMock.Setup(s => s.CreatePaymentMethodAsync(paymentMethod)).ReturnsAsync(response);

            var result = await _controller.PostPaymentMethod(paymentMethod);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetPaymentMethod), createdResult.ActionName);
            Assert.Equal(id, ((dynamic)createdResult.RouteValues)["id"]);
            Assert.Equal(response, createdResult.Value);
        }

        [Fact]
        public async Task PostPaymentMethod_ReturnsBadRequestOnFailure()
        {
            var paymentMethod = new PaymentMethod { Id = Guid.NewGuid(), Name = "Test", Description = "Desc", Type = 1 };
            var response = new ApplicationResponse<PaymentMethod>
            {
                Success = false,
                Message = "Failed",
                StatusCode = StatusCodes.Status400BadRequest
            };
            _serviceMock.Setup(s => s.CreatePaymentMethodAsync(paymentMethod)).ReturnsAsync(response);

            var result = await _controller.PostPaymentMethod(paymentMethod);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task DeletePaymentMethod_ReturnsBadRequestOnFailure()
        {
            var id = Guid.NewGuid();
            var response = new ApplicationResponse<PaymentMethod>
            {
                Success = false,
                Message = "Failed",
                StatusCode = StatusCodes.Status400BadRequest
            };
            _serviceMock.Setup(s => s.DeletePaymentMethodAsync(id)).ReturnsAsync(response);

            var result = await _controller.DeletePaymentMethod(id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task DeletePaymentMethod_ReturnsOkOnSuccess()
        {
            var id = Guid.NewGuid();
            var response = new ApplicationResponse<PaymentMethod>
            {
                Success = true,
                Data = new PaymentMethod { Id = id, Name = "Test", Description = "Desc", Type = 1 },
                StatusCode = StatusCodes.Status200OK
            };
            _serviceMock.Setup(s => s.DeletePaymentMethodAsync(id)).ReturnsAsync(response);

            var result = await _controller.DeletePaymentMethod(id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

    }
}
