using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;

namespace SmartShop.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    { 
        private readonly ILogger<CustomersController> _logger;
        private readonly ICustomerService _customerService;

        public CustomersController(SmartShopDbContext context, ILogger<CustomersController> logger, ICustomerService customerService)
        { 
            _logger = logger;
            _customerService = customerService;
        }

        // GET: api/Customers
        [HttpGet]
        public async Task<IActionResult> GetCustomers()
        {
            var response = await _customerService.GetAllCustomersAsync();
            return StatusCode(response.StatusCode ?? StatusCodes.Status200OK, response);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(Guid id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);
            return StatusCode(response.StatusCode ?? StatusCodes.Status200OK, response);
        }
         
        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(Guid id, Customer customer)
        {
            var response = await _customerService.UpdateCustomerAsync(id, customer);
            return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);
        }

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostCustomer(Customer customer)
        {
            var response = await _customerService.CreateCustomerAsync(customer);

            if (!response.Success)
                return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);

            return CreatedAtAction(nameof(GetCustomer), new { id = response.Data.Id }, response);
        }

        // DELETE: api/Customers/5 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var response = await _customerService.DeleteCustomerAsync(id);
            return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);
        }

    }
}
