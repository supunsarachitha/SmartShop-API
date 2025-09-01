using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartShop.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartShop.API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing invoices in the system.
    /// </summary>
    /// <remarks>This controller handles CRUD operations for invoices, including retrieving, creating,
    /// updating, and deleting invoices. All endpoints require authorization and are accessible via the route
    /// <c>api/Invoices</c>.</remarks>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly SmartShopDbContext _context;
        private readonly ILogger<CustomersController> _logger;
        public InvoicesController(SmartShopDbContext context, ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all invoices.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch all invoices from the
        /// database. The result is returned as an HTTP response with the list of invoices.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see
        /// cref="ActionResult{T}"/> of <see cref="IEnumerable{T}"/> containing all invoices.</returns>
        // GET: api/Invoices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices.ToListAsync();
        }

        /// <summary>
        /// Retrieves an invoice by its unique identifier.
        /// </summary>
        /// <remarks>This method performs a lookup in the database for an invoice with the specified
        /// <paramref name="id"/>. If no matching invoice is found, a 404 Not Found response is returned.</remarks>
        /// <param name="id">The unique identifier of the invoice to retrieve.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the requested <see cref="Invoice"/> if found;  otherwise, a <see
        /// cref="NotFoundResult"/> if the invoice does not exist.</returns>

        // GET: api/Invoices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(Guid id)
        {
            var invoice = await _context.Invoices.FindAsync(id);

            if (invoice == null)
            {
                return NotFound();
            }

            return invoice;
        }


        /// <summary>
        /// Updates an existing invoice with the specified identifier.
        /// </summary>
        /// <remarks>This method uses optimistic concurrency control. If a concurrency conflict occurs
        /// during the update, the exception is rethrown unless the invoice does not exist.</remarks>
        /// <param name="id">The unique identifier of the invoice to update.</param>
        /// <param name="invoice">The updated invoice data. The <see cref="Invoice.Id"/> property must match the <paramref name="id"/>
        /// parameter.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation: <list type="bullet">
        /// <item><description><see cref="BadRequestResult"/> if the <paramref name="id"/> does not match the <see
        /// cref="Invoice.Id"/> property of the provided <paramref name="invoice"/>.</description></item>
        /// <item><description><see cref="NotFoundResult"/> if no invoice with the specified <paramref name="id"/>
        /// exists.</description></item> <item><description><see cref="NoContentResult"/> if the update is
        /// successful.</description></item> </list></returns>
        // PUT: api/Invoices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(Guid id, Invoice invoice)
        {
            if (id != invoice.Id)
            {
                return BadRequest();
            }

            _context.Entry(invoice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        /// <summary>
        /// Creates a new invoice and adds it to the database.
        /// </summary>
        /// <remarks>This method adds the provided invoice to the database and saves the changes
        /// asynchronously.  Upon successful creation, it returns a 201 Created response with the location of the newly
        /// created resource.</remarks>
        /// <param name="invoice">The <see cref="Invoice"/> object to be added. Must not be null.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the created <see cref="Invoice"/> object,  along with a response
        /// indicating the resource was successfully created.</returns>
        // POST: api/Invoices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvoice", new { id = invoice.Id }, invoice);
        }

        /// <summary>
        /// Deletes the invoice with the specified identifier.
        /// </summary>
        /// <remarks>This operation is idempotent. If the specified invoice does not exist, the method
        /// returns  <see cref="NotFoundResult"/> without making any changes.</remarks>
        /// <param name="id">The unique identifier of the invoice to delete.</param>
        /// <returns>A <see cref="NoContentResult"/> if the invoice was successfully deleted;  otherwise, a <see
        /// cref="NotFoundResult"/> if no invoice with the specified identifier exists.</returns>
        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(Guid id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Determines whether an invoice with the specified identifier exists in the database.
        /// </summary>
        /// <remarks>This method performs a query against the database to check for the presence of an
        /// invoice with the given identifier.</remarks>
        /// <param name="id">The unique identifier of the invoice to check for existence.</param>
        /// <returns><see langword="true"/> if an invoice with the specified identifier exists; otherwise, <see
        /// langword="false"/>.</returns>
        private bool InvoiceExists(Guid id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }
    }
}
