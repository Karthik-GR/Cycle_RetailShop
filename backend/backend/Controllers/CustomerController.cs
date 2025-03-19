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
    public class CustomerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Customer
        [HttpGet]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }
            return customer;
        }

        // POST: api/Customer
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Customer>> AddCustomer([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest(new { error = "Invalid customer data" });
            }

            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.phoneNumber == customer.phoneNumber);
            if (existingCustomer != null)
            {
                return BadRequest(new { error = "Phone number is already in use" });
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.customerId }, customer);
        }


        // PATCH: api/Customer/5
        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer customer)
        {
            var existingCustomer = await _context.Customers.FindAsync(id);
            if (existingCustomer == null)
            {
                return NotFound("Customer not found");
            }

            if(customer.phoneNumber!=existingCustomer.phoneNumber)
            {
                var phoneExists = await _context.Customers.AnyAsync(c => c.phoneNumber == customer.phoneNumber);
                if (phoneExists)
                {
                    return BadRequest("Phone number is already in use");
                }
            }

            // Update only if new values are provided
            existingCustomer.name = string.IsNullOrWhiteSpace(customer.name) ? existingCustomer.name : customer.name;
            existingCustomer.email = string.IsNullOrWhiteSpace(customer.email) ? existingCustomer.email : customer.email;
            existingCustomer.phoneNumber = customer.phoneNumber == 0 ? existingCustomer.phoneNumber : customer.phoneNumber;
            existingCustomer.address = string.IsNullOrWhiteSpace(customer.address) ? existingCustomer.address : customer.address;

            await _context.SaveChangesAsync();
            return Ok("Customer updated successfully");
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound("Customer not found");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return Ok("Customer deleted successfully");
        }
    }
}
