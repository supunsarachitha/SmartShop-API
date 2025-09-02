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
    /// Provides endpoints for managing payment methods in the system.
    /// </summary>
    /// <remarks>This controller supports CRUD operations for payment methods, including retrieving all
    /// payment methods, retrieving a specific payment method by ID, creating new payment methods, updating existing
    /// payment methods, and deleting payment methods. All endpoints require authorization.</remarks>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly SmartShopDbContext _context;
        private readonly ILogger<CustomersController> _logger;

        public PaymentMethodsController(SmartShopDbContext context, ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }


        /// <summary>
        /// Retrieves a list of all available payment methods.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch all payment methods from the
        /// database. The result is returned as an HTTP response with the list of payment methods.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see
        /// cref="ActionResult{T}"/> of <see cref="IEnumerable{T}"/> containing the payment methods.</returns>
        // GET: api/PaymentMethods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetPaymentMethod()
        {
            return await _context.PaymentMethod.ToListAsync();
        }

        /// <summary>
        /// Retrieves a payment method by its unique identifier.
        /// </summary>
        /// <remarks>This method performs a lookup in the database for a payment method with the specified
        /// identifier. If the payment method is not found, a 404 Not Found response is returned.</remarks>
        /// <param name="id">The unique identifier of the payment method to retrieve.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the <see cref="PaymentMethod"/> if found;  otherwise, a <see
        /// cref="NotFoundResult"/> if no payment method with the specified identifier exists.</returns>
        // GET: api/PaymentMethods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentMethod>> GetPaymentMethod(Guid id)
        {
            var paymentMethod = await _context.PaymentMethod.FindAsync(id);

            if (paymentMethod == null)
            {
                return NotFound();
            }

            return paymentMethod;
        }


        /// <summary>
        /// Updates an existing payment method with the specified identifier.
        /// </summary>
        /// <remarks>This method uses optimistic concurrency control. If a concurrency conflict occurs
        /// during the update,  the exception is rethrown unless the payment method does not exist, in which case a <see
        /// cref="NotFoundResult"/> is returned.</remarks>
        /// <param name="id">The unique identifier of the payment method to update.</param>
        /// <param name="paymentMethod">The updated payment method data.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.  Returns <see
        /// cref="BadRequestResult"/> if the <paramref name="id"/> does not match the <see cref="PaymentMethod.Id"/>. 
        /// Returns <see cref="NotFoundResult"/> if the payment method does not exist.  Returns <see
        /// cref="NoContentResult"/> if the update is successful.</returns>
        // PUT: api/PaymentMethods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentMethod(Guid id, PaymentMethod paymentMethod)
        {
            if (id != paymentMethod.Id)
            {
                return BadRequest();
            }

            _context.Entry(paymentMethod).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentMethodExists(id))
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
        /// Creates a new payment method and adds it to the database.
        /// </summary>
        /// <remarks>This method uses the HTTP POST verb to create a new payment method.  Ensure that the
        /// <paramref name="paymentMethod"/> parameter contains valid data before calling this method.</remarks>
        /// <param name="paymentMethod">The payment method to be added. This object must contain valid data for all required fields.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the created <see cref="PaymentMethod"/> object,  along with a
        /// response status of 201 (Created) and a location header pointing to the newly created resource.</returns>
        // POST: api/PaymentMethods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PaymentMethod>> PostPaymentMethod(PaymentMethod paymentMethod)
        {
            _context.PaymentMethod.Add(paymentMethod);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentMethod", new { id = paymentMethod.Id }, paymentMethod);
        }

        /// <summary>
        /// Deletes the payment method with the specified identifier.
        /// </summary>
        /// <remarks>This operation removes the payment method from the database. Ensure that the
        /// specified  <paramref name="id"/> corresponds to an existing payment method before calling this
        /// method.</remarks>
        /// <param name="id">The unique identifier of the payment method to delete.</param>
        /// <returns>A <see cref="NoContentResult"/> if the payment method was successfully deleted;  otherwise, a <see
        /// cref="NotFoundResult"/> if no payment method with the specified identifier exists.</returns>
        // DELETE: api/PaymentMethods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod(Guid id)
        {
            var paymentMethod = await _context.PaymentMethod.FindAsync(id);
            if (paymentMethod == null)
            {
                return NotFound();
            }

            _context.PaymentMethod.Remove(paymentMethod);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Determines whether a payment method with the specified identifier exists in the data store.
        /// </summary>
        /// <param name="id">The unique identifier of the payment method to check for existence.</param>
        /// <returns><see langword="true"/> if a payment method with the specified identifier exists; otherwise, <see
        /// langword="false"/>.</returns>

        private bool PaymentMethodExists(Guid id)
        {
            return _context.PaymentMethod.Any(e => e.Id == id);
        }
    }
}
