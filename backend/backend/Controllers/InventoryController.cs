using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.DTO;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InventoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventories()
        {
            return await _context.Inventories.ToListAsync();
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Inventory>> AddInventory([FromForm] InventoryCreateDto dto)
        {
            Console.WriteLine($"Received data: CycleName={dto.CycleName}, Brand={dto.Brand}, Type={dto.Type}, Price={dto.Price}, StockQuantity={dto.StockQuantity}, Description={dto.Description}");
            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
            {
                return BadRequest("Image file is required");
            }

            using var memoryStream = new MemoryStream();
            await dto.ImageFile.CopyToAsync(memoryStream);

            var inventory = new Inventory
            {
                CycleName = dto.CycleName,
                Brand = dto.Brand,
                Type = dto.Type,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                ImageFile = memoryStream.ToArray(),
                Description=dto.Description
            };

            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInventories), new { id = inventory.Id }, inventory);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateInventory(int id, [FromForm] InventoryUpdateDto dto)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null) return NotFound();

            if (!string.IsNullOrEmpty(dto.CycleName)) inventory.CycleName = dto.CycleName;
            if (!string.IsNullOrEmpty(dto.Brand)) inventory.Brand = dto.Brand;
            if (!string.IsNullOrEmpty(dto.Type)) inventory.Type = dto.Type;
            if (dto.Price.HasValue) inventory.Price = dto.Price.Value;
            if (dto.StockQuantity.HasValue) inventory.StockQuantity = dto.StockQuantity.Value;

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await dto.ImageFile.CopyToAsync(memoryStream);
                inventory.ImageFile = memoryStream.ToArray();
            }

            if (!string.IsNullOrEmpty(dto.Description)) inventory.Description = dto.Description;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Inventory updated successfully", inventory });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory == null)
            {
                return NotFound(new { message = "Inventory item not found" });
            }

            _context.Inventories.Remove(inventory);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
