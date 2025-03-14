using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CycleBrandController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CycleBrandController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetCycleBrands()
        {
            return await _context.CycleBrands.ToListAsync();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Brand>> AddCycleBrand([FromBody] Brand brand)
        {
            _context.CycleBrands.Add(brand);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCycleBrands), new { id = brand.BrandId }, brand);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCycleBrand(int id, [FromBody] Brand brand)
        {
            var existingBrand = await _context.CycleBrands.FindAsync(id);
            if (existingBrand == null)
            {
                return NotFound("Brand not found");
            }

            existingBrand.BrandName = brand.BrandName ?? existingBrand.BrandName;

            await _context.SaveChangesAsync();
            return Ok("Brand updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCycleBrand(int id)
        {
            var brand = await _context.CycleBrands.FindAsync(id);
            if (brand == null)
            {
                return NotFound("Brand not found");
            }

            _context.CycleBrands.Remove(brand);
            await _context.SaveChangesAsync();
            return Ok("Brand deleted successfully");
        }
    }
}
