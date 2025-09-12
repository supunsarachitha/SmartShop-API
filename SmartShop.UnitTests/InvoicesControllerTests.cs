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
    public class InvoicesControllerTests
    {
        private readonly Mock<IInvoiceService> _invoiceServiceMock;
        private readonly Mock<ILogger<InvoicesController>> _loggerMock;
        private readonly InvoicesController _controller;

        public InvoicesControllerTests()
        {
            _invoiceServiceMock = new Mock<IInvoiceService>();
            _loggerMock = new Mock<ILogger<InvoicesController>>();
            _controller = new InvoicesController(_loggerMock.Object, _invoiceServiceMock.Object);
        }

        [Fact]
        public async Task GetInvoices_ReturnsOkWithInvoices()
        {
            var invoices = new List<Invoice> { new Invoice { Id = Guid.NewGuid() } };
            var response = new ApplicationResponse<List<Invoice>> { Success = true, Data = invoices, StatusCode = StatusCodes.Status200OK };
            _invoiceServiceMock.Setup(s => s.GetAllInvoicesAsync()).ReturnsAsync(response);

            var result = await _controller.GetInvoices();

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task GetInvoice_ReturnsOkWithInvoice()
        {
            var invoice = new Invoice { Id = Guid.NewGuid() };
            var response = new ApplicationResponse<Invoice> { Success = true, Data = invoice, StatusCode = StatusCodes.Status200OK };
            _invoiceServiceMock.Setup(s => s.GetInvoiceByIdAsync(invoice.Id)).ReturnsAsync(response);

            var result = await _controller.GetInvoice(invoice.Id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task PutInvoice_ReturnsBadRequestOnFailure()
        {
            var invoice = new Invoice { Id = Guid.NewGuid() };
            var response = new ApplicationResponse<Invoice> { Success = false, StatusCode = StatusCodes.Status400BadRequest };
            _invoiceServiceMock.Setup(s => s.UpdateInvoiceAsync(invoice.Id, invoice)).ReturnsAsync(response);

            var result = await _controller.PutInvoice(invoice.Id, invoice);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task PutInvoice_ReturnsOkOnSuccess()
        {
            var invoice = new Invoice { Id = Guid.NewGuid() };
            var response = new ApplicationResponse<Invoice>
            {
                Success = true,
                Data = invoice,
                StatusCode = StatusCodes.Status200OK
            };
            _invoiceServiceMock.Setup(s => s.UpdateInvoiceAsync(invoice.Id, invoice)).ReturnsAsync(response);

            var result = await _controller.PutInvoice(invoice.Id, invoice);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task PostInvoice_ReturnsCreatedOnSuccess()
        {
            var invoice = new Invoice { Id = Guid.NewGuid() };
            var response = new ApplicationResponse<Invoice> { Success = true, Data = invoice, StatusCode = StatusCodes.Status201Created };
            _invoiceServiceMock.Setup(s => s.CreateInvoiceAsync(invoice)).ReturnsAsync(response);

            var result = await _controller.PostInvoice(invoice);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetInvoice), createdResult.ActionName);
            Assert.Equal(invoice.Id, ((dynamic)createdResult.RouteValues)["id"]);
            Assert.Equal(response, createdResult.Value);
        }

        [Fact]
        public async Task PostInvoice_ReturnsBadRequestOnFailure()
        {
            var invoice = new Invoice { Id = Guid.NewGuid() };
            var response = new ApplicationResponse<Invoice> { Success = false, StatusCode = StatusCodes.Status400BadRequest };
            _invoiceServiceMock.Setup(s => s.CreateInvoiceAsync(invoice)).ReturnsAsync(response);

            var result = await _controller.PostInvoice(invoice);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }

        [Fact]
        public async Task PostInvoice_ReturnsInternalServerErrorIfDataNull()
        {
            var invoice = new Invoice { Id = Guid.NewGuid() };
            var response = new ApplicationResponse<Invoice> { Success = true, Data = null, StatusCode = StatusCodes.Status201Created };
            _invoiceServiceMock.Setup(s => s.CreateInvoiceAsync(invoice)).ReturnsAsync(response);

            var result = await _controller.PostInvoice(invoice);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
            var appResponse = Assert.IsType<ApplicationResponse<Invoice>>(objectResult.Value);
            Assert.False(appResponse.Success);
            Assert.Equal("No invoice data was returned.", appResponse.Message);
        }

        [Fact]
        public async Task DeleteInvoice_ReturnsBadRequestOnFailure()
        {
            var id = Guid.NewGuid();
            var response = new ApplicationResponse<Invoice> { Success = false, StatusCode = StatusCodes.Status400BadRequest };
            _invoiceServiceMock.Setup(s => s.DeleteInvoiceAsync(id)).ReturnsAsync(response);

            var result = await _controller.DeleteInvoice(id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }
        [Fact]
        public async Task DeleteInvoice_ReturnsOkOnSuccess()
        {
            var id = Guid.NewGuid();
            var invoice = new Invoice { Id = id };
            var response = new ApplicationResponse<Invoice>
            {
                Success = true,
                Data = invoice,
                StatusCode = StatusCodes.Status200OK
            };
            _invoiceServiceMock.Setup(s => s.DeleteInvoiceAsync(id)).ReturnsAsync(response);

            var result = await _controller.DeleteInvoice(id);

            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
            Assert.Equal(response, objectResult.Value);
        }


    }
}

