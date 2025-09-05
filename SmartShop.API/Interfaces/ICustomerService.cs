
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Interfaces
{
    public interface ICustomerService
    {
        Task<ApplicationResponse<List<Customer>>> GetAllCustomersAsync();
        Task<ApplicationResponse<Customer>> GetCustomerByIdAsync(Guid id);
        Task<ApplicationResponse<Customer>> UpdateCustomerAsync(Guid id, Customer customer);
        Task<ApplicationResponse<Customer>> CreateCustomerAsync(Customer customer);
        Task<ApplicationResponse<Customer>> DeleteCustomerAsync(Guid id);
    }

}
