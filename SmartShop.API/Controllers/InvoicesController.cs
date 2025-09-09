using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartShop.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly ILogger<InvoicesController> _logger;
        private readonly IInvoiceService _invoiceService;

        public InvoicesController(ILogger<InvoicesController> logger, IInvoiceService invoiceService)
        {
            _logger = logger;
            _invoiceService = invoiceService;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var response = await _invoiceService.GetAllInvoicesAsync();
            return StatusCode(response.StatusCode ?? StatusCodes.Status200OK, response);
        }

        // GET: api/Invoices/guid
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(Guid id)
        {
            var response = await _invoiceService.GetInvoiceByIdAsync(id);
            return StatusCode(response.StatusCode ?? StatusCodes.Status200OK, response);
        }

        // PUT: api/Invoices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(Guid id, Invoice invoice)
        {
            var response = await _invoiceService.UpdateInvoiceAsync(id, invoice);
            return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);
        }

        // POST: api/Invoices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostInvoice(Invoice invoice)
        {
            var response = await _invoiceService.CreateInvoiceAsync(invoice);

            if (!response.Success)
                return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);

            if (response.Data == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApplicationResponse<Invoice>
                {
                    Success = false,
                    Message = "No invoice data was returned.",
                    Data = default!,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }

            return CreatedAtAction(nameof(GetInvoice), new { id = response.Data.Id }, response);
        }

        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            var response = await _invoiceService.DeleteInvoiceAsync(id);
            return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);
        }
    }
}
