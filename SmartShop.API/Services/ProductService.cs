using Microsoft.EntityFrameworkCore;
using SmartShop.API.Common;
using SmartShop.API.Interfaces;
using SmartShop.API.Models;
using SmartShop.API.Models.Responses;

namespace SmartShop.API.Services
{
    public class ProductService : IProductService
    {
        private readonly SmartShopDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;

        public ProductService(SmartShopDbContext context, IDateTimeProvider dateTimeProvider)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<ApplicationResponse<List<Product>>> GetAllProductsAsync()
        {
            try
            {
                var products = await _context.Products.ToListAsync();

                return ResponseFactory.CreateSuccessResponse(
                    products,
                    "Products retrieved successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<List<Product>>(
                    "Failed to retrieve products.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<Product>> GetProductByIdAsync(Guid id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return ResponseFactory.CreateErrorResponse<Product>(
                        "Product not found.",
                        "Id",
                        "No product found with the specified ID.",
                        StatusCodes.Status404NotFound);
                }

                return ResponseFactory.CreateSuccessResponse(
                    product,
                    "Product retrieved successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<Product>(
                    "Failed to retrieve product.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<Product>> UpdateProductAsync(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return ResponseFactory.CreateErrorResponse<Product>(
                    "Product ID mismatch.",
                    "Id",
                    "The ID in the URL does not match the ID in the payload.",
                    StatusCodes.Status400BadRequest);
            }

            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return ResponseFactory.CreateErrorResponse<Product>(
                    "Product not found.",
                    "Product",
                    "No product found with the specified ID.",
                    StatusCodes.Status404NotFound);
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Stock = product.Stock;
            existingProduct.UpdatedDate = _dateTimeProvider.UtcNow;

            try
            {
                await _context.SaveChangesAsync();

                return ResponseFactory.CreateSuccessResponse(
                    existingProduct,
                    "Product updated successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<Product>(
                    "Product update failed.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<ApplicationResponse<Product>> CreateProductAsync(Product product)
        {
            try
            {
                // Ensure the product Id is initialized
                if (product.Id == Guid.Empty)
                {
                    product.Id = Guid.NewGuid();
                }
                product.CreatedDate = _dateTimeProvider.UtcNow;
                product.UpdatedDate = _dateTimeProvider.UtcNow;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return ResponseFactory.CreateSuccessResponse(
                    product,
                    "Product created successfully.",
                    StatusCodes.Status201Created);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<Product>(
                    "Product creation failed.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<ApplicationResponse<Product>> DeleteProductAsync(Guid id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    return ResponseFactory.CreateErrorResponse<Product>(
                        "Product not found.",
                        "Id",
                        "No product found with the specified ID.",
                        StatusCodes.Status404NotFound);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return ResponseFactory.CreateSuccessResponse(
                    product,
                    "Product deleted successfully.",
                    StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ResponseFactory.CreateErrorResponse<Product>(
                    "Product deletion failed.",
                    "Exception",
                    ex.Message,
                    StatusCodes.Status500InternalServerError);
            }
        }
    }
}
