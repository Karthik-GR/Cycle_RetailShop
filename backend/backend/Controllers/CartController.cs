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
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cart
        // For demonstration, this returns the first available cart.
        // In a production scenario, you'd link carts to users.
        [HttpGet]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public async Task<ActionResult<Cart>> GetCart()
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(ci => ci.Inventory)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                // Create a new cart if none exists.
                cart = new Cart();
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            return Ok(cart);
        }

        // POST: api/Cart/AddItem
        [HttpPost("AddItem")]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public async Task<ActionResult<CartItem>> AddItemToCart([FromBody] CartItem newItem)
        {
            // For simplicity, using the first cart.
            var cart = await _context.Carts.FirstOrDefaultAsync();
            if (cart == null)
            {
                cart = new Cart();
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            newItem.CartId = cart.CartId;
            _context.CartItems.Add(newItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCart), new { id = newItem.CartItemId }, newItem);
        }

        // PATCH: api/Cart/UpdateItem/5
        [HttpPatch("UpdateItem/{id}")]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public async Task<IActionResult> UpdateCartItem(int id, [FromBody] CartItem updatedItem)
        {
            var existingItem = await _context.CartItems.FindAsync(id);
            if (existingItem == null)
            {
                return NotFound("Cart item not found");
            }
            existingItem.Quantity = updatedItem.Quantity;
            await _context.SaveChangesAsync();
            return Ok("Cart item updated successfully");
        }

        // DELETE: api/Cart/RemoveItem/5
        [HttpDelete("RemoveItem/{id}")]
        [Authorize(Roles = "Admin,Employee,Customer")]
        public async Task<IActionResult> RemoveCartItem(int id)
        {
            var item = await _context.CartItems.FindAsync(id);
            if (item == null)
            {
                return NotFound("Cart item not found");
            }
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return Ok("Cart item removed successfully");
        }
    }
}
