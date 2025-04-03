using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using backend.DTO;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessoryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccessoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Accessory>>> GetAccessories()
        {
            return await _context.Accessories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Accessory>> GetAccessory(int id)
        {
            var accessory = await _context.Accessories.FindAsync(id);
            if (accessory == null)
            {
                return NotFound("Accessory not found");
            }
            return accessory;
        }

        [HttpPost]
        public async Task<ActionResult<Accessory>> PostAccessory([FromForm] AccessoryCreateDto dto)
        {
            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
            {
                return BadRequest("Image file is required");
            }

            using var memoryStream = new MemoryStream();
            await dto.ImageFile.CopyToAsync(memoryStream);

            var accessory = new Accessory
            {
                AccessoryName = dto.AccessoryName,
                Brand = dto.Brand,
                Type = dto.Type,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                ImageFile = memoryStream.ToArray(), // store as byte array using "ImageFile"
                Description=dto.Description
            };

            _context.Accessories.Add(accessory);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAccessory), new { id = accessory.AccessoryId }, accessory);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAccessory(int id, [FromForm] AccessoryUpdateDto dto)
        {
            var existingAccessory = await _context.Accessories.FindAsync(id);
            if (existingAccessory == null)
            {
                return NotFound("Accessory not found");
            }

            existingAccessory.AccessoryName = dto.AccessoryName ?? existingAccessory.AccessoryName;
            existingAccessory.Brand = dto.Brand ?? existingAccessory.Brand;
            existingAccessory.Type = dto.Type ?? existingAccessory.Type;
            existingAccessory.Price = dto.Price ?? existingAccessory.Price;
            existingAccessory.StockQuantity = dto.StockQuantity ?? existingAccessory.StockQuantity;
            existingAccessory.Description = dto.Description ?? existingAccessory.Description;

            if (dto.ImageFile != null && dto.ImageFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await dto.ImageFile.CopyToAsync(memoryStream);
                existingAccessory.ImageFile = memoryStream.ToArray();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccessoryExists(id))
                {
                    return NotFound("Accessory not found");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool AccessoryExists(int id)
        {
            return _context.Accessories.Any(a => a.AccessoryId == id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccessory(int id)
        {
            var accessory = await _context.Accessories.FindAsync(id);
            if (accessory == null)
            {
                return NotFound("Accessory not found");
            }

            _context.Accessories.Remove(accessory);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
