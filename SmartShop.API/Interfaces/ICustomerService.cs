
using SmartShop.API.Models;

namespace SmartShop.API.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<bool> UpdateCustomerAsync(Guid id, Customer customer);
        Task<bool> CustomerExistsAsync(Guid id);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<bool> DeleteCustomerAsync(Guid id);

    }

}
