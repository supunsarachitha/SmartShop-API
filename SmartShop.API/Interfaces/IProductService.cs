using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Services
{
    public interface IProductService
    {
        Task<ApplicationResponse<List<Product>>> GetAllProductsAsync();
        Task<ApplicationResponse<Product>> GetProductByIdAsync(Guid id);
        Task<ApplicationResponse<Product>> UpdateProductAsync(Guid id, Product product);
        Task<ApplicationResponse<Product>> CreateProductAsync(Product product);
        Task<ApplicationResponse<Product>> DeleteProductAsync(Guid id);
    }
}
