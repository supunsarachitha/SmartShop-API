using Microsoft.EntityFrameworkCore;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;

namespace SmartShop.API.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly SmartShopDbContext _context;

        public CustomerService(SmartShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<bool> UpdateCustomerAsync(Guid id, Customer customer)
        {
            if (id != customer.Id)
            {
                return false;
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExistsAsync(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> CustomerExistsAsync(Guid id)
        {
            return await _context.Customers.AnyAsync(e => e.Id == id);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return false;
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
