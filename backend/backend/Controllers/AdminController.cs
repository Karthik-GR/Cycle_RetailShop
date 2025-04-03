using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using backend.DTO;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetEmployees()
        {
            var user = HttpContext.User.Identity;
            Console.WriteLine($"User Authenticated: {user.IsAuthenticated}, Name: {user.Name}");
            return await _context.Users.ToListAsync();
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] UpdatedUser_dto updatedUserDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Employee not found" });
            }

            if (!string.IsNullOrEmpty(updatedUserDto.Username)) user.Username = updatedUserDto.Username;
            if (!string.IsNullOrEmpty(updatedUserDto.Password)) user.Password = updatedUserDto.Password;
            if (!string.IsNullOrEmpty(updatedUserDto.Email)) user.email = updatedUserDto.Email;
            if (!string.IsNullOrEmpty(updatedUserDto.Role)) user.Role = updatedUserDto.Role;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee updated successfully", user });
        }



        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Employee not found" });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Employee deleted successfully" });
        }
    }
}