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
    public class CycleTypeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CycleTypeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CycleType>>> GetCycleTypes()
        {
            return await _context.CycleTypes.ToListAsync();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CycleType>> AddCycleType([FromBody] CycleType cycleType)
        {
            _context.CycleTypes.Add(cycleType);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCycleTypes), new { id = cycleType.CycleTypeId }, cycleType);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCycleType(int id, [FromBody] CycleType cycleType)
        {
            var existingType = await _context.CycleTypes.FindAsync(id);
            if (existingType == null)
            {
                return NotFound("Cycle Type not found");
            }

            existingType.CycleTypeName = cycleType.CycleTypeName ?? existingType.CycleTypeName;

            await _context.SaveChangesAsync();
            return Ok("Cycle Type updated successfully");
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCycleType(int id)
        {
            var cycleType = await _context.CycleTypes.FindAsync(id);
            if (cycleType == null)
            {
                return NotFound("Cycle Type not found");
            }

            _context.CycleTypes.Remove(cycleType);
            await _context.SaveChangesAsync();
            return Ok("Cycle Type deleted successfully");
        }
    }
}
