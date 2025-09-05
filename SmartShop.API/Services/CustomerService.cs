using Microsoft.EntityFrameworkCore;
using SmartShop.API.Common;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly SmartShopDbContext _context;

        public CustomerService(SmartShopDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationResponse<List<Customer>>> GetAllCustomersAsync()
        {
            try
            {
                var customers = await _context.Customers.ToListAsync();

                if (customers == null || customers.Count == 0)
                {
                    return ResponseFactory.CreateErrorResponse<List<Customer>>(
                        "No customers found.",
                        "Customers",
                        "The customer list is empty.",
                        StatusCodes.Status404NotFound);
                }

                return ResponseFactory.CreateSuccessResponse(
                    customers,
                    "Customers retrieved successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<List<Customer>>(
                    "Failed to retrieve customers.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<Customer>> GetCustomerByIdAsync(Guid id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    return ResponseFactory.CreateErrorResponse<Customer>(
                        "Customer not found.",
                        "Id",
                        "No customer found with the specified ID.",
                        StatusCodes.Status404NotFound);
                }

                return ResponseFactory.CreateSuccessResponse(
                    customer,
                    "Customer retrieved successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<Customer>(
                    "Failed to retrieve customer.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<Customer>> UpdateCustomerAsync(Guid id, Customer customer)
        {
            if (id != customer.Id)
            {
                return ResponseFactory.CreateErrorResponse<Customer>(
                    "Customer ID mismatch.",
                    "Id",
                    "The ID in the URL does not match the ID in the payload.",
                    StatusCodes.Status400BadRequest);
            }

            // Check if customer exists before updating
            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
            {
                return ResponseFactory.CreateErrorResponse<Customer>(
                    "Customer not found.",
                    "Customer",
                    "No customer found with the specified ID.",
                    StatusCodes.Status404NotFound);
            }

            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.Phone = customer.Phone;
            existingCustomer.UpdatedDate = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();

                return ResponseFactory.CreateSuccessResponse(
                    existingCustomer,
                    "Customer updated successfully.",
                    StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<Customer>(
                    "Customer update failed.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<Customer>> CreateCustomerAsync(Customer customer)
        {
            try
            {
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Email == customer.Email);

                if (existingCustomer != null)
                {
                    return ResponseFactory.CreateErrorResponse<Customer>(
                        "Duplicate email detected.",
                        "Email",
                        "A customer with this email already exists.",
                        StatusCodes.Status409Conflict); // 409 = Conflict
                }

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                return ResponseFactory.CreateSuccessResponse(
                    customer,
                    "Customer created successfully.",
                    StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<Customer>(
                    "Customer creation failed.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<Customer>> DeleteCustomerAsync(Guid id)
        {
            try
            {
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    return ResponseFactory.CreateErrorResponse<Customer>(
                        "Customer not found.",
                        "Id",
                        "No customer found with the specified ID.",
                        StatusCodes.Status404NotFound);
                }

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                return ResponseFactory.CreateSuccessResponse(
                    customer,
                    "Customer deleted successfully.",
                    StatusCodes.Status204NoContent);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<Customer>(
                    "Customer deletion failed.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
