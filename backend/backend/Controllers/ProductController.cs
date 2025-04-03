using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.DTO;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Only authenticated users can access; restrict roles as needed.
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found");
            return product;
        }

        // POST: api/Product
        // Creates a new product
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> CreateProduct([FromForm] ProductCreateDto dto)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
            {
                return BadRequest("Image file is required");
            }

            using var memoryStream = new MemoryStream();
            await dto.ImageFile.CopyToAsync(memoryStream);

            var product = new Product
            {
                Name = dto.Name,
                Brand = dto.Brand,
                Category = dto.Category,
                Type = dto.Type ?? string.Empty,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                Description = dto.Description,
                ImageFile = memoryStream.ToArray()
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // PATCH: api/Product/{id}
        // Updates an existing product
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] ProductUpdateDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found");

            if (!string.IsNullOrEmpty(dto.Name))
                product.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Brand))
                product.Brand = dto.Brand;
            if (!string.IsNullOrEmpty(dto.Category))
                product.Category = dto.Category;
            if (!string.IsNullOrEmpty(dto.Type))
                product.Type = dto.Type;
            if (dto.Price.HasValue)
                product.Price = dto.Price.Value;
            if (dto.StockQuantity.HasValue)
                product.StockQuantity = dto.StockQuantity.Value;
            if (!string.IsNullOrEmpty(dto.Description))
                product.Description = dto.Description;

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await dto.ImageFile.CopyToAsync(memoryStream);
                product.ImageFile = memoryStream.ToArray();
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Product updated successfully", product });
        }

        // DELETE: api/Product/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
