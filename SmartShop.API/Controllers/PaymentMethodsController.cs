using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;


namespace SmartShop.API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing payment methods in the system.
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private readonly ILogger<PaymentMethodsController> _logger;
        private readonly IPaymentMethods _paymentMethodService;

        public PaymentMethodsController(ILogger<PaymentMethodsController> logger, IPaymentMethods paymentMethodService)
        {
            _logger = logger;
            _paymentMethodService = paymentMethodService;
        }

        // GET: api/PaymentMethods
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var response = await _paymentMethodService.GetAllPaymentMethodsAsync();
            return StatusCode(response.StatusCode ?? StatusCodes.Status200OK, response);
        }

        // GET: api/PaymentMethods/guid
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPaymentMethod(Guid id)
        {
            var response = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
            return StatusCode(response.StatusCode ?? StatusCodes.Status200OK, response);
        }

        // PUT: api/PaymentMethods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentMethod(Guid id, PaymentMethod paymentMethod)
        {
            var response = await _paymentMethodService.UpdatePaymentMethodAsync(id, paymentMethod);
            return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);
        }

        // POST: api/PaymentMethods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostPaymentMethod(PaymentMethod paymentMethod)
        {
            var response = await _paymentMethodService.CreatePaymentMethodAsync(paymentMethod);

            if (!response.Success)
                return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);

            return CreatedAtAction(nameof(GetPaymentMethod), new { id = response.Data.Id }, response);
        }

        // DELETE: api/PaymentMethods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaymentMethod(Guid id)
        {
            var response = await _paymentMethodService.DeletePaymentMethodAsync(id);
            return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);
        }
    }
}
