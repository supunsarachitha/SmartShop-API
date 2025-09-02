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
        private readonly SmartShopDbContext _context;
        private readonly ILogger<CustomersController> _logger;
        public ProductsController(SmartShopDbContext context, ILogger<CustomersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all products.
        /// </summary>
        /// <remarks>This method performs an asynchronous operation to fetch all products from the
        /// database. The result is returned as an HTTP response with a status code of 200 (OK) if successful.</remarks>
        /// <returns>A task that represents the asynchronous operation. The task result contains an  <see
        /// cref="ActionResult{T}"/> of <see cref="IEnumerable{T}"/> containing all products.</returns>
        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }


        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <remarks>This method performs a database lookup to find the product with the specified
        /// identifier. If the product is not found, a 404 Not Found response is returned.</remarks>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the product with the specified identifier  if found; otherwise,
        /// a <see cref="NotFoundResult"/> if the product does not exist.</returns>
        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }


        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <remarks>This method uses Entity Framework to update the product in the database. It performs
        /// a concurrency check to ensure the product still exists before saving changes.</remarks>
        /// <param name="id">The identifier of the product to update.</param>
        /// <param name="product">The updated product data. The <see cref="Product.Id"/> property must match the <paramref name="id"/>
        /// parameter.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation: <list type="bullet">
        /// <item><description><see cref="BadRequestResult"/> if the <paramref name="id"/> does not match the <see
        /// cref="Product.Id"/> property of the provided <paramref name="product"/>.</description></item>
        /// <item><description><see cref="NotFoundResult"/> if no product with the specified <paramref name="id"/>
        /// exists.</description></item> <item><description><see cref="NoContentResult"/> if the update is
        /// successful.</description></item> </list></returns>
        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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
        /// Creates a new product and adds it to the database.
        /// </summary>
        /// <remarks>This method adds the provided product to the database and saves the changes
        /// asynchronously.  The response includes the URI of the newly created product resource.</remarks>
        /// <param name="product">The product to be added. The product must not be null and should contain valid data.</param>
        /// <returns>An <see cref="ActionResult{T}"/> containing the created product, along with a status code of 201 (Created) 
        /// and a location header pointing to the newly created product.</returns>

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }


        /// <summary>
        /// Deletes the product with the specified identifier.
        /// </summary>
        /// <remarks>This action removes the product from the database. Ensure the specified <paramref
        /// name="id"/>  corresponds to an existing product before calling this method.</remarks>
        /// <param name="id">The unique identifier of the product to delete.</param>
        /// <returns>A <see cref="NoContentResult"/> if the product was successfully deleted;  otherwise, a <see
        /// cref="NotFoundResult"/> if no product with the specified identifier exists.</returns>
        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Determines whether a product with the specified identifier exists in the data source.
        /// </summary>
        /// <param name="id">The unique identifier of the product to check for existence.</param>
        /// <returns><see langword="true"/> if a product with the specified identifier exists; otherwise, <see
        /// langword="false"/>.</returns>
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
