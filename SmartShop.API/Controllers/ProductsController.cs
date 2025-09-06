using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartShop.API.Models;
using SmartShop.API.Services;

namespace SmartShop.API.Controllers
{
    /// <summary>
    /// Provides endpoints for managing products in the system.
    /// </summary>
    /// <remarks>This controller handles operations such as retrieving, creating, updating, and deleting
    /// products. All actions are exposed as RESTful API endpoints and follow standard HTTP conventions.</remarks>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly IProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var response = await _productService.GetAllProductsAsync();
            return StatusCode(response.StatusCode ?? StatusCodes.Status200OK, response);
        }

        // GET: api/Products/guid
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var response = await _productService.GetProductByIdAsync(id);
            return StatusCode(response.StatusCode ?? StatusCodes.Status200OK, response);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(Guid id, Product product)
        {
            var response = await _productService.UpdateProductAsync(id, product);
            return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostProduct(Product product)
        {
            var response = await _productService.CreateProductAsync(product);

            if (!response.Success)
                return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);

            return CreatedAtAction(nameof(GetProduct), new { id = response.Data.Id }, response);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var response = await _productService.DeleteProductAsync(id);
            return StatusCode(response.StatusCode ?? StatusCodes.Status400BadRequest, response);
        }
    }
}
